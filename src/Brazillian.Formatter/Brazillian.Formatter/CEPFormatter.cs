using System;
using System.Collections.Generic;
using System.Text;

namespace Brazillian.Formatter
{
    public static class CEPFormatter
    {
        private const int NumericDigitsSize = 8;
        private const int CepSize = 9;
        private const char Hyphen = '-';
        private static readonly IDictionary<int, char> _nonNumericCharsPositions = new Dictionary<int, char>
        {
            { 5, Hyphen }
        };

        public static string FormatString(ReadOnlySpan<char> cep)
        {
            if (TryFormatString(cep, out string formattedCep) == false)
            {
                throw new ArgumentException("The CEP is not in a valid format.", nameof(cep));
            }

            return formattedCep;
        }

        public static bool TryFormatString(ReadOnlySpan<char> cep, out string? formattedCep)
        {
            formattedCep = null;
            
            if (IsValid(cep) == false)
                return false;

            Span<char> formattedCepSpan = stackalloc char[CepSize];
            NumericDataFormatter.FormatData(_nonNumericCharsPositions, cep, ref formattedCepSpan);

            formattedCep = new string(formattedCepSpan);
            return true;
        }

        private static bool IsValid(ReadOnlySpan<char> data)
        {
            return NumericDataFormatter.CheckMinimumSize(data, NumericDigitsSize) 
                && NumericDataFormatter.CheckQuantityOfNumericChars(data, NumericDigitsSize);
        }

        private static bool IsValid(int cep)
        {
            return cep >= 0 && cep <= 99999999;
        }
    }
}
