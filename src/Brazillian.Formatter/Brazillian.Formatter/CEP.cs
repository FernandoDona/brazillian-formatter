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

        /// <summary>
        /// If the CEP is valid it will be returned a formatted CEP string
        /// </summary>
        /// <param name="input">CEP to be formatted</param>
        /// <exception cref="ArgumentException">If the input is not a valid CEP</exception>
        public static string FormatString(ReadOnlySpan<char> input)
        {
            if (TryFormatString(input, out string output) == false)
            {
                throw new ArgumentException("The CEP is not in a valid format.", nameof(input));
            }

            return output;
        }

        /// <summary>
        /// If the CEP is valid it will be returned a formatted CEP string
        /// </summary>
        /// <param name="input">CEP to be formatted</param>
        /// <param name="output">A formatted string</param>
        public static bool TryFormatString(ReadOnlySpan<char> input, out string? output)
        {
            output = null;

            if (IsValid(input) == false)
                return false;

            Span<char> formattedSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, input, ref formattedSpan);

            output = new string(formattedSpan);
            return true;
        }

        /// <summary>
        /// If the CEP is valid it will be returned a formatted CEP string
        /// </summary>
        /// <param name="input">CEP to be formatted</param>
        /// <exception cref="ArgumentException">If the input is not a valid CEP</exception>
        public static string Parse(int input)
        {
            if (TryParse(input, out string output) == false)
            {
                throw new ArgumentException("The CEP is not in a valid format.", nameof(input));
            }

            return output;
        }

        /// <summary>
        /// If the CEP is valid it will be returned a formatted CEP string
        /// </summary>
        /// <param name="input">CEP to be formatted</param>
        /// <param name="output">A formatted string</param>
        public static bool TryParse(int input, out string output)
        {
            output = null;

            if (IsValidRange(input) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            numericSpan[0] = '0';
            NumericData.TryParseNumberToSpanChar(input, ref numericSpan);

            Span<char> outputAsSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, numericSpan, ref outputAsSpan);

            output = new string(outputAsSpan);

            return true;
        }
        
        private static bool IsValid(ReadOnlySpan<char> input)
        {
            return NumericData.CheckQuantityOfNumericChars(input, NumericDigitsSize);
        }

        private static bool IsValidRange(int cep) => cep >= 999999 && cep <= 99999999;

        public override string ToString() => _value;

        public bool Equals(CEP other) => _value == other._value;
        public override bool Equals(object obj) => obj.ToString() == _value;
        public override int GetHashCode() => HashCode.Combine(_value);

        public static implicit operator CEP(string input) => new CEP(input);
        public static implicit operator CEP(int input) => new CEP(input);
    }
}
