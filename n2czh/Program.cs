using System.Text;
using System.Text.RegularExpressions;

namespace n2czh
{
    internal class Program
    {
        //零壹贰叄肆伍陆柒捌玖
        readonly static char[] lsChars = new char[]
        {
            (char)38646, (char)22777, (char)36144,
            (char)21444, (char)32902, (char)20237,
            (char)38470, (char)26578, (char)25420,
            (char)29590
        };
        //拾佰仟万亿兆
        readonly static char[] lsUnits = new char[]
        {
            (char)25342, (char)20336, (char)20191,
            (char)19975, (char)20159, (char)20806
        };
        //圆角分整
        readonly static char[] lsCurrency = new char[]
        {
            '\u5706', '\u89d2', '\u5206', '\u6574'
        };

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
                        @"**ERROR** n2czh only recognize pattern match/{0}/",
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

            // 处理小数点之后的
            if (NumParts.Length == 2)
            {
                // len 不会超过 2， 因为 rxNumber
                int len = NumParts[1].Length;
                for (int i = 0; i < len; i++)
                {
                    string digit = NumParts[1].Substring(i, 1);
                    if (digit == "0") continue; //小数点后面的不需要输出零
                    int numIndex = int.Parse(NumParts[1].Substring(i, 1));
                    resultSB.Append(lsChars[numIndex])
                            .Append(lsCurrency[1 + i]);
                    if (needZheng) needZheng = false;
                }
            }

            // 处理小数点之前的部分



            if (needZheng)
            {
                resultSB.Append(lsCurrency[3]);
            }
            Console.WriteLine(resultSB.ToString());


        }

        internal static void ConvertN2czh(int unitDigit, int unitLength, StringBuilder strBuilder)
        {

        }
    }
}