using System;
using System.Collections.Generic;

namespace Brazillian.Formatter
{
    public struct CEP : IEquatable<CEP>
    {
        private const int NumericDigitsSize = 8;
        private const int Size = 9;
        private const char Hyphen = '-';
        private static readonly IDictionary<int, char> _nonNumericCharsPositions = new Dictionary<int, char>
        {
            { 5, Hyphen }
        };
        private readonly string _value;

        private CEP(int value)
        {
            _value = Parse(value);
        }
        private CEP(string value)
        {
            _value = FormatString(value);
        }

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

            Span<char> formattedSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, cep, ref formattedSpan);

            formattedCep = new string(formattedSpan);
            return true;
        }

        public static string Parse(int cep)
        {
            if (TryParse(cep, out string formattedCep) == false)
            {
                throw new ArgumentException("The CEP is not in a valid format.", nameof(cep));
            }

            return formattedCep;
        }

        public static bool TryParse(int cep, out string formattedCep)
        {
            formattedCep = null;

            if (IsValidRange(cep) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            numericSpan[0] = '0';
            NumericData.TryParseNumberToSpanChar(cep, ref numericSpan);

            Span<char> formattedSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, numericSpan, ref formattedSpan);

            formattedCep = new string(formattedSpan);

            return true;
        }
        
        private static bool IsValid(ReadOnlySpan<char> data)
        {
            return NumericData.CheckQuantityOfNumericChars(data, NumericDigitsSize);
        }

        private static bool IsValidRange(int cep) => cep >= 999999 && cep <= 99999999;

        public override string ToString() => _value;

        public bool Equals(CEP other) => _value == other._value;
        public override bool Equals(object obj) => obj.ToString() == _value;

        public static implicit operator CEP(string input) => new CEP(input);
        public static implicit operator CEP(int input) => new CEP(input);
    }
}
