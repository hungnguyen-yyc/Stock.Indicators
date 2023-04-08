namespace Skender.Stock.Indicators;

// VOLUME WEIGHTED MOVING AVERAGE (SERIES)
public static partial class Indicator
{
    internal static List<VwmaResult> CalcVwma(
        this List<QuoteD> qdList,
        int lookbackPeriods)
    {
        // check parameter arguments
        ValidateVwma(lookbackPeriods);

        // initialize
        int length = qdList.Count;
        List<VwmaResult> results = new(length);

        // roll through quotes
        for (int i = 0; i < length; i++)
        {
            QuoteD q = qdList[i];

            VwmaResult r = new(q.Date);
            results.Add(r);

            if (i + 1 >= lookbackPeriods)
            {
                double? sumCl = 0;
                double? sumVl = 0;
                for (int p = i + 1 - lookbackPeriods; p <= i; p++)
                {
                    QuoteD d = qdList[p];
                    double? c = d.Close;
                    double? v = d.Volume;

                    sumCl += c * v;
                    sumVl += v;
                }

                r.Vwma = sumVl != 0 ? (sumCl / sumVl) : null;
            }
        }

        return results;
    }

    internal static List<VwmaResult> CalcVwma(
        this List<QuoteD> qdList,
        int lookbackPeriods,
        CandlePart candlePart)
    {
        // check parameter arguments
        ValidateVwma(lookbackPeriods);

        // initialize
        int length = qdList.Count;
        List<VwmaResult> results = new(length);

        // roll through quotes
        for (int i = 0; i < length; i++)
        {
            QuoteD q = qdList[i];

            VwmaResult r = new(q.Date);
            results.Add(r);

            if (i + 1 >= lookbackPeriods)
            {
                double? sumCl = 0;
                double? sumVl = 0;
                for (int p = i + 1 - lookbackPeriods; p <= i; p++)
                {
                    QuoteD d = qdList[p];

                    double? c = GetPriceValue(d, candlePart);
                    double? v = d.Volume;

                    sumCl += c * v;
                    sumVl += v;
                }

                r.Vwma = sumVl != 0 ? (sumCl / sumVl) : null;
            }
        }

        return results;
    }

    private static double? GetPriceValue(QuoteD d, CandlePart candlePart)
    {
        double? c = d?.Close ?? d?.Low ?? d?.High ?? d?.Open;
        if (d == null || c == null)
        {
            return null;
        }

        if (candlePart == CandlePart.Close)
        {
            c = d.Close;
        }

        if (candlePart == CandlePart.High)
        {
            c = d.High;
        }

        if (candlePart == CandlePart.Low)
        {
            c = d.Low;
        }

        if (candlePart == CandlePart.Open)
        {
            c = d.Open;
        }

        if (candlePart == CandlePart.HLC3)
        {
            c = (d.High + d.Low + d.Close) / 3;
        }

        if (candlePart == CandlePart.HL2)
        {
            c = (d.High + d.Low) / 2;
        }

        if (candlePart == CandlePart.OHL3)
        {
            c = (d.Open + d.High + d.Low) / 3;
        }

        if (candlePart == CandlePart.OHLC4)
        {
            c = (d.High + d.Close + d.Close + d.Open) / 4;
        }

        return c;
    }

    // parameter validation
    private static void ValidateVwma(
        int lookbackPeriods)
    {
        // check parameter arguments
        if (lookbackPeriods <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lookbackPeriods), lookbackPeriods,
                "Lookback periods must be greater than 0 for Vwma.");
        }
    }
}
