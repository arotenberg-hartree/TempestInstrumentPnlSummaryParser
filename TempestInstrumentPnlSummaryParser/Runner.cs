using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TempestInstrumentPnlSummaryParser
{
    public class Runner : IDisposable
    {
        static Runner()
        {
            // https://lowleveldesign.org/2013/05/16/be-careful-with-varchars-in-dapper/
            Dapper.SqlMapper.AddTypeMap(typeof(string), System.Data.DbType.AnsiString);
        }

        public ServiceProvider ServiceProvider { get; }

        public Runner()
        {

            var services = new ServiceCollection();
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var logOutputTemplate = "[{Timestamp:HH:mm:ss.fff} {Level:u3} {SourceContext:l}] {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()

                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: logOutputTemplate)
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .CreateLogger();

            services.AddLogging(configure => configure.AddSerilog());
            services.AddParser(config.GetSection("Parser"));
            ServiceProvider = services.BuildServiceProvider();
        }
        public void Dispose()
        {
            ServiceProvider.Dispose();
        }

    }
}
