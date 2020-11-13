using System;

namespace TempestInstrumentPnlSummaryParser
{
    public readonly struct Row
    {
        public Row(DateTime date, string instrument, double ltdUnrealized, double ltdRealized, double ltdFinalized)
        {
            Date = date;
            Instrument = instrument;
            LtdUnrealized = ltdUnrealized;
            LtdRealized = ltdRealized;
            LtdFinalized = ltdFinalized;
        }

        public DateTime Date { get; }
        public string Instrument { get; }
        public double LtdUnrealized { get; }
        public double LtdRealized { get; }
        public double LtdFinalized { get; }
    }
}
