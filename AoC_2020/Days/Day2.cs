using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AoC_2020.Days
{
    class Day2 : AocDay
    {
        IEnumerable<AocPassword> _passwords;

        public Day2(string inputFileName)
        {
            DayId = 2;
            var strings = File.ReadAllLines(inputFileName);
            _passwords = strings.Select(s => new AocPassword(s));
        }

        public override string Part1()
        {
            return _passwords.Count(p => p.IsValid1).ToString();
        }

        public override string Part2()
        {
            return _passwords.Count(p => p.IsValid2).ToString();
        }
    }
}
