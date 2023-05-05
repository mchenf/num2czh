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
        [DataRow("1234", "壹仟贰佰叄拾肆")]
        [DataRow("6532", "陆仟伍佰叄拾贰")]
        [DataRow("7047", "柒仟零肆拾柒")]
        [DataRow("7005", "柒仟零伍")]
        [DataRow("2000", "贰仟")]
        [DataRow("3760", "叄仟柒佰陆拾")]
        [DataRow("1200", "壹仟贰佰")]
        public void ProcessKClassTest(string input, string expected)
        {
            string actual = new string(input.ToCharArray().ProcessKClass());
            Assert.AreEqual(expected, actual);
        }
    }
}