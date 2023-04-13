using IdentityModel.Client;
using IdentityModel.Jwk;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetCore.Lti;
using NetCore.Lti.EntityFrameworkCore;
using NetCore.Lti.Samples.Spa;
using NetCore.Lti.Samples.Spa.Data;
using NetCore.Lti.Samples.Spa.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddSessionStateTempDataProvider();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(static options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);

    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

builder.Services.AddCors(static options => { options.AddPolicy(name: "ApiPolicy", static policy => { policy.WithOrigins("https://localhost:5173").AllowCredentials(); }); });
builder.Services.AddLti(options =>
    {
        var jwk = builder.Configuration.GetSection("LearningTool").GetValue<string>("Jwk");

        options.RedirectUri = "/lti/oidc/callback";
        options.Jwk = new JsonWebKey(jwk);
    })
    .AddEntityFrameworkRepositories<ApplicationDbContext>();

builder.Services.AddAuthentication(static options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
    .AddCookie(static options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Events.OnRedirectToLogin = static context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    })
    .AddOAuth(Constants.CanvasDockerName, options =>
    {
        options.ClientId = builder.Configuration["Authentication:Canvas:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Canvas:ClientSecret"];
        options.CallbackPath = "/login/canvas";
        options.AuthorizationEndpoint = "http://canvas.docker/login/oauth2/auth";
        options.TokenEndpoint = "http://canvas.docker/login/oauth2/token";
        options.SaveTokens = true;
    });

builder.Services.AddScoped<TokenClient>(static provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    var options = provider.GetRequiredService<IOptionsSnapshot<OAuthOptions>>().Get(Constants.CanvasDockerName);

    return new TokenClient(httpClient, new TokenClientOptions
    {
        Address = options.TokenEndpoint,
        ClientId = options.ClientId,
        ClientSecret = options.ClientSecret,
    });
});

builder.Services.AddHttpClient(Constants.CanvasDockerName, static (_, client) => { client.BaseAddress = new Uri("http://canvas.docker/"); });
builder.Services.AddHttpClient<ICanvasCourseService, CanvasCourseService>(Constants.CanvasDockerName)
    .AddAccessTokenManagement(Constants.CanvasDockerName);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("ApiPolicy");

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();