using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AoC_2020.Days
{
    public class Day5 : AocDay
    {
        private List<BoardingPass> _boardingPasses = new List<BoardingPass>();
        
        public override string Part1()
        {
            return $"Max Boarding Pass SeatId: {_boardingPasses.Max(bp => bp.SeatID)}";
        }

        public override string Part2()
        {
            // Find the one that skips a number
            _boardingPasses.Sort();
            int myId = -1;
            int lastId = _boardingPasses[0].SeatID;
            foreach (var bp in _boardingPasses)
            {
                if (bp.SeatID - lastId > 1)
                {
                    myId = lastId + 1;
                }
                lastId = bp.SeatID;
            }

            return $"Your seat was ID {myId}";
        }

        public Day5(string filename)
        {
            var lines = File.ReadAllLines(filename);
            _boardingPasses = lines.Select(l => new BoardingPass(l)).ToList();
        }

        public class BoardingPass : IComparable
        {
            public int SeatID
            {
                get
                {
                    return RowNumber * 8 + ColNumber;
                }
            }

            public int RowNumber { get; set; }
            public int ColNumber { get; set; }

            public BoardingPass(string input)
            {
                RowNumber = BinarySearch(input.Substring(0, 7), 'F', 'B', 0, 127);
                ColNumber = BinarySearch(input.Substring(7), 'L', 'R', 0, 7);
            }

            private int BinarySearch(string input, char lower, char upper, int min, int max)
            {
                foreach (var c in input)
                {
                    // take the lower half (0-63 in example)
                    if (c == lower)
                    {
                        min = min;
                        max = min + (max - min) / 2;
                    }
                    else if (c == upper)
                    {
                        min = max - (max - min) / 2;
                        max = max;
                    }
                    else
                    {
                        throw new Exception($"Bad input {input}");
                    }
                }
                if (min == max)
                {
                    return min;
                }
                throw new Exception("Failed to resolve");
            }

            public int CompareTo(object obj)
            {
                if (obj is BoardingPass)
                {
                    return this.SeatID.CompareTo((obj as BoardingPass).SeatID);
                }
                return 1;
            }
        }

        
    }
}
