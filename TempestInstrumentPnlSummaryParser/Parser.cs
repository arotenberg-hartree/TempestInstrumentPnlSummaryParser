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
        public void Run()
        {
            logger.LogInformation("Running from base folder: {baseFolder}", settings.Value.ReportBaseFolder);
        }
    }
}
