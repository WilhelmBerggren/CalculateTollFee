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
         * 10. On new day program should either throw exception or split toll fee on two days
         */
        [TestMethod]
        public void UnsortedInputTest()
        {
            //Program should throw exception if input array is unsorted
            //Bug: Unsorted array gives wrong fee

            var p = new Program();

            DateTime[] data = {
                new DateTime(2020, 11, 25, 8, 15, 00),
                new DateTime(2020, 11, 25, 6, 15, 00),
                new DateTime(2020, 11, 25, 16, 15, 00),
                new DateTime(2020, 11, 24, 14, 15, 00)
            };

            Assert.ThrowsException<FormatException>(() => p.TotalFeeCost(data));
        }

        [TestMethod]
        public void FirstFeeWithinHourLowerTest()
        {
            // If two dates within one hour, only the higher fee should be added
            // Bug: Both fees are added
            
            var p = new Program();

            var date1 = new DateTime(2020, 11, 25, 6, 15, 00);
            var date2 = new DateTime(2020, 11, 25, 6, 35, 00);

            Assert.AreEqual(13, p.TotalFeeCost(new[] { date1, date2 }));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnMultipleDatesTest()
        {
            //If the data span over multiple days an exception should be thrown
            //Bug: it don't, man.
            
            var p = new Program();

            var date1 = new DateTime(2020, 11, 25, 8, 01, 00);
            var date2 = new DateTime(2020, 11, 26, 8, 31, 00);

            Assert.ThrowsException<FormatException>(() => p.TotalFeeCost(new[] { date1, date2 }));
        }

        [TestMethod]
        public void TwoDatesWithinHourWhereSecondFeeIsLowerTest()
        {
            // If two dates within one hour where the first has a higher fee, only the higher fee should be added
            // Bug: The higher fee is added twice

            var p = new Program();

            var date1 = new DateTime(2020, 11, 25, 8, 01, 00);
            var date2 = new DateTime(2020, 11, 25, 8, 31, 00);

            Assert.AreEqual(13, p.TotalFeeCost(new[] { date1, date2 }));
        }

        [TestMethod]
        public void NoFeeAfterSixThirtyTest()
        {
            // No fee should occur after 18.30

            var p = new Program();

            var date1 = new DateTime(2020, 11, 23, 19, 30, 00);

            for (int i = 0; i < 7; i++)
            {
                Assert.AreEqual(0, p.TotalFeeCost(new[] { date1 }));
                date1 = date1.AddDays(1);
            }
        }

        [TestMethod]
        public void FeeOnWeekdaysTest()
        {
            //Fees should be applied on weekdays between 6:00 and 18:29
            //Bug: No fee is added on fridays
            
            var p = new Program();

            var monday = new DateTime(2020, 11, 23, 9, 35, 00);
            var tuesday = new DateTime(2020, 11, 24, 9, 00, 00);
            var wednesday = new DateTime(2020, 11, 25, 9, 00, 00);
            var thursday = new DateTime(2020, 11, 26, 9, 00, 00);
            var friday = new DateTime(2020, 11, 27, 9, 00, 00);

            var actual1 = p.TotalFeeCost(new[] { monday });
            var actual2 = p.TotalFeeCost(new[] { tuesday });
            var actual3 = p.TotalFeeCost(new[] { wednesday });
            var actual4 = p.TotalFeeCost(new[] { thursday });
            var actual5 = p.TotalFeeCost(new[] { friday });

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
            
            var p = new Program();

            var weekendDate = new DateTime(2020, 11, 28, 0, 00, 00);

            for (int i = 0; i < 96; i++)
            {
                Assert.AreEqual(0, p.TotalFeeCost(new[] { weekendDate }));
                weekendDate = weekendDate.AddMinutes(30);
            }

        }

        [TestMethod]
        public void FeeIsLessThanSixtyTest()
        {
            // Fee should be able to be less than 60
            // Bug: fee is never below 60
            
            var p = new Program();

            var date1 = new DateTime(2020, 11, 25, 6, 15, 00);

            Assert.AreEqual(8, p.TotalFeeCost(new[] { date1 }));
        }

        [TestMethod]
        public void FeeIsAtMostSixtyTest()
        {
            // Fee should never be more than 60
            // Bug: Fee can be more than 60
            
            var p = new Program();

            var date1 = new DateTime(2020, 11, 25, 7, 30, 00);
            var date2 = new DateTime(2020, 11, 25, 15, 1, 00);
            var date3 = new DateTime(2020, 11, 25, 16, 5, 00);
            var date4 = new DateTime(2020, 11, 25, 17, 10, 00);

            Assert.AreEqual(60, p.TotalFeeCost(new[] { date1, date2, date3, date4 }));
        }

        [TestMethod]
        public void NoFeeBetweenFirstHalfHourBetweenEightAndTwoThirtyTest()
        {
            // Bug: In the first half hour of each hour between 8 and 14.30, 
            
            var p = new Program();

            var date1 = new DateTime(2020, 11, 25, 9, 00, 00);
            var date2 = new DateTime(2020, 11, 25, 11, 15, 00);
            var date3 = new DateTime(2020, 11, 25, 13, 25, 00);

            Assert.AreNotEqual(0, p.TotalFeeCost(new[] { date1 }));
            Assert.AreNotEqual(0, p.TotalFeeCost(new[] { date2 }));
            Assert.AreNotEqual(0, p.TotalFeeCost(new[] { date3 }));
        }
    }
}
