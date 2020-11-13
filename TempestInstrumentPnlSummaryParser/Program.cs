using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.DependencyInjection;

namespace TempestInstrumentPnlSummaryParser
{
    internal class Program
    {
        static int Main(string[] args)
        {

            var cmd = new Command("run")
                {
                    new Argument<DateTime>("startDate", "The start date"),
                    new Argument<DateTime>("endDate", "The start date"),
                };
            cmd.Handler = CommandHandler.Create<DateTime, DateTime>((startDate, endDate) => Run(startDate, endDate));

            return new RootCommand("A simple parsing tool for Tempest instrument PnL summary")
            {
                cmd
            }.Invoke(args);
        }

        private static void Run(DateTime startDate, DateTime endDate)
        {
            using var runner = new Runner();
            runner.ServiceProvider.GetService<Parser>().Run(startDate, endDate);
        }
    }
}
