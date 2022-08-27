using System;
using System.Collections.Generic;

namespace Brazillian.Formatter
{
    internal static class NumericData
    {
        internal static bool CheckQuantityOfNumericChars(ReadOnlySpan<char> data, int numericCharsinData)
        {
            if (data.Length < numericCharsinData)
                return false;

            var numericCounter = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (char.IsLetter(data[i]))
                    return false;

                if (char.IsNumber(data[i]))
                    numericCounter++;

                if (numericCounter > numericCharsinData)
                    return false;
            }

            if (numericCounter == numericCharsinData)
                return true;

            return false;
        }

        internal static void GetOnlyNumericValues(ReadOnlySpan<char> dataOrigin, ref Span<char> dataResult)
        {
            var resultSize = 0;

            for (int i = 0; i < dataOrigin.Length; i++)
            {
                if (resultSize >= dataResult.Length)
                    break;

                if (char.IsNumber(dataOrigin[i]) == false)
                    continue;

                dataResult[resultSize] = dataOrigin[i];
                resultSize++;
            }
        }

        internal static void FormatData(IDictionary<int, char> nonNumericCharsPositions, ReadOnlySpan<char> dataOrigin, ref Span<char> formattedData)
        {
            var addedChars = 0;

            for (int i = 0; i < dataOrigin.Length; i++)
            {
                if (!char.IsNumber(dataOrigin[i]))
                    continue;

                if (nonNumericCharsPositions.ContainsKey(addedChars))
                    formattedData[addedChars] = nonNumericCharsPositions[addedChars++];

                formattedData[addedChars++] = dataOrigin[i];
            }
        }

        internal static bool CheckIfAllNumbersAreNotTheSame(ReadOnlySpan<char> numericOnlyData)
        {
            var lastChar = numericOnlyData[0];
            for (int i = 0; i < numericOnlyData.Length; i++)
            {
                if (lastChar != numericOnlyData[i])
                {
                    return true;
                }

                lastChar = numericOnlyData[i];
            }

            return false;
        }

        internal static void TryParseNumberToSpanChar(long data, ref Span<char> numericOnlyData)
        {
            var index = numericOnlyData.Length - 1;
            while (data > 0)
            {
                var rest = data % 10;
                var restChar = Convert.ToChar(rest + '0');
                numericOnlyData[index--] = Convert.ToChar(restChar);
                data /= 10;
            }

            for (int i = 0; i < numericOnlyData.Length; i++)
            {
                if(numericOnlyData[i] == '\0')
                    numericOnlyData[i] = '0';

                if (numericOnlyData[i] != '\0')
                    break;
            }
        }
    }
}
