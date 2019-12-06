using System;
using System.Collections.Generic;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Puzzle(Input);
            Console.WriteLine($"Day 2 Puzzle 1 Answer: {puzzle.GetAnswer1()}");
            Console.WriteLine($"Day 2 Puzzle 2 Answer: {puzzle.GetAnswer2()}");
        }

        public static string Input = @"1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,6,19,1,9,19,23,2,23,10,27,1,27,5,31,1,31,6,35,1,6,35,39,2,39,13,43,1,9,43,47,2,9,47,51,1,51,6,55,2,55,10,59,1,59,5,63,2,10,63,67,2,9,67,71,1,71,5,75,2,10,75,79,1,79,6,83,2,10,83,87,1,5,87,91,2,9,91,95,1,95,5,99,1,99,2,103,1,103,13,0,99,2,14,0,0";
    }

    public class Puzzle
    {
        List<int> _input;

        public Puzzle(string input)
        {
            _input = ParseInput(input);
        }

        public List<int> ParseInput(string input) => input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();

        public int GetAnswer1()
        {
            _input[1] = 12;
            _input[2] = 2;

            var output = RunComputer(_input);

            return output[0];
        }

        List<int> RunComputer(List<int> input)
        {
            List<int> output = new List<int>(input);

            for (int i = 0; i <= input.Count - 4; i += 4)
            {
                switch (input[i])
                {
                    case 1: // Add
                        Console.WriteLine($"{i}:ADD");
                        output[Reg(i + 3)] = output[Reg(i + 1)] + output[Reg(i + 2)];
                        break;
                    case 2: // Mul
                        Console.WriteLine($"{i}:MUL");
                        output[Reg(i + 3)] = output[Reg(i + 1)] * output[Reg(i + 2)];
                        break;
                    case 99: // Halt
                        Console.WriteLine($"{i}:HALT");
                        return output;
                    default:
                        Console.WriteLine($"{i}:ERROR!");
                        break;
                }
            }

            return output;
        }

        int Reg(int index) => _input[index];

        public int GetAnswer2()
        {
            var wantedOutput = 19690720;

            for (var noun = 0; noun < 100; noun++)
            {
                for (var verb = 0; verb < 100; verb++)
                {
                    Console.WriteLine($"Run [{noun},{verb}]");
                    _input[1] = noun;
                    _input[2] = verb;

                    var output = RunComputer(_input);

                    if (output[0] == wantedOutput) return 100 * noun + verb;
                }
            }

            Console.WriteLine("FAILED");
            return 0;
        }
    }
}
