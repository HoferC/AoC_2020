using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC_2020.Days
{
    public class Day11 : AocDay
    {
        SeatingMap _map;

        public Day11(string inputFileName)
        {
            DayId = 11;
            var lines = System.IO.File.ReadAllLines(inputFileName);
            // Build up a map from the lines
            _map = new SeatingMap(lines);
        }

        public override string Part1()
        {
            int stepCount = 0;
            while (_map.Step())
            {
                // Do Nothing
                stepCount++;
            }
            return $"Steady State after {stepCount-1} steps with {_map.OccupiedSeats} seats occupied.";
        }

        public override string Part2()
        {
            return "";
        }
    }

    public class SeatingMap
    {
        private List<List<SeatingSquare>> _rows = new List<List<SeatingSquare>>();

        public int OccupiedSeats
        {
            get
            {
                int result = 0;
                foreach (var row in _rows)
                {
                    result += row.Count(s => s.State == SeatingSquareState.Occupied);
                }
                return result;
            }
        }

        /// <summary>
        /// Build up a SeatingMap from the puzzle input
        /// </summary>
        /// <param name="input">A map of seats in text format - L is empty, # is occupied, and . is floor.</param>
        public SeatingMap(IList<string> input)
        {
            for (int rowIdx = 0; rowIdx < input.Count; rowIdx++)
            {
                var row = new List<SeatingSquare>();
                for (int colIdx = 0; colIdx < input[rowIdx].Length; colIdx++)
                {
                    Coordinate c = new Coordinate(rowIdx, colIdx);
                    SeatingSquareState state;
                    switch (input[rowIdx][colIdx])
                    {
                        case 'L':
                            state = SeatingSquareState.Empty;
                            break;
                        case '#':
                            state = SeatingSquareState.Occupied;
                            break;
                        default:
                            state = SeatingSquareState.Floor;
                            break;
                    }
                    row.Add(new SeatingSquare { Location = c, State = state });
                }
                _rows.Add(row);
            }
            // Once we have the initial map loaded,
            // we can go through and identify the adjacency of the squares
            for (int rowIdx = 0; rowIdx < input.Count; rowIdx++)
            {
                for (int colIdx = 0; colIdx < input[rowIdx].Length; colIdx++)
                {
                    // There are a max of 8 adjacent seats for any given seat:
                    // x-1, y
                    // x-1, y-1
                    // x-1, y+1
                    // x, y-1
                    // x, y+1
                    // x+1, y
                    // x+1, y-1
                    // x+1, y+1
                    if (rowIdx - 1 >= 0)
                    {
                        _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx - 1][colIdx]);

                        if (colIdx - 1 >= 0)
                        {
                            _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx - 1][colIdx - 1]);
                        }
                        if (colIdx + 1 < input[rowIdx].Length)
                        {
                            _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx - 1][colIdx + 1]);
                        }
                    }
                    if (rowIdx + 1 < _rows.Count)
                    {
                        _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx + 1][colIdx]);

                        if (colIdx - 1 >= 0)
                        {
                            _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx + 1][colIdx - 1]);
                        }
                        if (colIdx + 1 < input[rowIdx].Length)
                        {
                            _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx + 1][colIdx + 1]);
                        }
                    }
                    if (colIdx - 1 >= 0)
                    {
                        _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx][colIdx - 1]);
                    }
                    if (colIdx + 1 < input[rowIdx].Length)
                    {
                        _rows[rowIdx][colIdx].AdjacentSeats.Add(_rows[rowIdx][colIdx + 1]);
                    }
                }
            }
        }

        public bool Step()
        {
            bool result = false;
            if (Program.DEBUG)
            {
                Console.WriteLine("Before");
                Console.WriteLine(this);
            }
            foreach (var row in _rows)
            {
                foreach (var seat in row)
                {
                    // Step each seat and if it changes, then we mark this as a change
                    result = seat.Step() || result;
                }
            }
            // Once we've evaluated everything, actually take the step
            foreach (var row in _rows)
            {
                foreach (var seat in row)
                {
                    // Step each seat and if it changes, then we mark this as a change
                    seat.TakeStep();
                }
            }
            if (Program.DEBUG)
            {
                Console.WriteLine("After");
                Console.WriteLine(this);
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var row in _rows)
            {
                string rowString = String.Concat(row.SelectMany(r =>
                {
                    switch (r.State)
                    {
                        case SeatingSquareState.Floor:
                            return ".";
                        case SeatingSquareState.Empty:
                            return "L";
                        case SeatingSquareState.Occupied:
                            return "#";
                    }
                    return "";
                }));
                result.Append(rowString);
                result.Append(" ");
                rowString = String.Concat(row.SelectMany(r =>
                {
                    switch (r.NextState)
                    {
                        case SeatingSquareState.Floor:
                            return ".";
                        case SeatingSquareState.Empty:
                            return "L";
                        case SeatingSquareState.Occupied:
                            return "#";
                    }
                    return "";
                }));
                result.AppendLine(rowString);
            }
            return result.ToString();
        }
    }

    public class SeatingSquare
    {
        public Coordinate Location { get; set; }
        public SeatingSquareState State { get; set; }
        public SeatingSquareState NextState { get; private set; }
        public List<SeatingSquare> AdjacentSeats = new List<SeatingSquare>();

        /// <summary>
        /// Take a step for this square
        /// </summary>
        /// <returns>True if the state of this seat changes, false otherwise.</returns>
        public bool Step()
        {
            /*
             * If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
             * If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
             * Otherwise, the seat's state does not change.
            */
            NextState = State;
            if (State == SeatingSquareState.Empty)
            {
                if (AdjacentSeats.Count(s => s.State == SeatingSquareState.Occupied) == 0)
                {
                    NextState = SeatingSquareState.Occupied;
                    return true;
                }
            }
            else if (State == SeatingSquareState.Occupied)
            {
                if (AdjacentSeats.Count(s => s.State == SeatingSquareState.Occupied) >= 4)
                {
                    NextState = SeatingSquareState.Empty;
                    return true;
                }
            }
            return false;
        }

        public void TakeStep()
        {
            State = NextState;
        }

        public override string ToString()
        {
            return $"({Location.X}, {Location.Y}: {State} -> {NextState}";
        }

    }

    public enum SeatingSquareState
    {
        Floor,
        Empty,
        Occupied
    }
}

