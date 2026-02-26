using CleanArhictecture_2025.Application.Employees;
using CleanArhictecture_2025.Domain.Employees;
using MediatR;
using TS.Result;

namespace CleanArhictecture_2025.WebAPI.Modules;
public static class EmployeeModule
{
    public static void RegisterEmployeeRouter(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/employees").WithTags("Employees").RequireAuthorization();

        group.MapPost(string.Empty, async (ISender sender, EmployeeCreateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        })
            .Produces<Result<string>>();

        group.MapGet(string.Empty, async (ISender sender, Guid id, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new EmployeeGetQuery(id), cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).
            Produces<Result<Employee>>();
    }
}

// burada Result<string> döneceğini söylüyoruz, böylece scalarda artık ne döneceğini görebiliyorum.
// buradaki requireauthorization bize controllerda da token iste demek. Yani biz her controllerın başına tek tek authorizationschemme jwtbarear yazmamıza gerek kalmıyor.