using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AoC_2020.Days
{
    class Day6 : AocDay
    {
        List<string> _records = new List<string>();
        List<CustomsGroup> _groups = new List<CustomsGroup>();

        public Day6 (string inputFileName)
        {
            DayId = 6;
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
            _groups = _records.Select(r => new CustomsGroup(r)).ToList();
        }

        public override string Part1()
        {
            // Expanded
            int sumVotes = 0;
            foreach (var r in _records.Select(r => r.Replace("\r\n", "")))
            {
                var unique = r.Distinct();
                var length = unique.Count();
                sumVotes += length;
            }

            // One-liner
            int sumVotes2 = _records.Select(r => r.Replace("\r\n", "").Distinct().Count()).Sum();
            return $"The sum of unique answers is {sumVotes2}";
        }

        public override string Part2()
        {
            int result = _groups.Sum(g => g.UnanimousVotes);
            return $"There are {result} groups with unanimous votes";
        }

        class CustomsGroup
        {
            int NumGuests { get; set; }

            string Value { get; set; }

            Dictionary<char, int> Votes { get; } = new Dictionary<char, int>();

            public int UnanimousVotes
            {
                get
                {
                    return Votes.Where(kvp => kvp.Value == NumGuests).Count();
                }
            }

            public CustomsGroup(string definition)
            {
                Value = definition;
                NumGuests = definition.Count(c => c == '\n');
                foreach (char c in definition)
                {
                    if (c == '\r' || c == '\n')
                    { 
                        continue;
                    }    
                    if (!Votes.ContainsKey(c))
                    {
                        Votes.Add(c, 0);
                    }
                    Votes[c]++;
                }
            }
        }
    }
}
