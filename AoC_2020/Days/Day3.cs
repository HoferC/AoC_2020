using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AoC_2020.Days
{
    class Day3 : AocDay
    {
        TobogganRun _toboggan;

        public Day3(string inputFileName)
        {
            DayId = 3;
            var strings = File.ReadAllLines(inputFileName);
            _toboggan = new TobogganRun(strings);   
        }

        public override string Part1()
        {
            return _toboggan.CalculateRun(new Coordinate(3, 1), out _);
        }

        public override string Part2()
        {
            List<Coordinate> runs = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(3, 1),
                new Coordinate(5, 1),
                new Coordinate(7, 1),
                new Coordinate(1, 2)
            };
            return _toboggan.CalculateRunProducts(runs);
        }

        
    }
}
