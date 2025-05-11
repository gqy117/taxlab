namespace TaxLab.Tests;

public class TaxCalculatorTests
{
    public static IEnumerable<object[]> TestData()
    {
        return new List<object[]>
        {
            new object[] { 0, 0 },
            new object[] { -11M, 0M },
            new object[] { 10_000M, 1150M },
            new object[] { 14_000.00M, 1610.00M },
            new object[] { 48_000.00M, 8_750.00M },
            new object[] { 49_000.00M, 9_065.00M },
            new object[] { 70_000.00M, 15_680.00M },
            new object[] { 71_555.00M, 16_232.03M },
            new object[] { 80_000.00M, 19_230.00M },
            new object[] { 120_000.00M, 33_430.00M },
            new object[] { 125_368.25M, 35_335.73M },
            new object[] { 1_171_555.12M, 406_732.07M },
        };
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void Calculate_ShouldReturn_0_WhenInputIs0OrNegative(decimal income, decimal expected)
    {
        // Arrange
        var taxCalculator = new TaxCalculator(new TaxBandYear(
            new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2026, 6, 1, 0, 0, 0, TimeSpan.Zero),
            [
                new TaxBand(0, 14_000.00M, 0.115M),
                new TaxBand(14_000.00M, 48_000.00M, 0.21M),
                new TaxBand(48_000.00M, 70_000.00M, 0.315M),
                new TaxBand(70_000.00M, Decimal.MaxValue, 0.355M)
            ])
        );

        // Act
        var result = taxCalculator.Calculate(income);

        // Assert
        Assert.Equal(expected, result);
    }
}