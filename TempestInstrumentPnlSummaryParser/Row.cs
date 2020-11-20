using System;

namespace TempestInstrumentPnlSummaryParser
{
    public readonly struct Row
    {
        public Row(DateTime date, string instrument, double ltdUnrealized, double ltdRealized, double ltdFinalized, double dtdUnrealized, double dtdRealized, double dtdFinalized, double mtdUnrealized, double mtdRealized, double mtdFinalized, double ytdUnrealized, double ytdRealized, double ytdFinalized)
        {
            Date = date;
            Instrument = instrument;
            LtdUnrealized = ltdUnrealized;
            LtdRealized = ltdRealized;
            LtdFinalized = ltdFinalized;
            DtdUnrealized = dtdUnrealized;
            DtdRealized = dtdRealized;
            DtdFinalized = dtdFinalized;
            MtdUnrealized = mtdUnrealized;
            MtdRealized = mtdRealized;
            MtdFinalized = mtdFinalized;
            YtdUnrealized = ytdUnrealized;
            YtdRealized = ytdRealized;
            YtdFinalized = ytdFinalized;
        }

        public DateTime Date { get; }
        public string Instrument { get; }
        public double LtdUnrealized { get; }
        public double LtdRealized { get; }
        public double LtdFinalized { get; }
        public double DtdUnrealized { get; }
        public double DtdRealized {get;}
        public double DtdFinalized {get;}
        public double MtdUnrealized { get; }
        public double MtdRealized {get;}
        public double MtdFinalized {get;}
        public double YtdUnrealized { get; }
        public double YtdRealized {get;}
        public double YtdFinalized {get;}        
    }
}
