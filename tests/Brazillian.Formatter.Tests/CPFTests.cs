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
    [InlineData("29717195021")]
    [InlineData("29717195021   ")]
    [InlineData("297 171 950 21")]
    [InlineData("  297171 95021")]
    public void ShouldReturnTrueIfCpfIsCorrect(string cpf)
    {
        Assert.True(CPF.TryFormatString(cpf, out _));
    }

    [Theory]
    [InlineData(50312283008)]
    [InlineData(69375907090)]
    [InlineData(90970999003)]
    public void ShouldReturnTrueAndFormatLongToString(long cnpj)
    {
        var correctResultList = new List<string>
        {
            "503.122.830-08",
            "693.759.070-90",
            "909.709.990-03"
        };

        var succeeded = CPF.TryParse(cnpj, out var formattedCpf);
        Assert.Contains(formattedCpf, correctResultList);
        Assert.True(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(9097099900)]
    [InlineData(909709990033)]
    public void ShouldReturnFalseWhenTryFormatLongToString(long cpf)
    {
        var succeeded = CPF.TryParse(cpf, out _);
        Assert.False(succeeded);
    }

    [Theory]
    [InlineData(162003456)]
    [InlineData(9097099900)]
    [InlineData(909709990033)]
    public void ShouldThrowExceptionWhenTryToFormatLongToString(long cpf)
    {
        CPF newCpf;
        Assert.Throws<ArgumentException>(() => newCpf = cpf);
    }
}
