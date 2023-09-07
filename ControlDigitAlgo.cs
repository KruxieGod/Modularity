using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static int[] SplitIntoNumbers<T>(this T number)
        {
            string strNumber = number.ToString();
            int[] result = new int[strNumber.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = strNumber[i] - '0';
            return result;
        }

        public static int SumThrough(this IEnumerable<int> array,Func<int,int,int> selector)
        {
            int sum = 0;
            int i = 0;
            foreach (var item in array)
                sum += selector(item, i++);
            return sum;
        }

        public static int ClampValue(this int value, int limitation) => (limitation - value%limitation) % limitation;
    }

    public static class ControlDigitAlgo
    {
        public static int Upc(long number)
        {
            var values = number.SplitIntoNumbers();
            return values
                .Reverse()
                .SumThrough((x, index) => index % 2 == 0 ? x * 3 : x)
                .ClampValue(10);
        }

        public static char Isbn10(long number)
        {
            var values = number.SplitIntoNumbers();
            int counter = values.Length +1;
            int result = values.Sum(x => x*counter--)
                .ClampValue(11);
            if (result == 10)
                return 'X';
            return result.ToString()[0];
        }

        public static int Luhn(long number)
        {
            var values = number.SplitIntoNumbers();
            int remains = values.Length % 2 == 0 ? 1 : 0;
            return values
                .SumThrough((x, index) => index % 2 == remains ?
                x > 4 ? x * 2 - 9 : x * 2 :
                x)
                .ClampValue(10); 
        }
    }
}
