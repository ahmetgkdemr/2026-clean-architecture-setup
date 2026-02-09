using CleanArhictecture_2025.Domain.Employees;
using CleanArhictecture_2025.Infrastructure.Context;
using CleanArhictecture_2025.Infrastructure.Repositories;
using GenericRepository;
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