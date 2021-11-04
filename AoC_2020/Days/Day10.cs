using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC_2020.Days
{
    class Day10 : AocDay
    {
        /// <summary>
        /// The list of all adapters in your bag.
        /// </summary>
        List<int> Adapters;

        /// <summary>
        /// A cache of path lengths
        /// </summary>
        Dictionary<int, long> pathLengths = new Dictionary<int, long>();

        public Day10(string inputFileName)
        {
            DayId = 10;
            var lines = System.IO.File.ReadAllLines(inputFileName);
            Adapters = lines.Select(line => int.Parse(line)).ToList();
            Console.WriteLine($"Loaded {Adapters.Count} adapters");
        }


        public override string Part1()
        {
            // Having this list be sorted may help us.
            Adapters.Sort();
            int lastJoltage = 0;
            int onesCount = 0;
            // Threes count is 1 because your device always counts as a 3.
            int threesCount = 1;
            foreach (var a in Adapters)
            {
                if (a - lastJoltage == 1)
                {
                    onesCount++;
                }
                else if (a - lastJoltage == 3)
                {
                    threesCount++;
                }
                lastJoltage = a;
            }
            return $"The jolt chain checksum is {onesCount * threesCount}";
        }

        public override string Part2()
        {
            // Add the 0 port and the device
            Adapters.Add(0);
            Adapters.Add(Adapters.Max() + 3);
            // Sort the adapters because they have to be within 3 of each other
            Adapters.Sort();

            long result = PathsFromHere(0);

            return $"There are {result} combinations.";
        }

        public long PathsFromHere(int index)
        {
            if (pathLengths.ContainsKey(index))
            {
                return pathLengths[index];
            }

            // Keep track of the adapter at the given index
            int adapterOfInterest = Adapters[index];

            // If we are the last adapter (your device) return 1 because there's only one path
            if (index == Adapters.Count-1)
            {
                return 1;
            }

            // Find the list of indices that will be compatible with this index
            List<int> compatibleAdapters = new List<int>();

            // Only go up to the index + 4 because there are no duplicates so we won't need to look at that many indices
            for (int i = index + 1; i < Math.Min(index + 4, Adapters.Count); i++)
            {
                if (Adapters[i] - adapterOfInterest <= 3)
                {
                    compatibleAdapters.Add(i);
                }
            }
            long result = compatibleAdapters.Sum(a => PathsFromHere(a));

            // Cache this value so we don't have to calculate it again
            pathLengths.Add(index, result);
            return result;
        }
    }
}
