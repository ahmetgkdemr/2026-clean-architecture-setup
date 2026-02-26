using CleanArhictecture_2025.Application.Services;
using CleanArhictecture_2025.Domain.Users;
using CleanArhictecture_2025.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArhictecture_2025.Infrastructure.Services;

public sealed class JwtProvider(
    IOptions<JwtOptions> options) : IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        List<Claim> claims = new()
        {
            new Claim("user-id",user.Id.ToString()) //user id claims içerisinde sakladık kullanacağız
        };

        var expires = DateTime.UtcNow.AddDays(1);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(securityToken);
        return Task.FromResult(token);
    }
}


// asenkron kullanım sebebi usera eğer refleshtoken vermek istersek, userManager dahil edip asenkron yapı kullanmamız gerekecekti. Eğer async kullanmasan bile Task.FromResult ile async yapıyı koruyabilirsin

// burada securitytoken direkt new edip configurasyon yazmassak içine yapı çalışır ancak bir bunu doğrulayamayız. Çünkü rastgele verilerden oluşuyor

// buradaki öncemli bilgileri appsetting.json dan alırken ICongiguration da kullanabilirdik ancak ben "option pattern" kullandım. 

// ID yi sonradan kullanacağımız için claim içerisine id ekledir.