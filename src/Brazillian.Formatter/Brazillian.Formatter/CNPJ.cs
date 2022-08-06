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

        public static string FormatString(ReadOnlySpan<char> cnpj)
        {
            if (TryFormatString(cnpj, out string formattedCnpj) == false)
            {
                throw new ArgumentException("The CNPJ is not in a valid format.", nameof(cnpj));
            }

            return formattedCnpj;
        }

        public static bool TryFormatString(ReadOnlySpan<char> cnpj, out string? formattedCnpj)
        {
            formattedCnpj = null;

            if (IsValid(cnpj) == false)
                return false;

            Span<char> formattedSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, cnpj, ref formattedSpan);

            formattedCnpj = new string(formattedSpan);
            return true;
        }

        private static bool CheckCnpjNumericRule(ReadOnlySpan<char> numericOnlyCnpj)
        {
            return NumericData.CheckIfAllNumbersAreNotTheSame(numericOnlyCnpj)
                && CheckVerificationDigits(numericOnlyCnpj);
        }

        private static bool CheckVerificationDigits(ReadOnlySpan<char> cnpj, int digitsToCheck = 2)
        {
            if (digitsToCheck == 0)
                return true;

            double sum = 0;
            int multiplier = digitsToCheck == 2 ? 5 : 6;

            for (int i = 0; i < cnpj.Length - digitsToCheck; i++)
            {
                sum += char.GetNumericValue(cnpj[i]) * multiplier;
                multiplier--;

                if (multiplier == 1)
                    multiplier = 9;
            }

            var rest = sum % 11;
            var verificationDigit = rest == 0 || rest == 1 ? 0 : 11 - rest;
            if (verificationDigit != (int)char.GetNumericValue(cnpj[NumericDigitsSize - digitsToCheck]))
                return false;

            return CheckVerificationDigits(cnpj, digitsToCheck - 1);
        }

        public static string Parse(long cnpj)
        {
            if (TryParse(cnpj, out string formattedCnpj) == false)
            {
                throw new ArgumentException("The CNPJ is not in a valid format.", nameof(cnpj));
            }

            return formattedCnpj;
        }

        public static bool TryParse(long cnpj, out string formattedCnpj)
        {
            formattedCnpj = null;

            if (IsValidRange(cnpj) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            NumericData.TryParseNumberToSpanChar(cnpj, ref numericSpan);

            Span<char> formattedSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, numericSpan, ref formattedSpan);

            formattedCnpj = new string(formattedSpan);

            return true;
        }

        private static bool IsValidRange(long cnpj) => cnpj > 9999999999999 && cnpj < 99999999999999;

        private static bool IsValid(ReadOnlySpan<char> cnpj)
        {
            if (NumericData.CheckQuantityOfNumericChars(cnpj, NumericDigitsSize) == false)
                return false;

            Span<char> numericOnlyCnpj = stackalloc char[NumericDigitsSize];
            NumericData.GetOnlyNumericValues(cnpj, ref numericOnlyCnpj);

            return CheckCnpjNumericRule(numericOnlyCnpj);
        }

        public override string ToString() => _value;
        public bool Equals(CNPJ other) => _value == other._value;
        public override bool Equals(object obj) => obj.ToString() == _value;

        public static implicit operator CNPJ(string input) => new CNPJ(input);
        public static implicit operator CNPJ(long input) => new CNPJ(input);
    }
}
