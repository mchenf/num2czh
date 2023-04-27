using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace n2czh.core
{
    
    public class NumConvert
    {
        private static readonly string validNumberString = @"^[1-9]\d+(\.\d{1,2}){0,1}$";
        private int[] numbers = new int[32];
        public int Length { get; private set; } = 0;
        public bool HasDecimal() => decimals[0] > 0 && decimals[1] > 0;
        private int[] decimals = new int[2];

        public NumConvert(string input)
        {
            if (Validate(input))
            {
                int len = input.Length;
                int i = 0;
                for (; i < len && input[i] != '.'; i++) {
                    numbers[i] = input[i] - '0';
                }
                Length = i;
                //最大剩余两位
                i++;
                for (int j = 0; i < len; i++, j++) {
                    decimals[j] = input[i] - '0';
                }
            }
        }

        public int this[int index, bool NumOn]
        {
            get 
            {
                int limit = NumOn ? Length : 1;
                if (index > limit) throw new IndexOutOfRangeException();

                return NumOn ? numbers[index] : decimals[index];
            }
        }

        public static bool Validate(string number)
        {
            return Regex.IsMatch(number, validNumberString);
        }
    }
}