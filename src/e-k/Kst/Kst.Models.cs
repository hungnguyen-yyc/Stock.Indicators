namespace Skender.Stock.Indicators;

[Serializable]
public sealed class KstResult : ResultBase, IReusableResult
{
    internal KstResult(DateTime date)
    {
        Date = date;
    }

    public double? Kst { get; set; }
    public double? Signal { get; set; }

    double? IReusableResult.Value => Kst;
}
