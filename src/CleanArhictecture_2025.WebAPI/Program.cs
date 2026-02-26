using CleanArhictecture_2025.Infrastructure;
using CleanArhictecture_2025.Application;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.OData;
using CleanArhictecture_2025.WebAPI.Controllers;
using CleanArhictecture_2025.WebAPI.Modules;
using CleanArhictecture_2025.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddOData(opt => opt
    .Select()
    .Filter()
        .OrderBy()
        .SetMaxTop(100)
        .Count()
        .Expand()
        .AddRouteComponents("odata", AppODataController.GetEdmModel())
    );

builder.Services.AddRateLimiter(opt => opt.AddFixedWindowLimiter("fixed", cfg =>
{
    cfg.QueueLimit = 100;
    cfg.Window = TimeSpan.FromSeconds(1);
    cfg.PermitLimit = 100;
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();


var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapDefaultEndpoints();

app.UseHttpsRedirection(); //ssl brave

app.UseCors(opt => opt
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(t => true));

app.RegisterRoutes();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression(); //size down =>fast list

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

ExtensionsMiddleware.CreateFirstUser(app);

app.Run();


// open api endpointerimizi json formatina ceviriyor, scalar ise bunu gorsellestirmemizi sagliyor
// open api icin mutlaka cors polity yazilmasi gerekiyor
// bundan sonra artik yazacagim her controller fixed rate limit icerisine girmis oluyor
// create updadte delete icin minimal api kullanacagiz daha performansli. GetAll icin OData kullanacagiz.
// buradaki Addjwtbearer içerisinde option olmamasinin sebebi ben bunu jwtoptionssetup icerisinde yazdim
// mapcontrollers devamina dazdigimiz requireauthrorization bizim tüm controllarimizzda bearer token kontrol etmesini sagliyor
// ancak modullerdekini bearerlari control etmez bu yuzden gidip modullerdeki mapgroup devamina bunu yine ekliyoruz