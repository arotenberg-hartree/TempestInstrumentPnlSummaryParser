using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TempestInstrumentPnlSummaryParser
{
    public static class ServiceInstaller
    {
        public static void AddParser(this IServiceCollection services, IConfigurationSection section)
        {
            services.AddSingleton<Parser>();
            services.Configure<Settings>(section);
        }
    }
}
