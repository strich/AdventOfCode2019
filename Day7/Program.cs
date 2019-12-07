using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Puzzle(Input);
            Console.WriteLine($"Day 7 Puzzle 1 Answer: {puzzle.GetAnswer1()}");
            Console.WriteLine($"Day 7 Puzzle 2 Answer: {puzzle.GetAnswer2()}");
        }

        public static string Input = @"3,8,1001,8,10,8,105,1,0,0,21,30,47,60,81,102,183,264,345,426,99999,3,9,1002,9,5,9,4,9,99,3,9,1002,9,5,9,1001,9,4,9,1002,9,4,9,4,9,99,3,9,101,2,9,9,1002,9,4,9,4,9,99,3,9,1001,9,3,9,1002,9,2,9,101,5,9,9,1002,9,2,9,4,9,99,3,9,102,4,9,9,101,4,9,9,1002,9,3,9,101,2,9,9,4,9,99,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,99";
    }

    public class Puzzle
    {
        List<int> _origInput;

        public Puzzle(string input)
        {
            _origInput = ParseInput(input);
        }

        public List<int> ParseInput(string input) => input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();

        public int GetAnswer1()
        {
            var amplifiers = new List<Amplifier>();
            var phases = new List<int>() { 0,1,2,3,4 };

            for (int i = 0; i < 5; i++)
            {
                amplifiers.Add(new Amplifier(_origInput));
            }

            int biggestPhasingOutput = 0;
            foreach (var phasing in Permutate(phases, phases.Count))
            {
                int ampOutput = 0;
                Console.WriteLine($"Trying [{string.Join(",", phasing)}] phasing...");
                for (int x = 0; x < amplifiers.Count; x++)
                {
                    amplifiers[x].Reset();
                    amplifiers[x].AddInput(phases[x]);
                    amplifiers[x].AddInput(ampOutput);
                    amplifiers[x].Run(out var outputs);
                    ampOutput = outputs[0];
                }
                if (ampOutput > biggestPhasingOutput) biggestPhasingOutput = ampOutput;
            }

            return biggestPhasingOutput;
        }

        public int GetAnswer2()
        {
            var amplifiers = new List<Amplifier>();
            var phases = new List<int>() { 5,6,7,8,9 };

            for (int i = 0; i < 5; i++) amplifiers.Add(new Amplifier(_origInput));

            int biggestPhasingOutput = 0;
            foreach (var phasing in Permutate(phases, phases.Count))
            {
                Console.WriteLine($"Trying [{string.Join(",", phasing)}] phasing...");
                int ampOutput = 0;
                for (int i = 0; i < amplifiers.Count; i++)
                {
                    amplifiers[i].Reset();
                    amplifiers[i].AddInput(phases[i]);
                }

                while (!amplifiers.Last().HasHalted)
                {
                    for (int x = 0; x < amplifiers.Count; x++)
                    {
                        if (amplifiers[x].HasHalted) continue;

                        amplifiers[x].AddInput(ampOutput);

                        amplifiers[x].Run(out var outputs);
                        ampOutput = outputs[0];
                    }
                }
                
                if (ampOutput > biggestPhasingOutput) biggestPhasingOutput = ampOutput;
            }

            return biggestPhasingOutput;
        }

        public static void RotateRight<T>(IList<T> sequence, int count)
        {
            T tmp = sequence[count-1];
            sequence.RemoveAt(count - 1);
            sequence.Insert(0, tmp);
        }

        public static IEnumerable<IList<T>> Permutate<T>(IList<T> sequence, int count)
        {
            if (count == 1) yield return sequence;
            else
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var perm in Permutate(sequence, count - 1))
                        yield return perm;
                    RotateRight(sequence, count);
                }
            }
        }
    }

    public class Amplifier
    {
        List<int> _instructions;
        List<int> _buffer;
        List<int> _inputs = new List<int>();
        int _currentInstructionIndex = 0;
        int _currentInputId = 0;
        int _lastRunResult = 0;
        public int LastOutput = 0;

        public Amplifier(List<int> instructions)
        {
            _instructions = instructions;
            Reset();
        }

        public void Reset()
        {
            _buffer = new List<int>(_instructions);
            _currentInstructionIndex = 0;
            _currentInputId = 0;
            _lastRunResult = 0;
            _inputs.Clear();
        }

        public void AddInput(int input) => _inputs.Add(input);

        public bool HasHalted => _lastRunResult == 1;
        public bool IsRunning => _lastRunResult == 0;
        public bool HasErrors => _lastRunResult == -1;

        public int Run(out List<int> outputs)
        {
            _lastRunResult = CPU.Run(_buffer, _inputs, out outputs, ref _currentInputId, ref _currentInstructionIndex);
            LastOutput = outputs[0];
            return _lastRunResult;
        }
    }

    public static class CPU
    {
        enum PMode { Pos = 0, Imm = 1 }

        public static int Run(in List<int> instructions, in List<int> inputs, out List<int> outputs, ref int inputId, ref int instructionIndex)
        {
            outputs = new List<int>();
            ref int i = ref instructionIndex;
            while (true)
            {
                Instruction(instructions[i], out var opcode, out var p1, out var p2, out var p3);

                switch (opcode)
                {
                    case 1: // Add
                        //Console.WriteLine($"{i}:ADD");
                        instructions[Deref(in instructions, i + 3, PMode.Pos)] = instructions[Deref(in instructions, i + 1, p1)] + instructions[Deref(in instructions, i + 2, p2)];
                        i += 4;
                        break;
                    case 2: // Mul
                        //Console.WriteLine($"{i}:MUL");
                        instructions[Deref(in instructions, i + 3, PMode.Pos)] = instructions[Deref(in instructions, i + 1, p1)] * instructions[Deref(in instructions, i + 2, p2)];
                        i += 4;
                        break;
                    case 3: // Input
                        if (inputs.Count <= inputId) return 0;
                        var input = inputs[inputId];
                        //Console.WriteLine($"{i}:INPUT: {input}");
                        inputId++;
                        instructions[Deref(in instructions, i + 1, PMode.Pos)] = input;
                        i += 2;
                        break;
                    case 4: // Output
                        var output = instructions[Deref(in instructions, i + 1, p1)];
                        //Console.WriteLine($"{i}:OUTPUT [{output}]");
                        outputs.Add(output);
                        i += 2;
                        break;
                    case 5: // jump-if-true
                        //Console.WriteLine($"{i}:JMPT");
                        if (instructions[Deref(in instructions, i + 1, p1)] != 0) i = instructions[Deref(in instructions, i + 2, p2)];
                        else i += 3;
                        break;
                    case 6: // jump-if-false
                        //Console.WriteLine($"{i}:JMPF");
                        if (instructions[Deref(in instructions, i + 1, p1)] == 0) i = instructions[Deref(in instructions, i + 2, p2)];
                        else i += 3;
                        break;
                    case 7: // less than
                        //Console.WriteLine($"{i}:LT");
                        if (instructions[Deref(in instructions, i + 1, p1)] < instructions[Deref(in instructions, i + 2, p2)])
                            instructions[Deref(in instructions, i + 3, PMode.Pos)] = 1;
                        else
                            instructions[Deref(in instructions, i + 3, PMode.Pos)] = 0;
                        i += 4;
                        break;
                    case 8: // equals
                        //Console.WriteLine($"{i}:LT");
                        if (instructions[Deref(in instructions, i + 1, p1)] == instructions[Deref(in instructions, i + 2, p2)])
                            instructions[Deref(in instructions, i + 3, PMode.Pos)] = 1;
                        else
                            instructions[Deref(in instructions, i + 3, PMode.Pos)] = 0;
                        i += 4;
                        break;
                    case 99: // Halt
                        //Console.WriteLine($"{i}:HALT");
                        return 1;
                    default:
                        Console.WriteLine($"{i}:ERROR!");
                        return -1;
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
