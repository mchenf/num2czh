using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace n2czh
{
    internal static class GlobalVars
    {
        //零壹贰叄肆伍陆柒捌玖
        internal readonly static char[] NumChars = new char[]
        {
            (char)38646, (char)22777, (char)36144,
            (char)21444, (char)32902, (char)20237,
            (char)38470, (char)26578, (char)25420,
            (char)29590
        };
        //空
        //拾佰仟
        //万亿兆
        internal readonly static char[] UnitChars = new char[]
        {
            '\0',
            (char)25342, (char)20336, (char)20191,
            (char)19975, (char)20159, (char)20806
        };
        //圆角分整
        internal readonly static char[] CurrencyChars = new char[]
        {
            '\u5706', '\u89d2', '\u5206', '\u6574'
        };
    }
    internal class Program
    {
        

        readonly static string rxNumber = @"^\d+(\.\d{1,2}){0,1}$";

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine(
                    string.Format(
                        "**ERROR** n2czh expects 1 argument, but {0} supplied",
                        args.Length
                    )
                );
                return;
            };
            var rx = new Regex(rxNumber);
            if (!rx.IsMatch(args[0]))
            {
                Console.WriteLine(
                    string.Format(
                        @"**ERROR** n2czh only recognize pattern match /{0}/",
                        rxNumber
                    )
                );
                Console.WriteLine(
                    string.Format(
                        "**ERROR** Check the argument:\r\n**ERROR** => {0}",
                        args[0]
                    )
                );
                return;
            }

            // 参数拆分为小数点之前与之后
            string[] NumParts = args[0].Split('.');
            bool needZheng = true;

            var resultSB = new StringBuilder();



            // 处理小数点之前的部分
            int numLen = NumParts[0].Length;
            int startIndex = numLen - 4;
            startIndex = startIndex < 0 ? 0 : startIndex;
            int takeLen = numLen < 4 ? numLen : 4;


            string segment1 = NumParts[0].Substring(startIndex, takeLen);

            numLen -= takeLen;
            resultSB.Append(Helpers.ToCapZh0(segment1.ToCharArray()));
            if (numLen > 0)
            {
                startIndex = numLen - 4;
                startIndex = startIndex < 0 ? 0 : startIndex;
                takeLen = numLen < 4 ? numLen : 4;

                //位级变换之时，是否需要以零连接
                if (segment1[0] == '0')
                {
                    resultSB.Insert(0, GlobalVars.NumChars[0]);
                }

                segment1 = NumParts[0].Substring(startIndex, takeLen);

                if(!segment1.All(num => num == '0')) //如果万级全为零时，直接跳过输出
                {
                    resultSB.Insert(0, Helpers.ToCapZh1(segment1.ToCharArray()));
                }
                numLen -= takeLen;
            }
            if (numLen > 0)
            {
                startIndex = numLen - 4;
                startIndex = startIndex < 0 ? 0 : startIndex;
                takeLen = numLen < 4 ? numLen : 4;

                //位级变换之时，是否需要以零连接
                if (segment1[0] == '0')
                {
                    resultSB.Insert(0, GlobalVars.NumChars[0]);
                }

                segment1 = NumParts[0].Substring(startIndex, takeLen);

                if (!segment1.All(num => num == '0')) //如果亿级全为零时，直接跳过输出
                {
                    resultSB.Insert(0, Helpers.ToCapZh1(segment1.ToCharArray()));
                }
            }


            // 处理小数点之后的
            if (NumParts.Length == 2)
            {
                // len 不会超过 2， 因为 rxNumber
                int len = NumParts[1].Length;
                for (int i = 0; i < len; i++)
                {
                    string digit = NumParts[1].Substring(i, 1);
                    if (digit == "0") continue; //小数点后面的不需要输出零
                    int numIndex = NumParts[1][i].CtoInt();
                    resultSB.Append(GlobalVars.NumChars[numIndex])
                            .Append(GlobalVars.CurrencyChars[1 + i]);
                    if (needZheng) needZheng = false;
                }
            }


            if (needZheng)
            {
                resultSB.Append(GlobalVars.CurrencyChars[3]);
            }
            Console.WriteLine(resultSB.ToString());
        }
    }

    internal static class Helpers
    {
        /// <summary>
        /// 从一个长度为 <paramref name="Length"/> 的字符串中，试图从右边拿走 <paramref name="TakeRight"/> 个字
        /// </summary>
        /// <param name="Length">字符串的总长</param>
        /// <param name="TakeRight">从右边拿走多少个字</param>
        /// <returns>第一段的开始位置、长度、第二段的开始位置、长度</returns>
        internal static (int, int, int, int) CutRight(int Length, int TakeRight)
        {
            int s1, l1, s2, l2;

            s1 = l1 = s2 = l2 = 0;

            if (Length <= TakeRight)
            {

                //总长度不够取
                s2 = 0;
                l2 = Length;
            }
            else
            {
                //总长度够取
                l1 = Length - TakeRight;
                s2 = l1 + 1;
                l2 = TakeRight;
            }



            return (s1, l1, s2, l2);
        }
        internal static string ToCapZh2(
            char[] target)
        {
            var sb = new StringBuilder();
            sb.Append(ToCapZh0(target));

            sb.Append(GlobalVars.UnitChars[5]);
            sb.Append('y');
            return sb.ToString();
        }
        //生成一个万级数字，长度不超过8
        internal static string ToCapZh1(
            char[] target)
        {
            var sb = new StringBuilder();
            sb.Append(ToCapZh0(target));

            sb.Append(GlobalVars.UnitChars[4]);
            return sb.ToString();
        }
        //生成一个個级的大写数字, 长度不超过4
        internal static string ToCapZh0(
            char[] target)
        {
            var sb = new StringBuilder();
            int len = target.Length;
            if (len == 0 || len > 4) return string.Empty;

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
                        sb.Insert(0, GlobalVars.NumChars[0]);
                    }

                    char num = GlobalVars.NumChars[target[i].CtoInt()];
                    char unit = GlobalVars.UnitChars[len - i - 1];
                    sb.Insert(0, unit);
                    sb.Insert(0, num);

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
            return sb.ToString();
        }

        internal static int CtoInt(this char c) => c - '0';
    }
    [Flags]
    internal enum WriteZeroStates
    {
        None            = 0b_0000_0000,
        BeenNonZero     = 0b_0000_0001, //之前曾经为非零
        PreIsZero       = 0b_0000_0010, //前位为零
        CurIsNonZero    = 0b_0000_0100, //当前为非零
        Ready           = BeenNonZero | PreIsZero | CurIsNonZero
    }
}