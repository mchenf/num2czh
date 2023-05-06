using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace n2czh.core
{
    public static class Extensions
    {

        /// <summary>
        /// 将代表字符串长度的正数 <paramref name="input"/> 以 <paramref name="cut"/> 为基底分解到新的位置
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
        /// <param name="cut">切割多少长度</param>
        /// <returns>(起始,长度)</returns>
        public static (int, int) BreakString(this int input, int cut = 4)
        {
            int len = Math.Min(input, cut);
            return (input - len, len);
        }
        /// <summary>
        /// 根据 <paramref name="take"/> 的长度，决定数量级，基于 <paramref name="target"/> 生成中文大写数字。
        /// 4位，万级；
        /// 8位，亿级；
        /// 16位，兆级；
        /// </summary>
        /// <param name="target">代表目标数字的字符数组片段，长度为 1-32</param>
        /// <param name="take">切取多少位</param>
        /// <returns>转化后的中文大写数字</returns>
        public static char[] ProcessXClass(this ReadOnlySpan<char> target)
        {
            int len = target.Length;
            if (len == 0 || len > 32)
            {
                return new char[0];
            }

#if DEBUG
            Console.WriteLine();
            Console.WriteLine("<ProcessXClass> : 正在处理：{0}", string.Join(',', target.ToArray()));


#endif

            int take = len > 16 ? 32 : len > 8 ? 16 : len > 4 ? 8 : 4;
            int unitIndex = len > 16 ? 6 : len > 8 ? 5 : len > 4 ? 4 : 0;

            if (take == 4)
            {
                return target.ProcessKClass();
            }

            char[] buffer, buffer2;
            char[] result = new char[128];
            int index = 0;
            int s = 0, t = 0;
                take /= 2;
                (s, t) = len.BreakString(take);
                var left = target.Slice(0, s);
                var right = target.Slice(s, t);

                buffer = ProcessXClass(left);
                for (int i = 0; i < buffer.Length; i++)
                {
                    result[index++] = buffer[i];
                }
                result[index++] = GlobalVariables.UnitChars[unitIndex--];
                buffer2 = ProcessXClass(right);
                for (int i = 0; i < buffer2.Length; i++)
                {
                    result[index++] = buffer2[i];
                }
#if DEBUG
                Console.WriteLine("<ProcessXClass> : 切分为左边 | 右边段: {0} || {1}", string.Join(',', left.ToArray()), string.Join(',', right.ToArray()));
                Console.WriteLine("<ProcessXClass> : 左边 | 右边处理结果: {0} || {1}", string.Join(',', buffer), string.Join(',', buffer2));


#endif
            char[] output = new char[index];
            for (int i = 0; i < index; i++)
            {
                output[i] = result[i];
            }
            return output;
        }
        /// <summary>
        /// 从一个长度不超过4位且不为零位的 <paramref name="target"/>
        /// </summary>
        /// <param name="target">代表目标数字的字符数组片段，长度为 1-4</param>
        /// <returns>转化后的中文大写数字</returns>
        public static char[] ProcessKClass(this ReadOnlySpan<char> target)
        {
            int len = target.Length;
            if (len == 0 || len > 4) return new char[0];

            char[] buffer = new char[7]; //最大可能生成的是“一千两百三十四” 一共七位
            int ptr = 6;
            var zeroState = WriteZeroStates.None;
            //开始循环处理，最大4位
            for (int i = len - 1; i >= 0; i--)
            {
                //对零进行特殊处理
                if (target[i] > '0')
                {
                    zeroState |= WriteZeroStates.CurIsNonZero;

                    if (zeroState == WriteZeroStates.Ready)
                    {
                        buffer[ptr] = GlobalVariables.NumChars[0];
                        ptr--;
                    }

                    char num = GlobalVariables.NumChars[target[i] - '0'];
                    char unit = GlobalVariables.UnitChars[len - i - 1];

                    if(unit > 0)
                    {
                        buffer[ptr] = unit;
                        ptr--;
                    }
                    buffer[ptr] = num;
                    ptr--;

                    zeroState = zeroState & ~WriteZeroStates.PreIsZero; //去掉前位非零信号
                    zeroState |= WriteZeroStates.BeenNonZero;
                }
                else
                {
                    //当前为零时
                    zeroState = zeroState & ~WriteZeroStates.CurIsNonZero; //去掉当前非零信号
                    zeroState |= WriteZeroStates.PreIsZero; //将前位为零信号传递到下一次循环
                }
            }
            ptr++; //之前多减了一次，加一后才是 ptr。
            int bufferLen = buffer.Length - ptr;
            //提取非 \0 的结果
            char[] result = new char[bufferLen];
            for (int i = ptr; i < buffer.Length; i++)
            {
                result[i - ptr] = buffer[i];
            }
            return result;
        }
    }
}
