using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC_2020.Days
{
    class Day8 : AocDay
    {
        string[] _lines;
        public Day8(string inputFileName)
        {
            DayId = 8;
            _lines = System.IO.File.ReadAllLines(inputFileName);

        }
        public override string Part1()
        {
            GameConsole console = new GameConsole(_lines);
            console.Boot();
            return $"Infinite loop detected when accumulator value was {console.ACC}";
        }

        public override string Part2()
        {
            GameConsole console = new GameConsole(_lines);
            for (int i = 0; i < _lines.Count(); i++)
            {
                if (console.Boot(i))
                {
                    return $"Fixed line {i} - Accumulator value for successful program: {console.ACC}";
                }
            }
            return $"No fix found.";
        }
    }

    class GameConsole
    {
        /// <summary>
        /// Accumulator for the game console
        /// </summary>
        public int ACC { get; private set; }

        /// <summary>
        /// The program counter pointing to where we currently are
        /// </summary>
        public int PC { get; private set; }

        List<Instruction> _program;

        List<Instruction> _modifiedProgram;

        public GameConsole(IList<string> program)
        {
            _program = program.Select(line => new Instruction(line)).ToList();
        }
        
        /// <summary>
        /// Reset the program counter and accumulator to 0
        /// </summary>
        public void Reset()
        {
            PC = 0;
            ACC = 0;
        }

        public bool Boot(int lineToFlip = -1)
        {
            _modifiedProgram = _program.ToList();
            if (lineToFlip != -1)
            {
                switch(_modifiedProgram[lineToFlip].Token)
                {
                    case "acc":
                        break;
                    case "jmp":
                        _modifiedProgram[lineToFlip] = new Instruction { Token = "nop", Value = _modifiedProgram[lineToFlip].Value };
                        break;
                    case "nop":
                        _modifiedProgram[lineToFlip] = new Instruction { Token = "jmp", Value = _modifiedProgram[lineToFlip].Value };
                        break;
                }
            }
            Reset();
            List<int> visitedPcValues = new List<int>();
            while (!visitedPcValues.Contains(PC))
            {
                if (PC >= _modifiedProgram.Count)
                {
                    break;
                }
                visitedPcValues.Add(PC);
                Instruction currentInstruction = _modifiedProgram[PC];
                switch (currentInstruction.Token)
                {
                    case "acc":
                        ACC += currentInstruction.Value;
                        PC++;
                        break;
                    case "jmp":
                        PC += currentInstruction.Value;
                        break;
                    case "nop":
                        PC++;
                        break;
                }
            }
            // If we fell off the end, the PC won't be in the list
            // If we found a loop, the PC will be in the list.
            return !visitedPcValues.Contains(PC);
        }

        struct Instruction
        {
            public string Token;
            public int Value;

            public Instruction(string line)
            {
                var strings = line.Split(" ");
                Token = strings[0];
                Value = int.Parse(strings[1]);
            }
        }
    }
}
