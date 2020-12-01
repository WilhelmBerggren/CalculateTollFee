using System;
using System.Linq;

namespace TollFeeCalculator
{
    public class Program
    {
        public static void Main()
        {
            var d = new DateTime(2020, 11, 29, 6, 00, 00);
            for (var i = 0; i <= 2 * 24; i++)
            {
                Console.Write(d.ToShortTimeString());
                Console.Write("\t");
                Console.Write(TotalFeeCost(new[] { d }));
                Console.WriteLine();
                d = d.AddMinutes(30);
            }

            run(Environment.CurrentDirectory + "../../../../testData.txt");
        }

        public static void run(String inputFile)
        {
            string indata = System.IO.File.ReadAllText(inputFile);
            String[] dateStrings = indata.Split(", ");
            DateTime[] dates = new DateTime[dateStrings.Length - 1];
            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.Parse(dateStrings[i]);
            }
            Console.Write("The total fee for the inputfile is " + TotalFeeCost(dates));
        }

        public static int TotalFeeCost(DateTime[] dates)
        {
            int fee = 0;

            Array.Sort(dates);

            DateTime startDate = dates[0]; //Starting interval
            foreach (var date in dates)
            {
                double diffInMinutes = (date - startDate).TotalMinutes;
                if (diffInMinutes > 60)
                {
                    fee += TollFeePass(date);
                    startDate = date;
                }
                else
                {
                    fee += Math.Max(TollFeePass(date), TollFeePass(startDate));
                }
            }
            return Math.Min(fee, 60);
        }

        public static int TollFeePass(DateTime d)
        {
            if (free(d)) return 0;
            int hour = d.Hour;
            int minute = d.Minute;
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }
        //Gets free dates
        public static bool free(DateTime day)
        {
            if (day.Hour < 6) return true;
            if (day.Hour >= 18 && day.Minute >= 30) return true;
            return (int)day.DayOfWeek == 6 || day.DayOfWeek == 0 || day.Month == 7;
        }
    }
}