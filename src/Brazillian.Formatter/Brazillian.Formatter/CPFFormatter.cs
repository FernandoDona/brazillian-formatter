using System;
using System.Collections.Generic;

namespace Brazillian.Formatter
{
    public static class CPFFormatter
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


        public static string FormatString(ReadOnlySpan<char> cpf)
        {
            if (TryFormatString(cpf, out string formattedCpf) == false)
            {
                throw new ArgumentException("The CPF is not in a valid format.", nameof(cpf));
            }

            return formattedCpf;
        }

        public static bool TryFormatString(ReadOnlySpan<char> cpf, out string? formattedCpf)
        {
            formattedCpf = null;

            if (IsValid(cpf) == false)
                return false;

            Span<char> formattedCpfSpan = stackalloc char[Size];
            NumericDataFormatter.FormatData(_nonNumericCharsPositions, cpf, ref formattedCpfSpan);

            formattedCpf = new string(formattedCpfSpan);
            return true;
        }

        private static bool CheckCpfNumericRule(ReadOnlySpan<char> numericOnlyCpf)
        {
            return NumericDataFormatter.CheckIfAllNumbersAreNotTheSame(numericOnlyCpf)
                && CheckVerificationDigits(numericOnlyCpf);
        }

        private static bool CheckVerificationDigits(ReadOnlySpan<char> cpf, int digitsToCheck = 2)
        {
            if (digitsToCheck == 0)
                return true;

            double sum = 0;
            int multiplier = NumericDigitsSize - 1;

            for (int i = 2 - digitsToCheck; i < cpf.Length - digitsToCheck; i++)
            {
                sum += char.GetNumericValue(cpf[i]) * multiplier;
                multiplier--;
            }

            var rest = sum % NumericDigitsSize;
            var verificationDigit = NumericDigitsSize - rest;
            if (verificationDigit != (int)char.GetNumericValue(cpf[NumericDigitsSize - digitsToCheck]))
                return false;

            return CheckVerificationDigits(cpf, digitsToCheck - 1);
        }

        public static bool IsValid(ReadOnlySpan<char> cpf)
        {
            if (NumericDataFormatter.CheckQuantityOfNumericChars(cpf, NumericDigitsSize) == false)
                return false;

            Span<char> numericOnlyCpf = stackalloc char[NumericDigitsSize];
            NumericDataFormatter.GetOnlyNumericValues(cpf, ref numericOnlyCpf);

            return CheckCpfNumericRule(numericOnlyCpf);
        }

        public static string ParseToString(long cpf)
        {
            if (TryParseToString(cpf, out string formattedCpf) == false)
            {
                throw new ArgumentException("The CPF is not in a valid format.", nameof(cpf));
            }

            return formattedCpf;
        }

        public static bool TryParseToString(long cpf, out string formattedCpf)
        {
            formattedCpf = null;

            if (IsValid(cpf) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            NumericDataFormatter.TryParseNumberToSpanChar(cpf, ref numericSpan);

            Span<char> formattedSpan = stackalloc char[Size];
            NumericDataFormatter.FormatData(_nonNumericCharsPositions, numericSpan, ref formattedSpan);

            formattedCpf = new string(formattedSpan);

            return true;
        }

        private static bool IsValid(long cpf)
        {
            return cpf > 9999999999 && cpf < 99999999999;
        }
    }
}
