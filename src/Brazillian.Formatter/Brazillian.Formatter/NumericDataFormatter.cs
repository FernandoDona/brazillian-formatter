﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Brazillian.Formatter
{
    internal static class NumericDataFormatter
    {
        internal static bool CheckMinimumSize(ReadOnlySpan<char> data, int minimunSize) => data.Length >= minimunSize;

        internal static bool CheckQuantityOfNumericChars(ReadOnlySpan<char> data, int numericCharsinData)
        {
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
    }
}