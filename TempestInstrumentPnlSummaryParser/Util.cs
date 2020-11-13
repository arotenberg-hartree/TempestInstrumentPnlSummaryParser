using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace TempestInstrumentPnlSummaryParser
{
    public static class Util
    {
        public static Stream ToCsvStream<T>(this IEnumerable<T> data)
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            writer.WriteRecords(data);
            writer.Flush();
            streamWriter.Flush();
            stream.Position = 0;
            return stream;
        }
        
    }

}