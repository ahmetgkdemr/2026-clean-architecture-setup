using CleanArhictecture_2025.Domain.Employees;
using CleanArhictecture_2025.Domain.Users;
using CleanArhictecture_2025.Infrastructure.Context;
using CleanArhictecture_2025.Infrastructure.Options;
using CleanArhictecture_2025.Infrastructure.Repositories;
using GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace CleanArhictecture_2025.Infrastructure;

public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("PostgreSqlServer")!;
            opt.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());//unit of work ekliyoruz, dbcontext üzerinden alırız zaten)

        services
            .AddIdentity<AppUser, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequiredLength = 1; //şifreye 1 yazıp girilebilir.
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();


        //artık DI'a otomatik ekliyoruz servislerimi 
        services.Scan(opt => opt
        .FromAssemblies(typeof(InfrastructureRegistrar).Assembly)//mevcut katmanı tara
        .AddClasses(publicOnly: false)//classı public olmayan dahil hepsini listele
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)//eğer eklenmişse manuel skiple onu
        .AsImplementedInterfaces()//interface işaretleyenleri getir
        .WithScopedLifetime());//scoped life time ömrü ver burada

        return services;
    }
}


// Burada IConfiguration eklememizin sebebi: buradan connection bilgimizi okuyacağız
// GetConnectionString("PostgreSqlServer")! burada sonuna attığımız ünlemde null dönebilir diyor compiler, ben de diyorum ki kesinlikle null dönmeyecek sen hiç merak etme sonra hata vermiyor
// artık services.AddScoped<IEmployeeRepository, EmployeeRepository>(); şu yaklaşımda DI ekleme yapmıyoruz, bunun yerine scrutor kullanıyoruz. I harfi hariç kalanını otomatik okuyor ve ekliyor. Yukarıda hangi katmana ait assemblyleri nasıl kullanıyoruz gösterdim. 
// şimdi burada identity kütüphanesini kullandığımız için appuser sınıfı için, bunu dbcontext dışında add identity yazarak user ve role bilgilerimizi veriyoruz. Yani bu işlem bizim UserManager için bir DI işlemidir. UserManager kullanmayacaksan bunu yapmana gerek yok. DefaultTokenProviders için ise ben şifremi unuttum gibi işlemlerde token üretmek istediğim için yazıyorum. Tamammı UserManager kullanmak için yazılmakta.

// şimdi biz burada identity kütüphanesini kullanarak password configurasyonlarımızı yaptık veri tabanımıza kaydederken ancak şimdi bunu nasıl doğrulayacağız Handlerda. UserManager bunun doğrulamasını yapmaz o sadece giriş çıkışı kontrol eder. Biz burada SignInManager kullanarak doğrulama yapacağız. SignInManager, UserManager'ı kullanarak şifre doğrulaması yapar ve aynı zamanda lockout gibi işlemleri de yönetir. Bu sayede belirli sayıda yanlış şifre denemesi sonrası hesabın kilitlenmesini sağlayabiliriz.