using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC_2020.Days
{
    class Day7 : AocDay
    {
        public Day7(string inputFileName)
        {
            var lines = System.IO.File.ReadAllLines(inputFileName);
            var bags = lines.Select(l => new BagRule(l)).ToList();
        }

        public override string Part1()
        {
            return $"Not implemented yet";
        }

        public override string Part2()
        {
            return $"Not implemented yet";
        }
    }

    class BagRule
    {
        public string BagColor { get; private set; }
        public List<BagRule> ContainedBags { get; } = new List<BagRule>();


        Regex _definitionRegex = new Regex(@"^(\w+ \w+) bags? contain (\d+|(?:no)) ([\w\s]+) bags?(, (\d+) (\w+ \w+) bags?)*\.$");
        public BagRule(string definition)
        {
            var match = _definitionRegex.Match(definition);
            foreach (var g in match.Groups)
            {
                Console.WriteLine(g.ToString());
            }
        }

        public BagRule(string bagType, string definition)
        {
            
        }
    }
}
