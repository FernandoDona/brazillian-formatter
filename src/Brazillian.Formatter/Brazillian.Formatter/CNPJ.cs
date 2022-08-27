using System;
using System.Collections.Generic;

namespace Brazillian.Formatter
{
    public struct CNPJ : IEquatable<CNPJ>
    {
        private const int NumericDigitsSize = 14;
        private const int Size = 18;
        private const char Hyphen = '-';
        private const char Dot = '.';
        private const char Slash = '/';
        private static readonly IDictionary<int, char> _nonNumericCharsPositions = new Dictionary<int, char>
        {
            { 2, Dot },
            { 6, Dot },
            { 10, Slash },
            { 15, Hyphen }
        };
        private readonly string _value;

        private CNPJ(long value)
        {
            _value = Parse(value);
        }
        private CNPJ(string value)
        {
            _value = FormatString(value);
        }
        
        /// <summary>
        /// If the CNPJ is valid it will be returned a formatted CNPJ string
        /// </summary>
        /// <param name="input">CNPJ to be formatted</param>
        /// <exception cref="ArgumentException">If the input is not a valid CNPJ</exception>
        public static string FormatString(ReadOnlySpan<char> input)
        {
            if (TryFormatString(input, out string output) == false)
            {
                throw new ArgumentException("The CNPJ is not in a valid format.", nameof(input));
            }

            return output;
        }
        
        /// <summary>
        /// If the CNPJ is valid it will be returned a formatted CNPJ string
        /// </summary>
        /// <param name="input">CNPJ to be formatted</param>
        /// <param name="output">A formatted string</param>
        public static bool TryFormatString(ReadOnlySpan<char> input, out string? output)
        {
            output = null;

            if (IsValid(input) == false)
                return false;

            Span<char> outputAsSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, input, ref outputAsSpan);

            output = new string(outputAsSpan);
            return true;
        }

        private static bool CheckCnpjNumericRule(ReadOnlySpan<char> numericOnlyInput)
        {
            return NumericData.CheckIfAllNumbersAreNotTheSame(numericOnlyInput)
                && CheckVerificationDigits(numericOnlyInput);
        }

        private static bool CheckVerificationDigits(ReadOnlySpan<char> input, int digitsToCheck = 2)
        {
            if (digitsToCheck == 0)
                return true;

            double sum = 0;
            int multiplier = digitsToCheck == 2 ? 5 : 6;

            for (int i = 0; i < input.Length - digitsToCheck; i++)
            {
                sum += char.GetNumericValue(input[i]) * multiplier;
                multiplier--;

                if (multiplier == 1)
                    multiplier = 9;
            }

            var rest = sum % 11;
            var verificationDigit = 11 - rest;
            verificationDigit = verificationDigit >= 10 ? 0 : verificationDigit;
            if (verificationDigit != (int)char.GetNumericValue(input[NumericDigitsSize - digitsToCheck]))
                return false;

            return CheckVerificationDigits(input, digitsToCheck - 1);
        }

        /// <summary>
        /// If the CNPJ is valid it will be returned a formatted CNPJ string
        /// </summary>
        /// <param name="input">CNPJ to be formatted</param>
        /// <exception cref="ArgumentException">If the input is not a valid CNPJ</exception>
        public static string Parse(long input)
        {
            if (TryParse(input, out string output) == false)
            {
                throw new ArgumentException("The CNPJ is not in a valid format.", nameof(input));
            }

            return output;
        }

        /// <summary>
        /// If the CNPJ is valid it will be returned a formatted CNPJ string
        /// </summary>
        /// <param name="input">CNPJ to be formatted</param>
        /// <param name="output">A formatted string</param>
        public static bool TryParse(long input, out string output)
        {
            output = null;

            if (IsValidRange(input) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            NumericData.TryParseNumberToSpanChar(input, ref numericSpan);

            if (CheckCnpjNumericRule(numericSpan) == false)
                return false;

            Span<char> outputAsSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, numericSpan, ref outputAsSpan);

            output = new string(outputAsSpan);

            return true;
        }

        private static bool IsValidRange(long cnpj) => cnpj > 01_000_000_000_000 && cnpj < 99_999_999_999_999;

        private static bool IsValid(ReadOnlySpan<char> input)
        {
            if (NumericData.CheckQuantityOfNumericChars(input, NumericDigitsSize) == false)
                return false;

            Span<char> numericOnlyInput = stackalloc char[NumericDigitsSize];
            NumericData.GetOnlyNumericValues(input, ref numericOnlyInput);

            return CheckCnpjNumericRule(numericOnlyInput);
        }

        public override string ToString() => _value;
        public bool Equals(CNPJ other) => _value == other._value;
        public override bool Equals(object obj) => obj.ToString() == _value;
        public override int GetHashCode() => HashCode.Combine(_value);

        public static implicit operator CNPJ(string input) => new CNPJ(input);
        public static implicit operator CNPJ(long input) => new CNPJ(input);
    }
}
