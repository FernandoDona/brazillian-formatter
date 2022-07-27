using System;
using System.Collections.Generic;
using System.Text;

namespace Brazillian.Formatter
{
    public struct CEPFormatter
    {
        private const int NumberOfDigitsInCep = 8;
        private const char Hyphen = '-';

        public static string FormatString(ReadOnlySpan<char> cep)
        {
            if (!IsValid(cep))
            {
                throw new ArgumentException("The CEP is not in a valid format.", nameof(cep));
            }

            Span<char> formattedCep = stackalloc char[9];
            var addedChars = 0;

            for (int i = 0; i < cep.Length; i++)
            {
                if (!char.IsNumber(cep[i]))
                    continue;

                if (addedChars == 5)
                {
                    formattedCep[addedChars] = Hyphen;
                    addedChars++;
                }
                 
                formattedCep[addedChars] = cep[i];
                addedChars++;
            }

            return new string(formattedCep);
        }

        public static bool TryFormatString(ReadOnlySpan<char> cep, out string? formattedCep)
        {
            formattedCep = null;
            
            if (!IsValid(cep))
            {
                return false;
            }

            Span<char> formattedCepSpan = stackalloc char[9];
            var addedChars = 0;

            for (int i = 0; i < cep.Length; i++)
            {
                if (!char.IsNumber(cep[i]))
                    continue;

                if (addedChars == 5)
                {
                    formattedCepSpan[addedChars] = Hyphen;
                    addedChars++;
                }

                formattedCepSpan[addedChars] = cep[i];
                addedChars++;
            }

            formattedCep = new string(formattedCepSpan);
            return true;
        }

        public static bool IsValid(ReadOnlySpan<char> cep)
        {
            if (cep.Length < NumberOfDigitsInCep)
                return false;

            var numericCounter = 0;
            
            for (int i = 0; i < cep.Length; i++)
            {
                if (char.IsLetter(cep[i]))
                    return false;

                if (char.IsNumber(cep[i]))
                    numericCounter++;
            
                if (numericCounter > NumberOfDigitsInCep)
                    return false;
            }

            if (numericCounter == NumberOfDigitsInCep)
                return true;

            return false;
        }

        public static bool IsValid(int cep)
        {
            return cep >= 0 && cep <= 99999999;
        }
    }
}
