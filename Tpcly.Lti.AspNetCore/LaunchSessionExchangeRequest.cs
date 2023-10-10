namespace Tpcly.Lti.AspNetCore;

public class LaunchSessionExchangeRequest
{
    public string State { get; set; } = null!;

    public string Token { get; set; } = null!;
}