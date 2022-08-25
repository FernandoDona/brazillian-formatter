using System;
using System.Collections.Generic;

namespace Brazillian.Formatter
{
    public struct CPF : IEquatable<CPF>
    {
        private const int NumericDigitsSize = 11;
        private const int Size = 14;
        private const char Hyphen = '-';
        private const char Dot = '.';
        private static readonly IDictionary<int, char> _nonNumericCharsPositions = new Dictionary<int, char>
        {
            { 3, Dot },
            { 7, Dot },
            { 11, Hyphen }
        };
        private readonly string _value;

        private CPF(long value)
        {
            _value = Parse(value);
        }
        private CPF(string value)
        {
            _value = FormatString(value);
        }

        /// <summary>
        /// If the CPF is valid it will be returned a formatted CPF string
        /// </summary>
        /// <param name="input">CPF to be formatted</param>
        /// <exception cref="ArgumentException">If the input is not a valid CPF</exception>
        public static string FormatString(ReadOnlySpan<char> input)
        {
            if (TryFormatString(input, out string output) == false)
            {
                throw new ArgumentException("The CPF is not in a valid format.", nameof(input));
            }

            return output;
        }

        /// <summary>
        /// If the CPF is valid it will be returned a formatted CPF string
        /// </summary>
        /// <param name="input">CPF to be formatted</param>
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

        private static bool CheckCpfNumericRule(ReadOnlySpan<char> numericOnlyInput)
        {
            return NumericData.CheckIfAllNumbersAreNotTheSame(numericOnlyInput)
                && CheckVerificationDigits(numericOnlyInput);
        }

        private static bool CheckVerificationDigits(ReadOnlySpan<char> input, int digitsToCheck = 2)
        {
            if (digitsToCheck == 0)
                return true;

            double sum = 0;
            int multiplier = NumericDigitsSize - 1;

            for (int i = 2 - digitsToCheck; i < input.Length - digitsToCheck; i++)
            {
                sum += char.GetNumericValue(input[i]) * multiplier;
                multiplier--;
            }

            var rest = sum % NumericDigitsSize;
            var verificationDigit = NumericDigitsSize - rest;
            if (verificationDigit != (int)char.GetNumericValue(input[NumericDigitsSize - digitsToCheck]))
                return false;

            return CheckVerificationDigits(input, digitsToCheck - 1);
        }

        private static bool IsValid(ReadOnlySpan<char> input)
        {
            if (NumericData.CheckQuantityOfNumericChars(input, NumericDigitsSize) == false)
                return false;

            Span<char> numericOnlyInput = stackalloc char[NumericDigitsSize];
            NumericData.GetOnlyNumericValues(input, ref numericOnlyInput);

            return CheckCpfNumericRule(numericOnlyInput);
        }

        /// <summary>
        /// If the CPF is valid it will be returned a formatted CPF string
        /// </summary>
        /// <param name="input">CPF to be formatted</param>
        /// <exception cref="ArgumentException">If the input is not a valid CPF</exception>
        public static string Parse(long input)
        {
            if (TryParse(input, out string output) == false)
            {
                throw new ArgumentException("The CPF is not in a valid format.", nameof(input));
            }

            return output;
        }

        /// <summary>
        /// If the CPF is valid it will be returned a formatted CPF string
        /// </summary>
        /// <param name="input">CPF to be formatted</param>
        /// <param name="output">A formatted string</param>
        public static bool TryParse(long input, out string output)
        {
            output = null;

            if (IsValidRange(input) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            NumericData.TryParseNumberToSpanChar(input, ref numericSpan);

            Span<char> outputAsSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, numericSpan, ref outputAsSpan);

            output = new string(outputAsSpan);

            return true;
        }

        private static bool IsValidRange(long cpf)
        {
            return cpf > 9999999999 && cpf < 99999999999;
        }

        public override string ToString() => _value;
        public bool Equals(CPF other) => _value == other._value;
        public override bool Equals(object obj) => obj.ToString() == _value;
        public override int GetHashCode() => HashCode.Combine(_value);

        public static implicit operator CPF(string input) => new CPF(input);
        public static implicit operator CPF(long input) => new CPF(input);
    }
}
