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



        }
    }
}