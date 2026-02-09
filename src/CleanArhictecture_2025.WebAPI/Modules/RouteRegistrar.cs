namespace CleanArhictecture_2025.WebAPI.Modules
{
    public static class RouteRegistrar
    {
        public static void RegisterRoutes(this IEndpointRouteBuilder app)
        {
            app.RegisterEmployeeRouter();
        }
    }
}


// burası ana route kaydedicisi, buraya tüm route kayıtlarımızı yapacağız