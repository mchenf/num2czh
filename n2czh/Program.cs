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

        const string rxNumber = @"^\d+(\.\d{1,2}){0,1}$";

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
            resultSB.Insert(0, Helpers.ToCapZh3(NumParts[0]));

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

public static class Helpers
{

    public static (string, string) SplitNumStr(this string input, int TakeRight)
    {
        int s1, l1, l2;
        string out1, out2;
        (s1, l1, l2) = CutRight(input.Length, TakeRight);
        out1 = out2 = "";
        if (l1 > 0)
        {
            out1 = input.Substring(s1, l1);
        }

        out2 = input.Substring(l1, l2);

        return (out1, out2);
    }
        /// <summary>
        /// 从一个长度为 <paramref name="Length"/> 的字符串中，试图从右边拿走 <paramref name="TakeRight"/> 个字
        /// </summary>
        /// <param name="Length">字符串的总长</param>
        /// <param name="TakeRight">从右边拿走多少个字</param>
        /// <returns>第一段的开始位置、长度、第二段的开始位置、长度</returns>
        public static (int, int, int) CutRight(int Length, int TakeRight)
        {
            int s1, l1, l2;

            s1 = l1 = l2 = 0;

            if (Length <= TakeRight)
            {

                //总长度不够取
                l2 = Length;
            }
            else
            {
                //总长度够取
                l1 = Length - TakeRight;
                l2 = TakeRight;
            }
            return (s1, l1, l2);
        }


        //生成一个兆级数字，长度不超过32
        internal static string ToCapZh3(
            string target)
        {

            Console.WriteLine($"[INFO]: ToCapZh3({target})");

            if (target.Length == 0 || target.Length > 32) return string.Empty;
            (string o1, string o2) = target.SplitNumStr(16);
            var sb = new StringBuilder();
            //生成兆级部分
            if (o1 != string.Empty)
            {
                sb.Append(ToCapZh2(o1));
                sb.Append(GlobalVars.UnitChars[6]);
            }
            //生成万级部分
            sb.Append(ToCapZh2(o2));
            return sb.ToString();
        }


        //生成一个亿级数字，长度不超过16
        internal static string ToCapZh2(
            string target)
        {

            Console.WriteLine($"[INFO]: ToCapZh2({target})");

            if (target.Length == 0 || target.Length > 16) return string.Empty;
            (string o1, string o2) = target.SplitNumStr(8);
            var sb = new StringBuilder();
            //生成亿级部分
            if (o1 != string.Empty)
            {
                sb.Append(ToCapZh1(o1));
                sb.Append(GlobalVars.UnitChars[5]);
            }
            //生成万级部分
            sb.Append(ToCapZh1(o2));
            return sb.ToString();
        }
        //生成一个万级数字，长度不超过8
        internal static string ToCapZh1(
            string target)
        {

            Console.WriteLine($"[INFO]: ToCapZh1({target})");
            if (target.Length == 0 || target.Length > 8) return string.Empty;
            (string o1, string o2) = target.SplitNumStr(4);


            var sb = new StringBuilder();

            //判断 o1 是否为空或者全零
            bool NotEmptyOrZero = o1 != string.Empty && o1 != new string('0', o1.Length);
            //生成万级部分
            if (NotEmptyOrZero)
            {
                sb.Append(ToCapZh0(o1));
                sb.Append(GlobalVars.UnitChars[4]);
            }

            //生成一个個级

            if (o2.Length > 0 && o2[0] == '0')
            {
                //如果o2 以0开头，向 sb 输入一个零
                sb.Append(GlobalVars.NumChars[0]);
            }

            string o2C = ToCapZh0(o2);
            sb.Append(o2C);


            return sb.ToString();
        }
        //生成一个個级的大写数字, 长度不超过4
        internal static string ToCapZh0(
            string target)
        {
            Console.WriteLine($"[INFO]: ToCapZh0({target})");
            int len = target.Length;
            if (len == 0 || len > 4) return string.Empty;

            var sb = new StringBuilder();
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