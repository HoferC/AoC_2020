using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC_2020.Days;

namespace AoC_2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Day1 day1 = new Day1("Input/day1.txt");
            Console.WriteLine(day1);

            Day2 day2 = new Day2("Input/day2.txt");
            Console.WriteLine(day2);

            Day3 day3 = new Day3("Input/day3.txt");
            Console.WriteLine(day3);

            Day4 day4 = new Day4("Input/day4.txt");
            Console.WriteLine(day4);

            Day5 day5 = new Day5("Input/day5.txt");
            Console.WriteLine(day5);

            Day6 day6 = new Day6("Input/day6.txt");
            Console.WriteLine(day6);
        }
    }
}
