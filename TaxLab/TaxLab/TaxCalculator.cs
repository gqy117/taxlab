namespace TaxLab;

public record TaxBand(decimal BandStart, decimal BandFinish, decimal TaxRate)
{
    public decimal BandTaxAmount;
}

public record TaxBandYear(DateTimeOffset From, DateTimeOffset To, List<TaxBand> TaxBands)
{
}

public class TaxCalculator
{
    private readonly List<TaxBand> _taxBands;

    public TaxCalculator(TaxBandYear taxBandYear)
    {
        _taxBands = taxBandYear.TaxBands;
        BuildBandTaxAmount();
    }

    public decimal Calculate(decimal income)
    {
        for (int i = 0; i < _taxBands.Count; i++)
        {
            var currentBand = _taxBands[i];
            var previousBandAmount = i == 0 ? 0 : _taxBands[i - 1].BandTaxAmount;

            if (income > currentBand.BandStart && income <= (currentBand.BandFinish))
            {
                return Math.Round(previousBandAmount + (income - currentBand.BandStart) * currentBand.TaxRate, 2, MidpointRounding.AwayFromZero);
            }
        }

        return 0M;
    }

    private void BuildBandTaxAmount()
    {
        for (var i = 0; i < _taxBands.Count; i++)
        {
            var currentBand = _taxBands[i];
            var previousBand = i == 0 ? currentBand : _taxBands[i - 1];

            currentBand.BandTaxAmount = previousBand.BandTaxAmount + (currentBand.BandFinish - currentBand.BandStart) * currentBand.TaxRate;
        }
    }
}