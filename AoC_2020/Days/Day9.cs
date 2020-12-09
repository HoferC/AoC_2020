using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC_2020.Days
{
    class Day9 : AocDay
    {
        XmasCracker _xmas;

        public Day9(string inputFileName)
        {
            DayId = 9;
            var numbers = System.IO.File.ReadAllLines(inputFileName).Select(line => long.Parse(line)).ToList();
            _xmas = new XmasCracker(numbers, 25);
        }
        public override string Part1()
        {
            return $"First non-compliant number: {_xmas.FindFirstNonCompliantNumber()}";
        }

        public override string Part2()
        {
            long target = _xmas.FindFirstNonCompliantNumber();
            return $"Encryption weakness: {_xmas.FindEncryptionWeakness(target)}";
        }
    }

    class XmasCracker
    {
        private List<long> _numbers;
        int _currentIndex;

        public int PreambleLength { get; }

        private List<long> _preamble = new List<long>();
        public XmasCracker(List<long> numbers, int preambleLength)
        {
            _numbers = numbers;
            PreambleLength = preambleLength;
            for (int i = 0; i < preambleLength; i++)
            {
                _preamble.Add(_numbers[_currentIndex]);
                _currentIndex++;
            }
        }

        public long FindFirstNonCompliantNumber()
        {
            while (_numbers.Count > 0)
            {
                long target = _numbers[_currentIndex];
                bool compliant = _preamble.Any(num1 => _preamble.Any(num2 => num1 + num2 == target));
                if (!compliant)
                {
                    return target;
                }
                _preamble.RemoveAt(0);
                _preamble.Add(target);
                _currentIndex++;
            }
            return 0;
        }

        public long FindEncryptionWeakness(long target)
        {
            for (int i = 0; i < _numbers.Count - 1; i++)
            {
                for (int j = i; j < _numbers.Count; j++)
                {
                    var range = _numbers.GetRange(i, j - i);
                    if (range.Sum() == target)
                    {
                        return range.Min() + range.Max();
                    }
                }
            }
            return 0;
        }
    }
}
