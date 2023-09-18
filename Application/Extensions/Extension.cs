namespace Application.Extensions
{
    public static class Extension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITracksService, TracksService>();
            services.AddScoped<IUser, UserService>();
            services.AddScoped<IProject, ProjectService>();
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}