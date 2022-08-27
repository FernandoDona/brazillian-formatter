using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazillian.Formatter.Tests;
public class CPFTests
{
    [Theory]
    [InlineData("162.s00.000-56")]
    [InlineData("111.111.111-11")]
    [InlineData("888.888.888-88")]
    [InlineData("345.134.163-23")]
    [InlineData("32.598.657/0001-99")]
    [InlineData("345.134.163-2332")]
    [InlineData("297.171.90-21")]
    public void ShouldThrowExceptionWhenTryToFormatInvalidCPF(string cpf)
    {
        CPF newCpf;
        Assert.Throws<ArgumentException>(() => newCpf = cpf);
    }

    [Theory]
    [InlineData("297.171.950-21")]
    [InlineData("29717195021")]
    [InlineData("29717195021   ")]
    [InlineData("297 171 950 21")]
    [InlineData("  297171 95021")]
    public void ShouldFormatCorrectly(string cpf)
    {
        CPF newCpf = cpf;
        Assert.Equal("297.171.950-21", newCpf);
    }

    [Theory]
    [InlineData("162.s00.000-56")]
    [InlineData("111.111.111-11")]
    [InlineData("888.888.888-88")]
    [InlineData("345.134.163-23")]
    [InlineData("345.134.163-2332")]
    [InlineData("32.598.657/0001-99")]
    [InlineData("297.171.90-21")]
    public void ShouldReturnFalseWhenTryToFormatInvalidCPF(string cpf)
    {
        Assert.False(CPF.TryFormatString(cpf, out _));
    }

    [Theory]
    [InlineData("297.171.950-21")]
    [InlineData("020.776.020-90")]
    [InlineData("29717195021   ")]
    [InlineData("297 171 950 21")]
    [InlineData("  297171 95021")]
    public void ShouldReturnTrueIfCpfIsCorrect(string cpf)
    {
        Assert.True(CPF.TryFormatString(cpf, out _));
    }

    [Theory]
    [InlineData(69853456036)]
    [InlineData(02077602090)]
    [InlineData(84463849063)]
    public void ShouldReturnTrueAndFormatLongToString(long cpf)
    {
        var correctResultList = new List<string>
        {
            "698.534.560-36",
            "020.776.020-90",
            "844.638.490-63"
        };

        var succeeded = CPF.TryParse(cpf, out var formattedCpf);
        Assert.Contains(formattedCpf, correctResultList);
        Assert.True(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(9097099900)]
    [InlineData(34513416323)]
    [InlineData(909709990033)]
    public void ShouldReturnFalseWhenTryParseLongToString(long cpf)
    {
        var succeeded = CPF.TryParse(cpf, out _);
        Assert.False(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(9097099900)]
    [InlineData(34513416323)]
    [InlineData(909709990033)]
    public void ShouldThrowExceptionWhenTryToParseLongToString(long cpf)
    {
        CPF newCpf;
        Assert.Throws<ArgumentException>(() => newCpf = cpf);
    }
}
