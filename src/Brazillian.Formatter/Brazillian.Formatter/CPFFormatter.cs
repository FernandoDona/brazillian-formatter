using System;
using System.Collections.Generic;
using System.Text;

namespace Brazillian.Formatter
{
    public static class CPFFormatter
    {
        private const int NumericDigitsSize = 11;
        private const int CpfSize = 14;
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

            Span<char> formattedCpfSpan = stackalloc char[CpfSize];
            NumericDataFormatter.FormatData(_nonNumericCharsPositions, cpf, ref formattedCpfSpan);

            formattedCpf = new string(formattedCpfSpan);
            return true;
        }

        private static bool CheckIfAllNumbersAreNotTheSame(ReadOnlySpan<char> numericOnlyCpf)
        {
            var lastChar = numericOnlyCpf[0];
            for (int i = 0; i < numericOnlyCpf.Length; i++)
            {
                if (lastChar != numericOnlyCpf[i])
                {
                    return true;
                }

                lastChar = numericOnlyCpf[i];
            }

            return false;
        }

        private static bool CheckCpfNumericRule(ReadOnlySpan<char> numericOnlyCpf)
        {
            return CheckIfAllNumbersAreNotTheSame(numericOnlyCpf) 
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
            if (NumericDataFormatter.CheckMinimumSize(cpf, NumericDigitsSize) == false 
                && NumericDataFormatter.CheckQuantityOfNumericChars(cpf, NumericDigitsSize) == false)
                return false;

            Span<char> numericOnlyCpf = stackalloc char[NumericDigitsSize];
            NumericDataFormatter.GetOnlyNumericValues(cpf, ref numericOnlyCpf);

            return CheckCpfNumericRule(numericOnlyCpf);
        }
    }
}
