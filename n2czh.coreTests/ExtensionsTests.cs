using Microsoft.VisualStudio.TestTools.UnitTesting;
using n2czh.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n2czh.core.Tests
{
    [TestClass()]
    public class ExtensionsTests
    {
        [TestMethod()]
        [DataRow("6532", "陆仟伍佰叄拾贰", DisplayName = "全四位")]
        [DataRow("7047", "柒仟零肆拾柒", DisplayName = "全四位，百位为零")]
        [DataRow("7005", "柒仟零伍", DisplayName = "全四位, 百位十位为零")]
        [DataRow("2000", "贰仟", DisplayName = "全四位, 百位十位个位为零")]
        [DataRow("3760", "叄仟柒佰陆拾", DisplayName = "全四位, 个位为零")]
        [DataRow("1200", "壹仟贰佰", DisplayName = "全四位, 十位个位为零")]

        [DataRow("567", "伍佰陆拾柒", DisplayName = "仅三位")]
        [DataRow("809", "捌佰零玖", DisplayName = "仅三位，含零")]
        [DataRow("400", "肆佰", DisplayName = "仅三位，佰")]

        [DataRow("57", "伍拾柒", DisplayName = "仅两位")]
        [DataRow("80", "捌拾", DisplayName = "仅两位，含零")]

        [DataRow("6", "陆", DisplayName = "仅一")]

        [DataRow("0", "", DisplayName = "零")]
        public void ProcessKClassTest(string input, string expected)
        {
            ReadOnlySpan<char> inp = new ReadOnlySpan<char>(input.ToCharArray());
            string actual = new string(inp.ProcessKClass());
            Assert.AreEqual(expected, actual);
        }
    }
}