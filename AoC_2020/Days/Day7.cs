using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC_2020.Days
{
    class Day7 : AocDay
    {
        private List<Bag> Bags { get; set; }

        public Day7(string inputFileName)
        {
            DayId = 7;

            var lines = System.IO.File.ReadAllLines(inputFileName);

            Bags = lines.Select(l => new Bag(l)).ToList();
            // Need to go through and resolve the containing bag strings
            foreach (var b in Bags)
            {
                b.ResolveContainedBags(Bags);
            }
        }

        public override string Part1()
        {
            int bagsContainingGold = Bags.Count(bag => bag.CanContain("shiny gold"));
            return $"{bagsContainingGold} bags can contain shiny gold bags";
        }

        public override string Part2()
        {
            return $"The shiny gold bag can contain {Bags.Single(b => b.Color == "shiny gold").GetContainedBagQuantity()} other bags";
        }
    }

    /// <summary>
    /// A bag which can hold other bags
    /// </summary>
    class Bag
    {
        public string Color { get; private set; }

        public List<Tuple<Bag, int>> ContainedBags { get; } = new List<Tuple<Bag, int>>();

        private string _containsString;

        /// <summary>
        /// Create a new Bag based on a definition string
        /// </summary>
        /// <param name="definition">A definition string of the format {{color}} bags contain {{other bags}}</param>
        public Bag(string definition)
        {
            var match = Regex.Match(definition, @"^(\w+ \w+) bags contain (.*).$");
            Color = match.Groups[1].Value;
            _containsString = match.Groups[2].Value;
        }

        /// <summary>
        /// Resolve the bags contained within this bag based on a complete inventory of all possible bag colors
        /// </summary>
        /// <param name="allBags"></param>
        public void ResolveContainedBags(List<Bag> allBags)
        { 
            foreach (var bagDef in _containsString.Split(","))
            {
                var match = Regex.Match(bagDef, @"(\d+|(?:no)) ([\w\s]+) bags?");
                string quantityString = match.Groups[1].Value;
                if (quantityString != "no")
                {
                    int quantity = int.Parse(quantityString);
                    string color = match.Groups[2].Value;
                    // Add the contained bag to the list
                    ContainedBags.Add(new Tuple<Bag, int>(allBags.Single(b => b.Color == color), quantity));
                }
            }
        }

        /// <summary>
        /// Gets the total number of bags that can be contained within this one (not including this one)
        /// </summary>
        /// <param name="isFirst">Should be set true for the root bag, and false by any subsequent recursion.</param>
        /// <returns>The quantity of bags that can be contained within this bag, not including itself.</returns>
        public int GetContainedBagQuantity(bool isFirst = true)
        {
            if (ContainedBags.Count == 0)
            {
                return 1;
            }
            else
            {
                // Start at 1 to count yourself
                int result = 1;
                foreach (var tuple in ContainedBags)
                {
                    result += tuple.Item2 * tuple.Item1.GetContainedBagQuantity(false);
                }
                if (isFirst)
                {
                    result--;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets whether this bag can contain a bag of the given color
        /// </summary>
        /// <param name="color">Color of the bag you want to put in this bag</param>
        /// <param name="visited">A private/internal tracking set to manage which bags have been visited.</param>
        /// <returns>True if this bag (or any of its contents) can contain a bag of the given color, false otherwise.</returns>
        public bool CanContain(string color, HashSet<Bag> visited = null)
        {
            // Need to keep track of visited bags to make sure we don't get caught in an infinite loop
            if (visited == null)
            {
                visited = new HashSet<Bag>();
            }
            // Base case -- if this bag has already been visited
            if (visited.Contains(this))
            {
                return false;
            }
            visited.Add(this);
            // Base case -- if you can directly hold the bag in question
            if (ContainedBags.Any(tup => tup.Item1.Color == color))
            {
                return true;
            }
            else
            {
                bool result = false;
                foreach (var b in ContainedBags)
                {
                    result = result || b.Item1.CanContain(color, visited);
                }
                return result;
            }
        }

        public override string ToString()
        {
            return Color;
        }
    }
}
