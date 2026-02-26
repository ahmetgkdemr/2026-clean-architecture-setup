namespace CleanArhictecture_2025.WebAPI.Modules
{
    public static class RouteRegistrar
    {
        public static void RegisterRoutes(this IEndpointRouteBuilder app)
        {
            app.RegisterEmployeeRouter();
            app.RegisterAuthRoutes();
        }
    }
}


// burası ana route kaydedicisi, buraya tüm route kayıtlarımızı yapacağız
// sonrasında bu dosyayı Program.cs dosyamızda çağırarak uygulamamızın route kayıtlarını yapacağız.