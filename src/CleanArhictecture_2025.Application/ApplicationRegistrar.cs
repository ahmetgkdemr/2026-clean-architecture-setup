using CleanArhictecture_2025.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArhictecture_2025.Application;

public static class ApplicationRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf =>
        {
            //1. Application katmanındaki tüm Handler'ları bul ve kaydet
            conf.RegisterServicesFromAssembly(typeof(ApplicationRegistrar).Assembly);
            //2. Tüm request'lere otomatik ValidationBehavior ekle (pipeline)
            conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Bu "EmployeeCreateCommandValidator" bu tip gibi tüm class'ları otomatik buluyor:
        services.AddValidatorsFromAssembly(typeof(ApplicationRegistrar).Assembly);
        return services;
    }
}

//addopenbehavior ile tüm handlerların öncesinde validation behavior motorumuzu çalışacak şekilde ayarladık, zaten rulefor ile kurallar genelde cqrs de employee create klasörü içinde employeecreatecommandvalidator gibi gibi yazılıyor

// üstteki mediatr içinde alttaki ise validasyon kurallarını eklememiz için gerekiyor

//eğer burada validator ve mediatr eklemeseydin 50 tane handlerı buraya tek tek eklemen gerekirdi, ve aynı klasörlerindeki 50 adet validasyonu da eklemek zorunda kalırdın!!