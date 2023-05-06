using Microsoft.VisualStudio.TestTools.UnitTesting;
using n2czh.core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace n2czh.core.Tests
{
    [TestClass()]
    public class NumConvertTests
    {
        [TestMethod()]
        [DataRow("00332211", false, DisplayName = "Leading Zeros")]
        [DataRow("00332211.32", false, DisplayName = "Leading Zeros, w/ 2 dec place")]
        [DataRow("3221.77", true, DisplayName = "Short digits with 2 decimal places")]
        [DataRow("332211778822211.445", false, DisplayName = "Long digits, with 3 decimal places")]
        [DataRow("1212121212121212", true, DisplayName = "Long digits, no decimal places")]
        [DataRow("332dda12322", false, DisplayName = "Contains alphabet")]
        [DataRow("223359988000332", true, DisplayName = "Check Parms used 1")]
        [DataRow("223359988000332.89", true, DisplayName = "Check Parms used 2")]
        public void ValidateTest(string var1, bool expected)
        {
            bool actual = NumConvert.Validate(var1);
            Assert.AreEqual(expected, actual);

        }
        [TestInitialize()]
        public void Setup()
        {

        }

        [TestMethod()]
        [DataRow("332", "", "332")]
        [DataRow("3322225567", "332222", "5567")]
        [DataRow("3311122", "331", "1122")]
        [DataRow("3311122", "331", "1122")]

        public void BreakStringTest(string number, string exp1, string exp2)
        {
            int len = number.Length;

            (int i, int l) = len.BreakString();
            Console.WriteLine("len={0}", len);
            Console.WriteLine("i={0}", i);
            Console.WriteLine("l={0}", l);
            string actual1 = i == 0 ? "" : number.Substring(0, i);
            string actual2 = number.Substring(i, l);
            Console.WriteLine("actual1={0}\r\nactual2={1}", actual1, actual2);
            Assert.AreEqual(exp1, actual1);
            Assert.AreEqual(exp2, actual2);


        }

        [TestMethod()]
        public void NumConvertTest1()
        {
            var n = new NumConvert("332502");

            Debug.Print(n.ToString());

            Assert.IsTrue(true);
        }

        [TestMethod()]
        [DataRow("4220", "肆仟贰佰贰拾")]
        [DataRow("881", "捌佰捌拾壹")]
        [DataRow("74", "柒拾肆")]
        [DataRow("9", "玖")]

        [DataRow("766480", "柒拾陆万陆仟肆佰捌拾")]
        [DataRow("42206480", "肆仟贰佰贰拾万陆仟肆佰捌拾")]

        [DataRow("422042206480", "肆仟贰佰贰拾亿肆仟贰佰贰拾万陆仟肆佰捌拾")]
        [DataRow("6422042206480", "陆万肆仟贰佰贰拾亿肆仟贰佰贰拾万陆仟肆佰捌拾")]

        [DataRow("7664876422042206480", "柒佰陆拾陆兆肆仟捌佰柒拾陆万肆仟贰佰贰拾亿肆仟贰佰贰拾万陆仟肆佰捌拾")]
        [DataRow("977664876422042206480", "玖万柒仟柒佰陆拾陆兆肆仟捌佰柒拾陆万肆仟贰佰贰拾亿肆仟贰佰贰拾万陆仟肆佰捌拾")]
        [DataRow("776977664876422042206480", "柒仟柒佰陆拾玖万柒仟柒佰陆拾陆兆肆仟捌佰柒拾陆万肆仟贰佰贰拾亿肆仟贰佰贰拾万陆仟肆佰捌拾")]
        [DataRow("270776977664876422042206482", "贰佰柒拾亿柒仟柒佰陆拾玖万柒仟柒佰陆拾陆兆肆仟捌佰柒拾陆万肆仟贰佰贰拾亿肆仟贰佰贰拾万陆仟肆佰捌拾贰")]

        [DataRow("60000000", "陆仟万")]
        [DataRow("70000060000000", "柒拾万亿零陆仟万")]



        public void ToStringTest(string testee, string expected)
        {
            var n = new NumConvert(testee);
            string actual = n.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}