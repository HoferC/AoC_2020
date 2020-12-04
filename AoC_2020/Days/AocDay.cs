using System;
using System.Collections.Generic;
using System.Text;

namespace AoC_2020
{
    /// <summary>
    /// A runner for a day of Advent of Code 2020
    /// </summary>
    public abstract class AocDay
    {
        public int DayId { get; protected set; }

        public abstract string Part1();

        public abstract string Part2();

        public override string ToString()
        {
            return $"Day {DayId} Part 1: {Part1()}{Environment.NewLine}Day {DayId} Part 2: {Part2()}";
        }
    }
}
