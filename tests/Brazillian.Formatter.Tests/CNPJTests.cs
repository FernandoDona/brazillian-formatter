using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazillian.Formatter.Tests;
public class CNPJTests
{
    [Theory]
    [InlineData("22.644.521/0001-20")]
    [InlineData("43.276.153/0001-03")]
    [InlineData("22.222.222/2222-22")]
    [InlineData("86.456.345/0002-14")]
    [InlineData("297.171.950-21")]
    [InlineData("96934594562194")]
    public void ShouldThrowExceptionWhenTryToFormatInvalidCNPJ(string cnpj)
    {
        CNPJ newCnpj;
        Assert.Throws<ArgumentException>(() => newCnpj = cnpj);
    }

    [Theory]
    [InlineData("28.640.571/0001-00")]
    [InlineData("89.491.969/0001-16")]
    [InlineData("65.487.395/0001-44")]
    [InlineData("05.349.012/0001-40")]
    [InlineData("32.598.657/0001-99")]
    [InlineData("32598657000199")]
    [InlineData("  32 598 657  0001 99  ")]
    public void ShouldFormatCorrectly(string cnpj)
    {
        var correctResultList = new List<string> {
            "28.640.571/0001-00",
            "89.491.969/0001-16",
            "65.487.395/0001-44",
            "32.598.657/0001-99",
            "05.349.012/0001-40"
        };
        CNPJ newCnpj = cnpj;
        Assert.Contains(newCnpj.ToString(), correctResultList);
    }

    [Theory]
    [InlineData("22.644.521/0001-20")]
    [InlineData("43.276.153/0001-03")]
    [InlineData("22.222.222/2222-22")]
    [InlineData("86.456.345/0002-14")]
    [InlineData("297.171.950-21")]
    [InlineData("96934594562194")]
    public void ShouldReturnFalseWhenTryToFormatInvalidCNPJ(string cnpj)
    {
        Assert.False(CNPJ.TryFormatString(cnpj, out _));
    }

    [Theory]
    [InlineData("28.640.571/0001-00")]
    [InlineData("89.491.969/0001-16")]
    [InlineData("32.598.657/0001-99")]
    [InlineData("32598657000199")]
    [InlineData("  32 598 657  0001 99  ")]
    public void ShouldReturnTrueIfCnpjIsCorrect(string cnpj)
    {
        Assert.True(CNPJ.TryFormatString(cnpj, out _));
    }

    [Theory]
    [InlineData(69841968000147)]
    [InlineData(39702266000111)]
    [InlineData(17641528000142)]
    [InlineData(05349012000140)]
    public void ShouldReturnTrueAndFormatLongToString(long cnpj)
    {
        var correctResultList = new List<string>
        {
            "69.841.968/0001-47",
            "39.702.266/0001-11",
            "17.641.528/0001-42",
            "05.349.012/0001-40"
        };

        var succeeded = CNPJ.TryParse(cnpj, out var formattedCep);
        Assert.Contains(formattedCep, correctResultList);
        Assert.True(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(6984196800147)]
    [InlineData(397022660001113)]
    public void ShouldReturnFalseWhenTryFormatParseToString(long cnpj)
    {
        var succeeded = CNPJ.TryParse(cnpj, out var formattedCnpj);
        Assert.False(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(6984196800147)]
    [InlineData(397022660001113)]
    public void ShouldThrowExceptionWhenTryToParseLongToString(long cnpj)
    {
        CNPJ newCnpj;
        Assert.Throws<ArgumentException>(() => newCnpj = cnpj);
    }
}
