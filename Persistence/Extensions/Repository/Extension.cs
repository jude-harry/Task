namespace Persistence.Extensions.Repository
{
    public static class Extension
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAsyncRepository<TaskProject>, AsyncRepository<TaskProject>>();
            services.AddScoped<IAsyncRepository<User>, AsyncRepository<User>>();
            services.AddScoped<IAsyncRepository<Notification>, AsyncRepository<Notification>>();
            services.AddScoped<IAsyncRepository<Project>, AsyncRepository<Project>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}