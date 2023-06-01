using System;
using IronBarCode;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BarcodeGenerator.Models
{
	partial class Barcode
	{
        public int CalculateDiameter(int D1, int D2)
        {
            int[] arrD = { 020, 025, 032, 040, 050, 063, 075, 090, 110, 125, 140, 160, 180, 200, 225, 250, 315 };
            int d1 = Array.IndexOf(arrD, D1);
            int d2 = Array.IndexOf(arrD, D2);
            int d = (d1 * 31) + d2;
            return d;
        }
        public int ModifyTime(int number)
        {
            int constDigit = 9;
            int first2Digit, firstDigit;
            int modifiedNumber;
            if (number > 99)
            {
                first2Digit = (number / 10);
                modifiedNumber = (constDigit * 100) + first2Digit;
                return modifiedNumber;
            }
            else
            {
                firstDigit = number / 10;
                int remainingDigits = number % 10;
                if (remainingDigits != 0)
                    modifiedNumber = (constDigit * 100) + (firstDigit * 10) + remainingDigits;
                else { modifiedNumber = (constDigit * 100) + number; }
                return modifiedNumber;
            }

        }
        public int GenerateDigit24(long first, long last)
        {
            int lastSumEven, lastSumOdd, digit24;
            int sumEvenFirst, sumOddFirst, sumEvenLast, sumOddLast;

            sumEvenFirst = Convert.ToInt32(sumEven(first));
            sumOddFirst = Convert.ToInt32(sumOdd(first));
            sumEvenLast = Convert.ToInt32(sumEven(last));
            sumOddLast = Convert.ToInt32(sumOdd(last));
            lastSumEven = sumEvenFirst + sumEvenLast;
            lastSumOdd = sumOddFirst + sumOddLast;
            lastSumOdd = lastSumOdd * 3;
            digit24 = lastSumOdd + lastSumEven;
            digit24 = 10 - (digit24 % 10);
            if (digit24 == 10)
                return 0;
            return digit24;
        }
        public long reverse(long barcode)
        {
            long rev = 0;
            while (barcode != 0)
            {
                rev = (rev * 10) + (barcode % 10);
                barcode /= 10;
            }
            return rev;
        }
        public long sumOdd(long barcode)
        {
            barcode = reverse(barcode);
            long sumOdd = 0, c = 1;
            while (barcode != 0)
            {
                if (c % 2 != 0)
                    sumOdd += barcode % 10;
                barcode /= 10;
                c++;
            }
            return sumOdd;
        }
        public long sumEven(long barcode)
        {
            barcode = reverse(barcode);
            long sumEven = 0, c = 1;
            while (barcode != 0)
            {
                if (c % 2 == 0)
                    sumEven += barcode % 10;
                barcode /= 10;
                c++;
            }
            return sumEven;
        }
    }
}

