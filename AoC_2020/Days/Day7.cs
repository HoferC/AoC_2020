using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC_2020.Days
{
    class Day7 : AocDay
    {
        private List<BagRule> Bags { get; set; }

        // Dictionary to keep track of which bags can contain which others
        private Dictionary<string, List<BagRule>> BagManager = new Dictionary<string, List<BagRule>>();
        public Day7(string inputFileName)
        {
            DayId = 7;
            var lines = System.IO.File.ReadAllLines(inputFileName);
            Bags = lines.Select(l => new BagRule(l)).ToList();
            foreach (var bag in Bags)
            {
                if (!BagManager.ContainsKey(bag.BagColor))
                {
                    BagManager.Add(bag.BagColor, bag.ContainedBags);
                }
                else
                {
                    throw new Exception("Duplicate");
                }
            }
        }

        public override string Part1()
        {
            int bagsContainingGold = Bags.Count(bag => bag.CanContain("shiny gold", BagManager));
            return $"{bagsContainingGold} bag can contain shiny gold bags";
        }

        public override string Part2()
        {
            return $"Not implemented yet";
        }
    }

    class BagRule
    {
        public string BagColor { get; private set; }
        // Quantity of bags covered by this rule
        public int BagQuantity { get; private set; }

        public List<BagRule> ContainedBags { get; } = new List<BagRule>();

        public bool CanContain(string color, Dictionary<string, List<BagRule>> bagManager)
        {
            foreach (var bag in bagManager[color])
            {
                if (bag.BagColor == color)
                {
                    return true;
                }
                if (bag.CanContain(color, bagManager))
                {
                    return true;
                }
            }
            return false;
        }

        Regex _definitionRegex = new Regex(@"^(\w+ \w+) bags? contain ((\d+|(?:no)) ([\w\s]+) bags?(, (\d+) (\w+ \w+) bags?)*\.)$");
        Regex _contentsRegex = new Regex(@"(\d+|(?:no)) ([\w\s]+) bags?");
        public BagRule(string definition)
        {
            var match = _definitionRegex.Match(definition);
            // Group 1 is the first bag definition
            // Group 2 is the contents of that bag
            this.BagColor = match.Groups[1].Value;
            this.BagQuantity = 1;
            foreach (Match m in _contentsRegex.Matches(match.Groups[2].Value))
            {
                if (m.Groups[1].Value == "no")
                {
                    break;
                }
                else
                {
                    int quantity = int.Parse(m.Groups[1].Value);
                    string color = m.Groups[2].Value;
                    ContainedBags.Add(new BagRule(quantity, color));
                }
            }
        }

        public BagRule(int quantity, string bagColor)
        {
            this.BagQuantity = quantity;
            this.BagColor = bagColor;
        }

        public override string ToString()
        {
            return $"{this.BagQuantity} {this.BagColor} bag(s)";
        }
    }
}
