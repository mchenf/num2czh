using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace n2czh.core
{
    /// <summary>
    /// 代表一个处理从阿拉伯数字到中文大写金额的任务
    /// </summary>
    public class NumConvert
    {
        private static readonly string validNumberString = @"^[1-9]\d+(\.\d{1,2}){0,1}$";
        private int[] numbers = new int[32];
        /// <summary>
        /// 当前整数部分的长度，最大为兆级（32）位
        /// </summary>
        public int Length { get; private set; } = 0;
        /// <summary>
        /// 当前小数部分的长度，最大为两位
        /// </summary>
        public int LengthDecimal { get; private set; } = 0;
        private int[] decimals = new int[2];
        /// <summary>
        /// 代表一个处理从阿拉伯数字到中文大写金额的任务
        /// </summary>
        /// <param name="input">输入的代表数字的字符串</param>
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
                    LengthDecimal++;
                }
            }
        }
        /// <summary>
        /// 获得数字字符串的某一位
        /// </summary>
        /// <param name="index">位数数字的序数</param>
        /// <param name="NumOn">当为 true 时，返回整数部分； false 时返回小数部分</param>
        /// <returns>位数上所代表的数字</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        public int this[int index, bool NumOn]
        {
            get 
            {
                int limit = NumOn ? Length : LengthDecimal;
                if (index > limit) throw new IndexOutOfRangeException();

                return NumOn ? numbers[index] : decimals[index];
            }
        }
        /// <summary>
        /// 检查某字符串是否符合 #00(.00) 的标准
        /// </summary>
        /// <param name="number">待检字符串</param>
        /// <returns>true 符合标准，false 不符合</returns>
        public static bool Validate(string number)
        {
            return Regex.IsMatch(number, validNumberString);
        }

        /// <summary>
        /// 将阿拉伯数字字符串转换为中文大写金额
        /// </summary>
        /// <returns>中文大写的金额</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int remain = Length;
            while (remain > 0)
            {
                (remain, int take) = BreakString(remain);
                char[] buffer = new char[take];
                for (int i = 0; i < take; i++)
                {
                    buffer[i] = GlobalVariables.NumChars[numbers[remain + i]];
                }
                sb.Insert(0, buffer);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将代表字符串长度的正数 <paramref name="input"/> 以4为基底分解到新的位置
        /// 0123456789 =>
        /// 012345 | 6789
        /// 输入 10
        /// 返回 (6, 4)
        /// 123 =>
        /// null | 123
        /// 输入 3
        /// 返回 (0, 3)
        /// </summary>
        /// <param name="input">当前字符串的长度</param>
        /// <returns>(起始,长度)</returns>
        public static (int, int) BreakString(int input)
        {
            int len = Math.Min(input, 4);
            return (input - len, len);
        }
    }
}