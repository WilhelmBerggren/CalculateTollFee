using System;
using System.IO;

namespace TollFeeCalculator
{
    public class Program
    {
        static void Main()
        {
            run(Environment.CurrentDirectory + "../../../../testData.txt");
        }

        public static void run(String inputFile)
        {
            string indata = File.ReadAllText(inputFile);
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
            DateTime startingInterval = dates[0];
            foreach (var d2 in dates)
            {
                double diffInMinutes = (d2 - startingInterval).TotalMinutes;
                if (diffInMinutes < 0) throw new FormatException("Dates is unsorted");
                if (diffInMinutes > 60)
                {
                    fee += TollFeePass(d2);
                    startingInterval = d2;
                }
                else
                {
                    fee += Math.Max(TollFeePass(d2), TollFeePass(startingInterval));
                }
            }
            return Math.Min(fee, 60);
        }

        static int TollFeePass(DateTime date)
        {
            if (IsDateFree(date)) return 0;
            var fees = new[]
            {
                ((0, 00), 0),
                ((6, 00), 8),
                ((6, 30), 13),
                ((7, 00), 18),
                ((8, 00), 13),
                ((8, 30), 8),
                ((15, 00), 13),
                ((15, 30), 18),
                ((17, 00), 13),
                ((18, 00), 8),
                ((18, 30), 0)
            };

            for (int i = 0; i < fees.Length; i++)
            {
                (var hour, var minute) = fees[i].Item1;

                if (date.Hour < hour && date.Minute < minute) return fees[i - 1].Item2;
            }
            return 0;
        }

        static bool IsDateFree(DateTime day)
        {
            return (
                day.DayOfWeek == DayOfWeek.Sunday || 
                day.DayOfWeek == DayOfWeek.Saturday || 
                day.Month == 7
            );
        }
    }
}