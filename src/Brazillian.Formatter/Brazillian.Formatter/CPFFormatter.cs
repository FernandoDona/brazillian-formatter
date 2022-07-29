using System;
using System.Collections.Generic;
using System.Text;

namespace Brazillian.Formatter
{
    public struct CPFFormatter
    {
        private const int NumericDigitsSize = 11;
        private const int CpfSize = 14;
        private const char Hyphen = '-';
        private const char Dot = '.';

        public static string FormatString(ReadOnlySpan<char> cpf)
        {
            if (!IsValid(cpf))
            {
                throw new ArgumentException("The CPF is not in a valid format.", nameof(cpf));
            }
            
            Span<char> formattedCpf = stackalloc char[CpfSize];
            var addedChars = 0;
            
            for (int i = 0; i < formattedCpf.Length; i++)
            {
                if (i >= cpf.Length)
                    break;
                if (!char.IsNumber(cpf[i]))
                    continue;

                if (addedChars == 3 || addedChars == 7)
                {
                    formattedCpf[addedChars] = Dot;
                    addedChars++;
                }
                else if (addedChars == 11)
                {
                    formattedCpf[addedChars] = Hyphen;
                    addedChars++;
                }

                formattedCpf[addedChars] = cpf[i];
                addedChars++;
            }

            return new string(formattedCpf);
        }

        public static bool TryFormatString(ReadOnlySpan<char> cpf, out string? formattedCpf)
        {
            formattedCpf = null;

            if (!IsValid(cpf))
            {
                return false;
            }

            Span<char> formattedCpfSpan = stackalloc char[CpfSize];
            var addedChars = 0;

            for (int i = 0; i < formattedCpfSpan.Length; i++)
            {
                if (i >= cpf.Length)
                    break;
                if (!char.IsNumber(cpf[i]))
                    continue;

                if (addedChars == 3 || addedChars == 7)
                {
                    formattedCpfSpan[addedChars] = Dot;
                    addedChars++;
                }
                else if (addedChars == 11)
                {
                    formattedCpfSpan[addedChars] = Hyphen;
                    addedChars++;
                }

                formattedCpfSpan[addedChars] = cpf[i];
                addedChars++;
            }

            formattedCpf = new string(formattedCpfSpan);
            return true;
        }

        /// <summary>
        /// Check if the <see langword="string"/> is valid to be formatted as CPF and follow the CPF rules.
        /// </summary>
        /// <param name="cpf"><see langword="string"/> to be checked.</param>
        /// <returns><see langword="true"/> if follows the CPF rules and can be formatted as CPF. <see langword="false"/> if is not valid.</returns>
        public static bool IsValid(ReadOnlySpan<char> cpf)
        {
            if (!CheckNumberOfDigits(cpf))
                return false;

            Span<char> numericOnlyCpf = stackalloc char[NumericDigitsSize]; 
            GetOnlyNumericValues(cpf, ref numericOnlyCpf);

            return CheckCpfNumericRule(numericOnlyCpf);
        }

        private static bool CheckNumberOfDigits(ReadOnlySpan<char> cpf)
        {
            if (cpf.Length < NumericDigitsSize)
                return false;

            var numericCounter = 0;

            for (int i = 0; i < cpf.Length; i++)
            {
                if (char.IsLetter(cpf[i]))
                    return false;

                if (char.IsNumber(cpf[i]))
                    numericCounter++;

                if (numericCounter > NumericDigitsSize)
                    return false;
            }

            if (numericCounter == NumericDigitsSize)
                return true;

            return false;
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

        private static void GetOnlyNumericValues(ReadOnlySpan<char> cpfOrigin, ref Span<char> cpfResult)
        {
            var resultSize = 0;

            for (int i = 0; i < cpfOrigin.Length; i++)
            {
                if (char.IsNumber(cpfOrigin[i]) == false)
                    continue;

                cpfResult[resultSize] = cpfOrigin[i];
                resultSize++;
            }
        }
    }
}
