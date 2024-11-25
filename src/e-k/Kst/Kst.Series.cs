namespace Skender.Stock.Indicators;

// KNOW SURE THING (SERIES)
public static partial class Indicator
{
    internal static List<KstResult> CalcKst(
        this List<QuoteD> prices,
        int rocLen1, int rocLen2, int rocLen3, int rocLen4,
        int smaLen1, int smaLen2, int smaLen3, int smaLen4,
        int signalPeriods)
    {
        var history = prices.Select(x => new Quote {
            Date = x.Date,
            High = (decimal)x.High,
            Low = (decimal)x.Low,
            Close = (decimal)x.Close,
            Open = (decimal)x.Open,
            Volume = (long)x.Volume
        }).ToList();
        var rcma1 = history.GetRoc(rocLen1, smaLen1).Select(x => x.RocSma ?? 0.0).ToList();
        var rcma2 = history.GetRoc(rocLen2, smaLen2).Select(x => x.RocSma ?? 0.0).ToList();
        var rcma3 = history.GetRoc(rocLen3, smaLen3).Select(x => x.RocSma ?? 0.0).ToList();
        var rcma4 = history.GetRoc(rocLen4, smaLen4).Select(x => x.RocSma ?? 0.0).ToList();

        var kst = new List<double>();
        for (var i = 0; i < history.Count; i++)
        {
            kst.Add(rcma1[i] * 1 + rcma2[i] * 2 + rcma3[i] * 3 + rcma4[i] * 4);
        }

        var signal = new List<double>();
        for (var i = 0; i < kst.Count; i++)
        {
            if (i < signalPeriods)
            {
                signal.Add(0.0);
            }
            else
            {
                signal.Add(kst.Skip(i - signalPeriods + 1).Take(signalPeriods).Average());
            }
        }

        List<KstResult> results = new(history.Count);
        for (var i = 0; i < history.Count; i++)
        {
            var h = history[i];
            KstResult result = new(h.Date)
            {
                Kst = kst[i],
                Signal = signal[i]
            };
            results.Add(result);
        }

        return results;
    }
}
