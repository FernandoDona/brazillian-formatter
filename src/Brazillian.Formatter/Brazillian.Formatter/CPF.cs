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
            NumericData.FormatData(_nonNumericCharsPositions, cpf, ref formattedCpfSpan);

            formattedCpf = new string(formattedCpfSpan);
            return true;
        }

        private static bool CheckCpfNumericRule(ReadOnlySpan<char> numericOnlyCpf)
        {
            return NumericData.CheckIfAllNumbersAreNotTheSame(numericOnlyCpf)
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

        private static bool IsValid(ReadOnlySpan<char> cpf)
        {
            if (NumericData.CheckQuantityOfNumericChars(cpf, NumericDigitsSize) == false)
                return false;

            Span<char> numericOnlyCpf = stackalloc char[NumericDigitsSize];
            NumericData.GetOnlyNumericValues(cpf, ref numericOnlyCpf);

            return CheckCpfNumericRule(numericOnlyCpf);
        }

        public static string Parse(long cpf)
        {
            if (TryParse(cpf, out string formattedCpf) == false)
            {
                throw new ArgumentException("The CPF is not in a valid format.", nameof(cpf));
            }

            return formattedCpf;
        }

        public static bool TryParse(long cpf, out string formattedCpf)
        {
            formattedCpf = null;

            if (IsValidRange(cpf) == false)
                return false;

            Span<char> numericSpan = stackalloc char[NumericDigitsSize];
            NumericData.TryParseNumberToSpanChar(cpf, ref numericSpan);

            Span<char> formattedSpan = stackalloc char[Size];
            NumericData.FormatData(_nonNumericCharsPositions, numericSpan, ref formattedSpan);

            formattedCpf = new string(formattedSpan);

            return true;
        }

        private static bool IsValidRange(long cpf)
        {
            return cpf > 9999999999 && cpf < 99999999999;
        }

        public override string ToString() => _value;
        public bool Equals(CPF other) => _value == other._value;
        public override bool Equals(object obj) => obj.ToString() == _value;

        public static implicit operator CPF(string input) => new CPF(input);
        public static implicit operator CPF(long input) => new CPF(input);
    }
}
