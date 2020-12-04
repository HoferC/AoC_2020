using System;
using System.Collections.Generic;
using System.Text;

namespace AoC_2020
{
    class TobogganRun
    {
        private string[] _mapDefinition;

        public int MapHeight { get; }
        public int MapSliceWidth { get; }

        public Coordinate CurrentCoordinate { get; set; }

        private Coordinate _offsetPerStep = new Coordinate(3, 1);

        public TobogganRun(string[] mapDefinition)
        {
            _mapDefinition = mapDefinition;
            MapHeight = _mapDefinition.Length;
            MapSliceWidth = _mapDefinition[0].Length;
            CurrentCoordinate = new Coordinate(0, 0);
        }

        public string CalculateRun(Coordinate stepToTake, out int treesHit)
        {
            treesHit = 0;
            CurrentCoordinate = new Coordinate(0, 0);
            // Go until we reach the end in the Y direction
            while (CurrentCoordinate.Y <= MapHeight)
            {
                Coordinate newCoord = CurrentCoordinate + stepToTake;
                if (!IsAtBottom(newCoord))
                {
                    break;
                }
                if (IsTreeAtLocation(newCoord))
                {
                    treesHit++;
                }
                CurrentCoordinate = newCoord;
            }
            return $"{treesHit} trees were encountered. Ended at position {CurrentCoordinate}";
        }

        public string CalculateRunProducts(List<Coordinate> steps)
        {
            long runningProduct = 1;
            foreach(var step in steps)
            {
                int tempResult;
                Console.WriteLine($"Step: {step}, {CalculateRun(step, out tempResult)}");
                runningProduct *= tempResult;
            }

            return $"{runningProduct} is the multipled tree number.";
        }

        
        public bool IsAtBottom(Coordinate location)
        {
            return location.Y < MapHeight;
        }

        public bool IsTreeAtLocation(Coordinate location)
        {
            if (location.X >= MapSliceWidth)
            {
                location.X = location.X % MapSliceWidth;
            }
            return _mapDefinition[location.Y][location.X] == '#';
        }
    }
}
