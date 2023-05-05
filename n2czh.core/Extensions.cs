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
        //生成一个個级的大写数字, 长度不超过4
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
            char[] result = new char[bufferLen];
            for (int i = ptr; i < buffer.Length; i++)
            {
                result[i - ptr] = buffer[i];
            }
            return result;
        }
    }
}
