using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AoC_2020.Days
{
    class Day4 : AocDay
    {
        /// <summary>
        /// Records containing all 
        /// </summary>
        List<string> _records = new List<string>();
        List<PassportInfo> _passports = new List<PassportInfo>();
        public Day4(string inputFileName)
        {
            DayId = 4;
            // Need to break based on the empty lines
            string[] lines = File.ReadAllLines(inputFileName);
            StringBuilder builder = new StringBuilder();
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    builder.AppendLine(line);
                }
                else
                {
                    _records.Add(builder.ToString());
                    builder.Clear();
                }
            }
            _records.Add(builder.ToString());
            _passports = _records.Select(r => new PassportInfo(r)).ToList();
        }
        public override string Part1()
        {
            Console.WriteLine($"Read {_passports.Count()} passport records");
            int validPassports = _passports.Count(p => p.IsValid);
            return $"There are {validPassports} valid passports.";
        }

        public override string Part2()
        {
            Console.WriteLine($"Read {_passports.Count()} passport records");
            int validPassports = _passports.Count(p => p.IsValid2);
            return $"There are {validPassports} REALLY valid passports.";
        }
    }
}
