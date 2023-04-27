using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n2czh.core
{
    internal static class GlobalVariables
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
}
