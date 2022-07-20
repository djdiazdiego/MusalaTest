namespace DoItFast.WebApi.Extensions
{
    public static class SettingExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSettingsExtension(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        }
    }
}
