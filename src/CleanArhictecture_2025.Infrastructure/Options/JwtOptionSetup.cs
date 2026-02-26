using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanArhictecture_2025.Infrastructure.Options;

public sealed class JwtOptionsSetup(
    IOptions<JwtOptions> jwtOptions) : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidateLifetime = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.ValidIssuer = jwtOptions.Value.Issuer;
        options.TokenValidationParameters.ValidAudience = jwtOptions.Value.Audience;
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));
    }
}


// bir şimdi jwt oluşturduk şimdi bunun doğrulanmasını program.cs da add autetication.addberaeriçerisinde option ile girerek appsettingjson daki bilgilerimizi girerek bunlara göre doğrula diyoruz. Ancak ben yine burada bir option patern kullanıcam.