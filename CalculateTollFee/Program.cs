using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TollFeeCalculator
{
    public class Program
    {
        private static void Main()
        {
            Run(Environment.CurrentDirectory + "../../../../testData.txt");
        }

        public static void Run(string inputFile)
        {
            var p = new Program();
            var indata = File.ReadAllText(inputFile);
            var dates = indata
                .Split(", ")
                .Select(s => DateTime.Parse(s));

            Console.Write("The total fee for the inputfile is " + p.TotalFeeCost(dates));
        }

        public int TotalFeeCost(IEnumerable<DateTime> dates)
        {
            var firstDate = dates.First();
            var startingTime = firstDate;
            var fees = new Stack<int>();
            fees.Push(CalculateTollFee(firstDate));

            foreach (var date in dates)
            {
                var diff = (date - startingTime).TotalMinutes;
                if (date.Day != firstDate.Day) throw new FormatException("Dates span multiple days");
                if (diff < 0) throw new FormatException("Dates are unsorted");
                if (diff < 60)
                {
                    var fee1 = fees.Pop();
                    var fee2 = CalculateTollFee(date);
                    fees.Push(Math.Max(fee1, fee2));
                }
                else
                {
                    startingTime = date;
                    fees.Push(CalculateTollFee(date));
                }
            }

            var totalFee = fees.Sum();

            return Math.Min(totalFee, 60);
        }

        private int CalculateTollFee(DateTime date)
        {
            if (IsDateFree(date)) return 0;
            var timeFees = new[]
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

            for (int i = 0; i < timeFees.Length; i++)
            {
                (var hour, var minute) = timeFees[i].Item1;
                var compare = new DateTime(date.Year, date.Month, date.Day, hour, minute, 00);

                var difference = (compare - date).TotalMinutes;
                if (difference > 0) return timeFees[i - 1].Item2;
            }
            return 0;
        }

        private bool IsDateFree(DateTime date)
        {
            return
                date.DayOfWeek == DayOfWeek.Sunday ||
                date.DayOfWeek == DayOfWeek.Saturday ||
                date.Month == 7;
        }
    }
}