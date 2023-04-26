using Microsoft.VisualStudio.TestTools.UnitTesting;
using n2czh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n2czh.Tests
{
    [TestClass()]
    public class HelpersTests
    {
        [TestMethod()]
        [DataRow(6, 4)]
        [DataRow(3, 4)]
        [DataRow(2, 4)]
        [DataRow(9, 4)]
        [DataRow(14, 4)]
        public void CutRightTest(int Length, int TakeRight)
        {
            (int a, int b, int c) = Helpers.CutRight(Length, TakeRight);

            Console.WriteLine($"Length={Length}, TakeRight={TakeRight}");
            Console.WriteLine($"First Segment Start Index: {a}");
            Console.WriteLine($"First Segment Length: {b}");
            Console.WriteLine($"Second Segment Start Index: {b}");
            Console.WriteLine($"Second Segment Length: {c}");
        }

        [TestMethod()]
        [DataRow("14533222", 4, "1453", "3222")]
        [DataRow("687732", 4, "68", "7732")]
        [DataRow("332", 4, "", "332")]
        public void SplitNumStrTest(string input, int TakeRight, string expected1, string expected2)
        {
            int length = input.Length;
            (int a, int b, int c) = Helpers.CutRight(length, TakeRight);

            Console.WriteLine($"Length={length}, TakeRight={TakeRight}");
            Console.WriteLine($"First Segment Start Index: {a}");
            Console.WriteLine($"First Segment Length: {b}");
            Console.WriteLine($"Second Segment Start Index: {b}");
            Console.WriteLine($"Second Segment Length: {c}");

            (string actual1, string actual2) = Helpers.SplitNumStr(input, TakeRight);
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }


    }
}