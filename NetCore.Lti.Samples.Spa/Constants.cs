namespace NetCore.Lti.Samples.Spa;

public static class Constants
{
    public const string CanvasDockerName = "canvas-docker";
    public static readonly ToolPlatform CanvasDockerToolPlatform = new(
        "<insert_tool_platform_guid>", // See https://www.imsglobal.org/spec/lti/v1p3#platform-instance-claim
        "<insert_name>",
        "https://canvas.instructure.com",
        "<insert_client_id>",
        new Uri("http://canvas.docker/login/oauth2/token"),
        new Uri("http://canvas.docker/api/lti/authorize_redirect"),
        new Uri("https://localhost:7084/lti/oidc/callback"),
        new Uri("http://canvas.docker/api/lti/security/jwks"),
        "2018-05-18T22:33:20Z"
    );
}