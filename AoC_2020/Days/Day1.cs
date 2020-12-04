using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AoC_2020.Days
{
    class Day1 : AocDay
    {
        private IEnumerable<int> _numbers;

        public Day1(string inputFileName)
        {
            DayId = 1;
            var strings = File.ReadAllLines(inputFileName);
            _numbers = strings.Select(s => int.Parse(s));
        }

        public override string Part1()
        {
            foreach (var i in _numbers)
            {
                foreach (var j in _numbers)
                {
                    if (i + j == 2020)
                    {
                        return (i * j).ToString();
                    }
                }
            }
            return 0.ToString();
        }

        public override string Part2()
        {
            foreach (var i in _numbers)
            {
                foreach (var j in _numbers)
                {
                    foreach (var k in _numbers)
                    {
                        if (i + j + k == 2020)
                        {
                            return (i * j * k).ToString();
                        }
                    }
                }
            }
            return 0.ToString();
        }
    }
}
