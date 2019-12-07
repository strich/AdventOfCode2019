using System;
using System.Collections.Generic;
using System.Linq;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Puzzle(Input);
            Console.WriteLine($"Day 5 Puzzle 1 Answer: {puzzle.GetAnswer1()}");
        }

        public static string Input = @"3,225,1,225,6,6,1100,1,238,225,104,0,1101,9,90,224,1001,224,-99,224,4,224,102,8,223,223,1001,224,6,224,1,223,224,223,1102,26,62,225,1101,11,75,225,1101,90,43,225,2,70,35,224,101,-1716,224,224,4,224,1002,223,8,223,101,4,224,224,1,223,224,223,1101,94,66,225,1102,65,89,225,101,53,144,224,101,-134,224,224,4,224,1002,223,8,223,1001,224,5,224,1,224,223,223,1102,16,32,224,101,-512,224,224,4,224,102,8,223,223,101,5,224,224,1,224,223,223,1001,43,57,224,101,-147,224,224,4,224,102,8,223,223,101,4,224,224,1,223,224,223,1101,36,81,225,1002,39,9,224,1001,224,-99,224,4,224,1002,223,8,223,101,2,224,224,1,223,224,223,1,213,218,224,1001,224,-98,224,4,224,102,8,223,223,101,2,224,224,1,224,223,223,102,21,74,224,101,-1869,224,224,4,224,102,8,223,223,1001,224,7,224,1,224,223,223,1101,25,15,225,1101,64,73,225,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,1008,226,677,224,1002,223,2,223,1005,224,329,1001,223,1,223,1007,677,677,224,102,2,223,223,1005,224,344,101,1,223,223,108,226,677,224,102,2,223,223,1006,224,359,101,1,223,223,108,226,226,224,1002,223,2,223,1005,224,374,1001,223,1,223,7,226,226,224,1002,223,2,223,1006,224,389,1001,223,1,223,8,226,677,224,1002,223,2,223,1006,224,404,1001,223,1,223,107,677,677,224,1002,223,2,223,1006,224,419,101,1,223,223,1008,677,677,224,102,2,223,223,1006,224,434,101,1,223,223,1107,226,677,224,102,2,223,223,1005,224,449,1001,223,1,223,107,226,226,224,102,2,223,223,1006,224,464,101,1,223,223,107,226,677,224,102,2,223,223,1005,224,479,1001,223,1,223,8,677,226,224,102,2,223,223,1005,224,494,1001,223,1,223,1108,226,677,224,102,2,223,223,1006,224,509,101,1,223,223,1107,677,226,224,1002,223,2,223,1005,224,524,101,1,223,223,1008,226,226,224,1002,223,2,223,1005,224,539,101,1,223,223,7,226,677,224,1002,223,2,223,1005,224,554,101,1,223,223,1107,677,677,224,1002,223,2,223,1006,224,569,1001,223,1,223,8,226,226,224,1002,223,2,223,1006,224,584,101,1,223,223,1108,677,677,224,102,2,223,223,1005,224,599,101,1,223,223,108,677,677,224,1002,223,2,223,1006,224,614,101,1,223,223,1007,226,226,224,102,2,223,223,1005,224,629,1001,223,1,223,7,677,226,224,1002,223,2,223,1005,224,644,101,1,223,223,1007,226,677,224,102,2,223,223,1005,224,659,1001,223,1,223,1108,677,226,224,102,2,223,223,1006,224,674,101,1,223,223,4,223,99,226";
    }

    public class Puzzle
    {
        enum PMode
        {
            Pos = 0, Imm = 1
        }

        List<int> _origInput;

        public Puzzle(string input)
        {
            _origInput = ParseInput(input);
        }

        public List<int> ParseInput(string input) => input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();

        public int GetAnswer1()
        {
            var input = new List<int>(_origInput);
            var output = RunComputer(in input);

            return input[0];
        }

        bool RunComputer(in List<int> reg)
        {
            int i = 0;
            while (true)
            {
                Instruction(reg[i], out var opcode, out var p1, out var p2, out var p3);

                switch (opcode)
                {
                    case 1: // Add
                        Console.WriteLine($"{i}:ADD");
                        reg[Deref(in reg, i + 3, PMode.Pos)] = reg[Deref(in reg, i + 1, p1)] + reg[Deref(in reg, i + 2, p2)];
                        i += 4;
                        break;
                    case 2: // Mul
                        Console.WriteLine($"{i}:MUL");
                        reg[Deref(in reg, i + 3, PMode.Pos)] = reg[Deref(in reg, i + 1, p1)] * reg[Deref(in reg, i + 2, p2)];
                        i += 4;
                        break;
                    case 3: // Input
                        Console.Write($"{i}:INPUT: ");
                        var input = Console.ReadLine();
                        reg[Deref(in reg, i + 1, PMode.Pos)] = int.Parse(input);
                        i += 2;
                        break;
                    case 4: // Output
                        Console.WriteLine($"{i}:OUTPUT [{reg[Deref(in reg, i + 1, p1)]}]");
                        i += 2;
                        break;
                    case 5: // jump-if-true
                        Console.WriteLine($"{i}:JMPT");
                        if (reg[Deref(in reg, i + 1, p1)] != 0) i = reg[Deref(in reg, i + 2, p2)];
                        else i += 3;
                        break;
                    case 6: // jump-if-false
                        Console.WriteLine($"{i}:JMPF");
                        if (reg[Deref(in reg, i + 1, p1)] == 0) i = reg[Deref(in reg, i + 2, p2)];
                        else i += 3;
                        break;
                    case 7: // less than
                        Console.WriteLine($"{i}:LT");
                        if (reg[Deref(in reg, i + 1, p1)] < reg[Deref(in reg, i + 2, p2)]) 
                            reg[Deref(in reg, i + 3, PMode.Pos)] = 1;
                        else
                            reg[Deref(in reg, i + 3, PMode.Pos)] = 0;
                        i += 4;
                        break;
                    case 8: // equals
                        Console.WriteLine($"{i}:LT");
                        if (reg[Deref(in reg, i + 1, p1)] == reg[Deref(in reg, i + 2, p2)])
                            reg[Deref(in reg, i + 3, PMode.Pos)] = 1;
                        else
                            reg[Deref(in reg, i + 3, PMode.Pos)] = 0;
                        i += 4;
                        break;
                    case 99: // Halt
                        Console.WriteLine($"{i}:HALT");
                        return true;
                    default:
                        Console.WriteLine($"{i}:ERROR!");
                        return false;
                }
            }
        }

        static void Instruction(int instruction, out int opcode, out PMode p1, out PMode p2, out PMode p3)
        {
            opcode = instruction % 100;
            p1 = (PMode)(instruction / 100 % 10);
            p2 = (PMode)(instruction / 1000 % 10);
            p3 = (PMode)(instruction / 10000 % 10);
        }

        static int Deref(in List<int> input, int index, PMode pm) => pm == PMode.Pos ? input[index] : index;
    }
}
