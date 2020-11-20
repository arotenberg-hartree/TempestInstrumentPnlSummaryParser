using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;

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
        public void Run(DateTime startDate, DateTime endDate, string outputCsv)
        {
            logger.LogInformation("Running with settings: {settings}", JsonConvert.SerializeObject(settings.Value));
            logger.LogInformation("Start Date: {startDate:yyyy-MM-dd}", startDate);
            logger.LogInformation("End Date: {endDate:yyyy-MM-dd}", endDate);
            logger.LogInformation("Output csv: {outputCsv}", outputCsv);

            var files = GetFiles(startDate, endDate);

            IEnumerable<Row> rows = GetData(files);
            File.Delete(outputCsv);
            using var fs = File.Create(outputCsv);
            using var sw = new StreamWriter(fs);
            using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);
            csv.WriteRecords(rows);
        }

        private IEnumerable<Row> GetData(List<(string File, DateTime Date)> files)
        {
            return files.SelectMany(GetRowsFromFile);
        }

        public IEnumerable<Row> GetRowsFromFile((string File, DateTime Date) file)
        {
            logger.LogInformation("Opening spreadsheet {file}", file.File);
            using var stream = new FileStream(file.File, FileMode.Open, FileAccess.Read);
            var pkg = new ExcelPackage(stream);
            var sheet = pkg.Workbook.Worksheets["Sheet1"];
            
            var count = 0;

            for(var row = 1; row < 1e6; row++)
            {
                if (string.IsNullOrWhiteSpace(sheet.Cells[row, 1].GetValue<string>())) continue;
                if (sheet.Cells[row, 1].GetValue<string>().StartsWith("GRAND TOTAL")) break;
                if (sheet.Cells[row, 1].GetValue<string>().StartsWith("TOTAL")) continue;
                
                var instrument = sheet.Cells[row,1].GetValue<string>();
                if (!double.TryParse(sheet.Cells[row, 2].GetValue<string>(), out double unrealizedLtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 3].GetValue<string>(), out double realizedLtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 4].GetValue<string>(), out double finalizedLtd)) continue;

                if (!double.TryParse(sheet.Cells[row, 6].GetValue<string>(), out double unrealizedDtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 7].GetValue<string>(), out double realizedDtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 8].GetValue<string>(), out double finalizedDtd)) continue;

                if (!double.TryParse(sheet.Cells[row, 10].GetValue<string>(), out double unrealizedMtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 11].GetValue<string>(), out double realizedMtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 12].GetValue<string>(), out double finalizedMtd)) continue;

                if (!double.TryParse(sheet.Cells[row, 13].GetValue<string>(), out double unrealizedYtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 14].GetValue<string>(), out double realizedYtd)) continue;
                if (!double.TryParse(sheet.Cells[row, 15].GetValue<string>(), out double finalizedYtd)) continue;

                count++;
                yield return new Row(file.Date, instrument, unrealizedLtd, realizedLtd, finalizedLtd, unrealizedDtd, realizedDtd, finalizedDtd, unrealizedMtd, realizedMtd, finalizedMtd, unrealizedYtd, realizedYtd, finalizedYtd);
            }

            logger.LogInformation("Finished processing spreadsheet {file}.  Found {count} rows.", file.File, count);
            
        }

        private List<(string File, DateTime Date)> GetFiles(DateTime startDate, DateTime endDate)
        {
            logger.LogInformation("Locating files...");
            var entries = Directory.GetFileSystemEntries(settings.Value.ReportBaseFolder, settings.Value.FileSearchPattern, SearchOption.AllDirectories);
            var result = new List<(string File, DateTime Date)>();
            foreach (var entry in entries)
            {
                var fi = new FileInfo(entry);
                var name = fi.Name;
                foreach (var r in settings.Value.FileNameStringsToReplace) name = name.Replace(r, string.Empty);
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
