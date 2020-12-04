using System;
using System.Collections.Generic;
using System.Text;

namespace AoC_2020
{
    struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate((a.X + b.X), (a.Y + b.Y));
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
