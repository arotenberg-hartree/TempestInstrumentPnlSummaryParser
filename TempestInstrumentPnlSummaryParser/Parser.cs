using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
            logger.LogInformation("Running with settings: {settings}", JsonConvert.SerializeObject(settings.Value));
            logger.LogInformation("Start Date: {startDate:yyyy-MM-dd}", startDate);
            logger.LogInformation("End Date: {endDate:yyyy-MM-dd}", endDate);

            var files = GetFiles(startDate, endDate);

            
        }

        private List<(string, DateTime)> GetFiles(DateTime startDate, DateTime endDate)
        {
            logger.LogInformation("Locating files...");
            var entries = Directory.GetFileSystemEntries(settings.Value.ReportBaseFolder, settings.Value.FileSearchPattern, SearchOption.AllDirectories);
            var result = new List<(string, DateTime)>();
            foreach(var entry in entries)
            {
                var fi = new FileInfo(entry);
                var name = fi.Name;
                foreach(var r in settings.Value.FileNameStringsToReplace) name = name.Replace(r, string.Empty);
                if (!DateTime.TryParseExact(name, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime d)) continue;
                if (d < startDate) continue;
                if (d > endDate) continue;

                logger.LogInformation("Located file for {d:yyyy-MM-dd}: {entry}", d, entry);
                result.Add((entry, d));
            }
            
            logger.LogInformation("Located {count} files...", result.Count);
            return result;
        }
    }
}
