using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazillian.Formatter.Tests;
public class CPFFormatterTests
{
    [Theory]
    [InlineData("162.s00.000-56")]
    [InlineData("111.111.111-11")]
    [InlineData("888.888.888-88")]
    [InlineData("345.134.163-23")]
    [InlineData("345.134.163-2332")]
    [InlineData("297.171.90-21")]
    public void ShouldThrowExceptionWhenTryToFormatInvalidCPF(string cpf)
    {
        Assert.Throws<ArgumentException>(() => CPFFormatter.FormatString(cpf));
    }

    [Theory]
    [InlineData("297.171.950-21")]
    [InlineData("29717195021")]
    [InlineData("29717195021   ")]
    [InlineData("297 171 950 21")]
    [InlineData("  297171 95021")]
    public void ShouldFormatCorrectly(string cpf)
    {
        Assert.Equal("297.171.950-21", CPFFormatter.FormatString(cpf));
    }

    [Theory]
    [InlineData("162.s00.000-56")]
    [InlineData("111.111.111-11")]
    [InlineData("888.888.888-88")]
    [InlineData("345.134.163-23")]
    [InlineData("345.134.163-2332")]
    [InlineData("297.171.90-21")]
    public void ShouldReturnFalseWhenTryToFormatInvalidCPF(string cpf)
    {
        Assert.False(CPFFormatter.TryFormatString(cpf, out _));
    }

    [Theory]
    [InlineData("297.171.950-21")]
    [InlineData("29717195021")]
    [InlineData("29717195021   ")]
    [InlineData("297 171 950 21")]
    [InlineData("  297171 95021")]
    public void ShouldReturnTrueIfCpfIsCorrect(string cpf)
    {
        Assert.True(CPFFormatter.TryFormatString(cpf, out _));
    }
}
