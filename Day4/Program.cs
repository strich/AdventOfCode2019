using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Puzzle(Input);
            var a1 = puzzle.GetAnswer1();
            var a2 = puzzle.GetAnswer2();
            Console.WriteLine($"Day 4 Puzzle 1 Answer: {a1}");
            Console.WriteLine($"Day 4 Puzzle 2 Answer: {a2}");
        }

        public static string Input = @"158126-624574";
    }

    class Puzzle
    {
        (string min, string max) _input;

        public Puzzle(string input)
        {
            _input = ParseInput(input);
        }

        (string min, string max) ParseInput(string input)
        {
            var splitStr = input.Split("-");
            return (splitStr[0], splitStr[1]);
        }

        readonly int[] _intBuffer = new int[6];
        bool IsValidPassword(int password, bool ignoreLargeGroups)
        {
            if (password > 1000000) return false;
            ToDigitArray(password, in _intBuffer);

            var groupCount = 1;
            var hasDouble = false;
            for (int i = 0; i < _intBuffer.Length - 1; i++)
            {
                if (_intBuffer[i] > _intBuffer[i + 1]) return false;
                if (_intBuffer[i] == _intBuffer[i + 1])
                {
                    groupCount++;

                    if(i + 2 >= _intBuffer.Length)
                    {
                        if (ignoreLargeGroups)
                        {
                            if (groupCount >= 2)
                                hasDouble = true;
                        }
                        else
                        {
                            if (groupCount == 2)
                                hasDouble = true;
                        }
                    }
                }
                else
                {
                    if (ignoreLargeGroups)
                    {
                        if (groupCount >= 2)
                            hasDouble = true;
                    }
                    else
                    {
                        if (groupCount == 2)
                            hasDouble = true;
                    }
                    
                    groupCount = 1;
                }
            }

            if (!hasDouble) return false;

            return true;
        }

        public int GetAnswer1()
        {
            int min = int.Parse(_input.min);
            int max = int.Parse(_input.max);
            List<int> validPasswords = new List<int>();
            for (var i = min; i <= max; i++)
            {
                Console.WriteLine($"Trying password [{i}]...");
                if (IsValidPassword(i, true))
                {
                    validPasswords.Add(i);
                    Console.WriteLine($"Valid password: {i}");
                }
            }

            return validPasswords.Count;
        }

        public int GetAnswer2()
        {
            int min = int.Parse(_input.min);
            int max = int.Parse(_input.max);
            List<int> validPasswords = new List<int>();
            for (var i = min; i <= max; i++)
            {
                Console.WriteLine($"Trying password [{i}]...");
                if (IsValidPassword(i, false))
                {
                    validPasswords.Add(i);
                    Console.WriteLine($"Valid password: {i}");
                }
            }

            return validPasswords.Count;
        }

        public static void ToDigitArray(int pw, in int[] result)
        {
            for (int i = result.Length-1; i >= 0; i--)
            {
                result[i] = pw % 10;
                pw /= 10;
            }
        }
    }
}
