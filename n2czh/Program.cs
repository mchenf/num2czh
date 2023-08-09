using System.Text;
using n2czh.core;
using System.Text.RegularExpressions;

namespace n2czh
{
    internal class Program
    {
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


            var numConvert = new NumConvert(args[0]);

            Console.WriteLine(numConvert.ToString());

            Console.WriteLine();
        }
    }
}