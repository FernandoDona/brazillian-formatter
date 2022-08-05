using System;
using System.Collections.Generic;

namespace Brazillian.Formatter
{
    public struct CNPJFormatter
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
            NumericDataFormatter.FormatData(_nonNumericCharsPositions, cnpj, ref formattedSpan);

            formattedCnpj = new string(formattedSpan);
            return true;
        }

        private static bool CheckCnpjNumericRule(ReadOnlySpan<char> numericOnlyCnpj)
        {
            return NumericDataFormatter.CheckIfAllNumbersAreNotTheSame(numericOnlyCnpj)
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

        public static string ParseToString(long cnpj)
        {
            if (TryParseToString(cnpj, out string formattedCnpj) == false)
            {
                throw new ArgumentException("The CNPJ is not in a valid format.", nameof(cnpj));
            }

            return formattedCnpj;
        }

        public static bool TryParseToString(long cnpj, out string formattedCnpj)
        {
            formattedCnpj = null;

            if (IsValid(cnpj) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            NumericDataFormatter.TryParseNumberToSpanChar(cnpj, ref numericSpan);

            Span<char> formattedSpan = stackalloc char[Size];
            NumericDataFormatter.FormatData(_nonNumericCharsPositions, numericSpan, ref formattedSpan);

            formattedCnpj = new string(formattedSpan);

            return true;
        }

        private static bool IsValid(long cnpj)
        {
            return cnpj > 9999999999999 && cnpj < 99999999999999;
        }

        public static bool IsValid(ReadOnlySpan<char> cnpj)
        {
            if (NumericDataFormatter.CheckQuantityOfNumericChars(cnpj, NumericDigitsSize) == false)
                return false;

            Span<char> numericOnlyCnpj = stackalloc char[NumericDigitsSize];
            NumericDataFormatter.GetOnlyNumericValues(cnpj, ref numericOnlyCnpj);

            return CheckCnpjNumericRule(numericOnlyCnpj);
        }
    }
}
