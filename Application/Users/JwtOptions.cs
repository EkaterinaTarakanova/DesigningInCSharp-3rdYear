namespace SchedulePlanner.Application.Users;

public class JwtOptions
{
    public TimeSpan Expires { get; set; }
    public string? SecretKey { get; set; }
}