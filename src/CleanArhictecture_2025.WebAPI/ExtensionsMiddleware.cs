using CleanArhictecture_2025.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace CleanArhictecture_2025.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(u => u.UserName == "admin"))
            {
                AppUser user = new AppUser()
                {
                    UserName = "admin",
                    FirstName = "ahmet",
                    LastName = "gkdmer",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    CreateAt = DateTimeOffset.UtcNow,
                };

                user.CreateUserId = user.Id;
                userManager.CreateAsync(user, "1").Wait();
            }
        }
    }
}


// biz burada audit login neden bir daha yazma sebebimiz usermanager dbcontext kullanmıyor. Bu yüzden override ettiğimiz savechangesasync çalışmayacaktır. Manuel kendimiz değer veriyoruz