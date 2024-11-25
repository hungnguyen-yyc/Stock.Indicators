namespace Skender.Stock.Indicators;

// KNOW SURE THING (API)
public static partial class Indicator
{
    public static IEnumerable<KstResult> GetKst<TQuote>(
        this IEnumerable<TQuote> quotes,
        int rocLen1 = 10,
        int rocLen2 = 15,
        int rocLen3 = 20,
        int rocLen4 = 30,
        int smaLen1 = 10,
        int smaLen2 = 10,
        int smaLen3 = 10,
        int smaLen4 = 15,
        int signalPeriods = 9)
        where TQuote : IQuote => quotes
            .ToQuoteD()
            .CalcKst(rocLen1, rocLen2, rocLen3, rocLen4, smaLen1, smaLen2, smaLen3, smaLen4, signalPeriods);
}
