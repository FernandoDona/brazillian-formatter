namespace Brazillian.Formatter.Tests;

public class CEPFormatterTests
{
    [Theory]
    [InlineData("16200000")]
    [InlineData("16200-000")]
    [InlineData("16200---000")]
    [InlineData(" 16200- --000  ")]
    public void ShouldFormatCorrectly(string cep)
    {
        Assert.Equal("16200-000", CEPFormatter.FormatString(cep));
    }

    [Theory]
    [InlineData("162s00000")]
    [InlineData("162003-000")]
    [InlineData("6200-000")]
    public void ShouldThrowExceptionWhenTryToFormatInvalidCEP(string cep)
    {
        Assert.Throws<ArgumentException>(() => CEPFormatter.FormatString(cep));
    }

    [Theory]
    [InlineData("16200000")]
    [InlineData("16200-000")]
    [InlineData("16200---000")]
    [InlineData(" 16200- --000  ")]
    public void ShouldReturnTrueAndFormatCorrectly(string cep)
    {
        var succeeded = CEPFormatter.TryFormatString(cep, out var formattedCep);
        Assert.Equal("16200-000", formattedCep);
        Assert.True(succeeded);
    }

    [Theory]
    [InlineData("162s00000")]
    [InlineData("162003-000")]
    [InlineData("6200-000")]
    public void ShouldReturnFalseWhenTryToFormatInvalidCEP(string cep)
    {
        var succeeded = CEPFormatter.TryFormatString(cep, out var formattedCep);
        Assert.Null(formattedCep);
        Assert.False(succeeded);
    }

    [Theory]
    [InlineData(16200456)]
    [InlineData(1620456)]
    public void ShouldReturnTrueAndFormatIntToString(int cep)
    {
        var correctResultList = new List<string>
        {
            "16200-456",
            "01620-456"
        };

        var succeeded = CEPFormatter.TryParseToString(cep, out var formattedCep);
        Assert.Contains(formattedCep, correctResultList);
        Assert.True(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(162034)]
    public void ShouldReturnFalseWhenTryFormatIntToString(int cep)
    {
        var succeeded = CEPFormatter.TryParseToString(cep, out var formattedCep);
        Assert.False(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(162034)]
    public void ShouldThrowExceptionWhenTryToFormatIntToString(int cep)
    {
        Assert.Throws<ArgumentException>(() => CEPFormatter.ParseToString(cep));
    }
}