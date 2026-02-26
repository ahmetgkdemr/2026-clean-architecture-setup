using CleanArhictecture_2025.Application.Services;
using CleanArhictecture_2025.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace CleanArhictecture_2025.Application.Auth;
public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;
}

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserNameOrEmail || u.Email == request.UserNameOrEmail, cancellationToken);
        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı bulunamadı.");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
                return (500, $"Şifrenizi 5 defa yanlış girdiğiniz için kullanıcı {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika süreyle bloke edilmiştir");
            else
                return (500, "Kullanıcınız 5 kez yanlış şifre girdiği için 5 dakika süreyle bloke edilmiştir");
        }

        if (signInResult.IsNotAllowed)
        {
            return (500, "Mail adresiniz onaylı değil");
        }

        if (!signInResult.Succeeded)
        {
            return (500, "Şifreniz yanlış");
        }


        //token üret

        var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);

        var response = new LoginCommandResponse()
        {
            AccessToken = token
        };

        return response;
    }
}


// mesela parola geçersizse direkt hata fırlatıp konuyu kapatırdak eğer kullanıcı burada sonsuza kadar şifre denemeye çalışabilir.
// bunun önüne geçmek için Identity kütüphanesinde InfrastructureRegistrar.cs içinde opt ile içine girip password configurasyon ayarlarını düzenleyebildik.

// şimdi biz bir kullanıcı login işlemi yaptık ancak register yapmadık yapmayacağız. Bunu program cs içinde kendimi manuel ekleyeceğiz. Kullanıcılarımızı admin kendisi manuel ekliyor register yok.