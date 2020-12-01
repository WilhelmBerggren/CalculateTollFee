using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TollFeeCalculator;

namespace CalculateTollFeeTest
{
    [TestClass]
    public class UnitTest1
    {
        /*
         * Bugs:
         * 1. Within hour applies both fees
         * 2. Applies fees after 18.30
         * 3. Free on Fridays
         * 4. Not free on Sundays
         * 5. Fee is ever less than 60
         * 6. Fee can be over 60
         * 7. Between 8.30 and 14.59, there is no fee in first 29 minutes of every hour
         * 8. When more than one hour between times, only minute difference is recognized
         * 9. Unsorted array gives wrong fee
         */

        [TestMethod]
        public void FirstFeeWithinHourLowerTest()
        {
            // If two dates within one hour, only the higher fee should be added
            // Bug: Both fees are added

            var date1 = new DateTime(2020, 11, 25, 6, 15, 00);
            var date2 = new DateTime(2020, 11, 25, 6, 35, 00);

            Assert.AreEqual(13, Program.TotalFeeCost(new[] { date1, date2 }));
        }

        [TestMethod]
        public void TwoDatesWithinHourWhereSecondFeeIsLowerTest()
        {
            // If two dates within one hour where the first has a higher fee, only the higher fee should be added
            // Bug: The higher fee is added twice

            var date1 = new DateTime(2020, 11, 25, 8, 01, 00);
            var date2 = new DateTime(2020, 11, 25, 8, 31, 00);

            Assert.AreEqual(13, Program.TotalFeeCost(new[] { date1, date2 }));
        }

        [TestMethod]
        public void NoFeeAfterSixThirtyTest()
        {
            // No fee should occur after 6.30

            var date1 = new DateTime(2020, 11, 25, 19, 30, 00);

            Assert.AreEqual(0, Program.TotalFeeCost(new[] { date1 }));
        }

        [TestMethod]
        public void FeeOnWeekdaysTest()
        {
            var monday = new DateTime(2020, 11, 23, 9, 35, 00);
            var tuesday = new DateTime(2020, 11, 24, 9, 00, 00);
            var wednesday = new DateTime(2020, 11, 25, 9, 00, 00);
            var thursday = new DateTime(2020, 11, 26, 9, 00, 00);
            var friday = new DateTime(2020, 11, 27, 9, 00, 00);

            var actual1 = Program.TotalFeeCost(new[] { monday });
            var actual2 = Program.TotalFeeCost(new[] { tuesday });
            var actual3 = Program.TotalFeeCost(new[] { wednesday });
            var actual4 = Program.TotalFeeCost(new[] { thursday });
            var actual5 = Program.TotalFeeCost(new[] { friday });

            Assert.AreNotEqual(0, actual1);
            Assert.AreNotEqual(0, actual2);
            Assert.AreNotEqual(0, actual3);
            Assert.AreNotEqual(0, actual4);
            Assert.AreNotEqual(0, actual5);
        }

        [TestMethod]
        public void NoFeeOnWeekendsTest()
        {
            // There should be no fees on weekends
            // Bug: Fees are added on Sundays
            var saturday = new DateTime(2020, 11, 28, 9, 00, 00);
            var sunday = new DateTime(2020, 11, 29, 9, 00, 00);

            Assert.AreEqual(0, Program.TotalFeeCost(new[] { saturday }));
            Assert.AreEqual(0, Program.TotalFeeCost(new[] { sunday }));
        }

        [TestMethod]
        public void FeeIsLessThanSixtyTest()
        {
            // Fee should be able to be less than 60
            // Bug: fee is never below 60

            var date1 = new DateTime(2020, 11, 25, 6, 15, 00);

            Assert.AreEqual(8, Program.TotalFeeCost(new[] { date1 }));
        }

        [TestMethod]
        public void FeeIsAtMostSixtyTest()
        {
            // Fee should never be more than 60
            // Bug: Fee can be more than 60

            var date1 = new DateTime(2020, 11, 25, 7, 30, 00);
            var date2 = new DateTime(2020, 11, 25, 15, 1, 00);
            var date3 = new DateTime(2020, 11, 25, 16, 5, 00);
            var date4 = new DateTime(2020, 11, 25, 17, 10, 00);

            Assert.AreEqual(60, Program.TotalFeeCost(new[] { date1, date2, date3, date4 }));
        }

        [TestMethod]
        public void NoFeeBetweenFirstHalfHourBetweenEightAndTwoThirtyTest()
        {
            // Bug: In the first half hour of each hour between 8 and 14.30, 

            var date1 = new DateTime(2020, 11, 25, 9, 00, 00);
            var date2 = new DateTime(2020, 11, 25, 11, 15, 00);
            var date3 = new DateTime(2020, 11, 25, 13, 25, 00);

            Assert.AreNotEqual(0, Program.TotalFeeCost(new[] { date1 }));
            Assert.AreNotEqual(0, Program.TotalFeeCost(new[] { date2 }));
            Assert.AreNotEqual(0, Program.TotalFeeCost(new[] { date3 }));
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
            var monday = new DateTime(2020, 11, 23, 9, 00, 00);
            var tuesday = new DateTime(2020, 11, 24, 9, 00, 00);
            var wednesday = new DateTime(2020, 11, 25, 9, 00, 00);
            var thursday = new DateTime(2020, 11, 26, 9, 00, 00);
            var friday = new DateTime(2020, 11, 27, 9, 00, 00);
            var saturday = new DateTime(2020, 11, 28, 9, 00, 00);
            var sunday = new DateTime(2020, 11, 29, 9, 00, 00);

            var july = new DateTime(2020, 7, 01, 9, 00, 00);
            var april = new DateTime(2020, 4, 01, 9, 00, 00);

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
