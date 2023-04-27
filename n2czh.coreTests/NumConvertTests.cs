using Microsoft.VisualStudio.TestTools.UnitTesting;
using n2czh.core;
using System;
using System.Collections.Generic;
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
        [DataRow("332774", new int[] { 0, 0, 3, 3, 2, 7, 7, 4 }, DisplayName = "Short Digit Only Number")]
        [DataRow("2217.73", new int[] { 7, 3, 2, 2, 1, 7 }, DisplayName = "Short Digit Number, Decimal")]
        [DataRow("223359988000332", new int[] { 0, 0, 2, 2, 3, 3, 5, 9, 9, 8, 8, 0, 0, 0, 3, 3, 2 }, DisplayName = "Long Digit Only Number")]
        [DataRow("223359988000332.89", new int[] { 8, 9, 2, 2, 3, 3, 5, 9, 9, 8, 8, 0, 0, 0, 3, 3, 2 }, DisplayName = "Long Digit Number, Decimal")]
        [TestMethod()]
        public void NumConvertTest(string NumberString, int[] expectedArgs)
        {
            var actual = new NumConvert(NumberString);
            ExpectedNumConvert expected = new ExpectedNumConvert(expectedArgs);
            Assert.IsTrue(expected == actual);

        }
    }

    public struct ExpectedNumConvert
    {
        public int Length { get; private set; }
        public int[] Numbers { get; set; } = new int[32];
        public int[] Decimals { get; set; } = new int[2];
        public ExpectedNumConvert(int[] args)
        {
            (Decimals[0], Decimals[1]) = (args[0], args[1]);
            Length = 0;
            for (int i = 0; i < Math.Min(args.Length - 2, 32); i++)
            {
                Numbers[i] = args[i + 2];
                Length++;
            }
        }

        public static bool operator ==(ExpectedNumConvert left, NumConvert right)
        {

            if (left.Length != right.Length) return false;
            for (int i = 0; i < left.Length; i++)
            {
                if (left.Numbers[i] != right[i, true]) return false;
            }
            for (int i = 0; i < 2; i++)
            {
                if (left.Decimals[i] != right[i, false]) return false;
            }

            return true;
        }

        public static bool operator !=(ExpectedNumConvert left, NumConvert right)
        {
            return !(left == right);

            
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}