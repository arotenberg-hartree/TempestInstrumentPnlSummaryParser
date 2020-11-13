using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TempestInstrumentPnlSummaryParser
{
    public class Parser
    {
        private readonly ILogger<Parser> logger;
        private readonly IOptions<Settings> settings;

        public Parser(ILogger<Parser> logger, IOptions<Settings> settings)
        {
            this.logger = logger;
            this.settings = settings;
        }
        public void Run(DateTime startDate, DateTime endDate)
        {
            logger.LogInformation("Running from base folder: {baseFolder}", settings.Value.ReportBaseFolder);
            logger.LogInformation("Start Date: {startDate:yyyy-MM-dd}", startDate);
            logger.LogInformation("End Date: {endDate:yyyy-MM-dd}", endDate);
            
        }
    }
}
