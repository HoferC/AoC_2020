using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace AoC_2020
{
    class AocPassword
    {
        string Value { get; set; }
        char Key { get; set; }
        int MinOccurrences { get; set; }
        int MaxOccurrences { get; set; }

        public AocPassword(string passwordWithPolicy)
        {
            Regex r = new Regex(@"(\d+)-(\d+) (\w): (\w+)");
            var match = r.Match(passwordWithPolicy);
            MinOccurrences = int.Parse(match.Groups[1].Value);
            MaxOccurrences = int.Parse(match.Groups[2].Value);
            Key = char.Parse(match.Groups[3].Value);
            Value = match.Groups[4].Value;
        }

        public bool IsValid1
        {
            get
            {
                int keyCount = Value.Where(c => c == Key).Count();
                return MinOccurrences <= keyCount && keyCount <= MaxOccurrences;
            }
        }

        public bool IsValid2
        {
            get
            {
                return Value[MinOccurrences - 1] == Key ^ Value[MaxOccurrences - 1] == Key;
            }
        }
    }
}
