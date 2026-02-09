using CleanArhictecture_2025.Application.Employees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CleanArhictecture_2025.WebAPI.Controllers;

[Route("odata")] //burada başlangıçta odata olarak başlayacak
[ApiController]
[EnableQuery]
public class AppODataController(
    ISender sender) : ODataController
{

    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<EmployeeGetAllQueryResponse>("employees");
        return builder.GetEdmModel();
    }

    [HttpGet("employees")]
    public async Task<IQueryable<EmployeeGetAllQueryResponse>> GetAllEmployees(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new EmployeeGetAllQuery(), cancellationToken);
        return response;
    }
}

// ISender mediatr kütüphanesinden gelir yine _mediatr dan tek farkı publish yapmıyor, _mediatr hem send hem publish yaparken ISender sadece send işlemi yapmaktadır

// şimdi burada OData çalışır ancak OData sorgu yapabilmek için EnableQuery ekle bu sayede buradaki getalla ekstra sorgular atabilmemiz sağlar. Ancak sadece EnableQuery eklediğimizde bana toplam count vermez veri tabanı için. Bunun için Program.cs dosyasında AddController() devamında . diyerek AddOData(opt .... ) diye eklemeler yapıyoruz. Sonrasında . AddRouteComponents diyerek ("odata") diyerekten OData controllerım bu diyoruz. Program.cs de yazdım incele orayı. son olarak countun da gözükebilmesi için static IEdmModel yazıyorsun bitiyor bu kadar.


// public static IEdmModel GetEdmModel() bu yapı sayesinde 

// properties kısmındaki launchsetting.json dosyasındaki launchurl ile scalar/v1 yazarak bunun başlamasını sağlıyoruz. Swagger yerine bunu kullanıyoruz artık

// httpget isteğinde scaların geriye ne döneceğinin templatetini görmek için IActionResult yerine IQuarable<EmployeeGetAllQueryResponse> içerisinde repsponse döndüğünde artık scalar ekranımda geriye ne döneceğini görebiliyorum