using Microsoft.Extensions.DependencyInjection;

namespace TempestInstrumentPnlSummaryParser
{
    class Program
    {
        static void Main()
        {
            using var runner = new Runner();
            runner.ServiceProvider.GetService<Parser>().Run();

        }
    }
}
