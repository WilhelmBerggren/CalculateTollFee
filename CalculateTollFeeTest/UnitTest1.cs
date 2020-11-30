using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TollFeeCalculator;

namespace CalculateTollFeeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TotalFeeCostTest()
        {

        }

        [TestMethod]
        public void TollFeePassTest()
        {
            var date1 = new DateTime(2020, 11, 29, 6, 15, 00);
            var date2 = new DateTime(2020, 11, 29, 6, 30, 00);
            var date3 = new DateTime(2020, 11, 29, 7, 00, 00);
            var date4 = new DateTime(2020, 11, 29, 8, 00, 00);
            var date5 = new DateTime(2020, 11, 29, 8, 30, 00);
            var date6 = new DateTime(2020, 11, 29, 15, 00, 00);
            var date7 = new DateTime(2020, 11, 29, 15, 30, 00);
            var date8 = new DateTime(2020, 11, 29, 17, 30, 00);
            var date9 = new DateTime(2020, 11, 29, 18, 00, 00);
            var date10 = new DateTime(2020, 11, 29, 18, 30, 00);

            Assert.AreEqual(8, Program.TollFeePass(date1));
            Assert.AreEqual(13, Program.TollFeePass(date2));
            Assert.AreEqual(18, Program.TollFeePass(date3));
            Assert.AreEqual(13, Program.TollFeePass(date4));
            Assert.AreEqual(8, Program.TollFeePass(date5));
            Assert.AreEqual(13, Program.TollFeePass(date6));
            Assert.AreEqual(18, Program.TollFeePass(date7));
            Assert.AreEqual(13, Program.TollFeePass(date8));
            Assert.AreEqual(8, Program.TollFeePass(date9));
            Assert.AreEqual(0, Program.TollFeePass(date10));

            Assert.AreNotEqual(8, Program.TollFeePass(date2));
        }

        [TestMethod]
        public void FreeTest()
        {
            var monday = new DateTime(2020, 11, 23);
            var tuesday = new DateTime(2020, 11, 24);
            var wednesday = new DateTime(2020, 11, 25);
            var thursday = new DateTime(2020, 11, 26);
            var friday = new DateTime(2020, 11, 27);
            var saturday = new DateTime(2020, 11, 28);
            var sunday = new DateTime(2020, 11, 29);

            var july = new DateTime(2020, 7, 01);
            var april = new DateTime(2020, 4, 01);

            Assert.IsFalse(Program.free(monday));
            Assert.IsFalse(Program.free(tuesday));
            Assert.IsFalse(Program.free(wednesday));
            Assert.IsFalse(Program.free(thursday));
            Assert.IsFalse(Program.free(friday));
            Assert.IsTrue(Program.free(saturday));
            Assert.IsTrue(Program.free(sunday));

            Assert.IsTrue(Program.free(july));
            Assert.IsFalse(Program.free(april));
        }
    }
}
