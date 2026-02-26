namespace CleanArhictecture_2025.Infrastructure.Options;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
}


// burada aslında appsettingdeki bilgileri optionpattern ile jwt provider da kullanmaya çalıştık eğer istersen IConfiguration ile de hızlıca kullanabilirsin