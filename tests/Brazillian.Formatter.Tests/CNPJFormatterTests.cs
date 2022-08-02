using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazillian.Formatter.Tests;
public class CNPJFormatterTests
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
        Assert.Throws<ArgumentException>(() => CNPJFormatter.FormatString(cnpj));
    }

    [Theory]
    [InlineData("28.640.571/0001-00")]
    [InlineData("89.491.969/0001-16")]
    [InlineData("65.487.395/0001-44")]
    [InlineData("32.598.657/0001-99")]
    [InlineData("32598657000199")]
    [InlineData("  32 598 657  0001 99  ")]
    public void ShouldFormatCorrectly(string cnpj)
    {
        var correctResultList = new List<string> {
            "28.640.571/0001-00",
            "89.491.969/0001-16",
            "65.487.395/0001-44",
            "32.598.657/0001-99"
        };

        Assert.Contains(CNPJFormatter.FormatString(cnpj), correctResultList);
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
        Assert.False(CNPJFormatter.TryFormatString(cnpj, out _));
    }

    [Theory]
    [InlineData("28.640.571/0001-00")]
    [InlineData("89.491.969/0001-16")]
    [InlineData("65.487.395/0001-44")]
    [InlineData("32.598.657/0001-99")]
    [InlineData("32598657000199")]
    [InlineData("  32 598 657  0001 99  ")]
    public void ShouldReturnTrueIfCnpjIsCorrect(string cnpj)
    {
        Assert.True(CNPJFormatter.TryFormatString(cnpj, out _));
    }
}
