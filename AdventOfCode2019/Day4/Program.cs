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
            Console.WriteLine($"Day 4 Puzzle 1 Answer: {puzzle.GetAnswer1()}");
            Console.WriteLine($"Day 4 Puzzle 2 Answer: {puzzle.GetAnswer2()}");
        }

        public static string Input = @"158126-624574";
    }

    class Puzzle
    {
        (string min, string max) _input;

        struct LineSegment
        {
            public Point A;
            public Point B;
        }

        struct Point
        {
            public int X;
            public int Y;
        }

        public Puzzle(string input)
        {
            _input = ParseInput(input);
        }

        (string min, string max) ParseInput(string input)
        {
            var splitStr = input.Split("-");
            return (splitStr[0], splitStr[1]);
        }

        int[] _intBuffer = new int[6];
        bool IsValidPassword(int password)
        {
            if (password > 1000000) return false;
            ToDigitArray(password, in _intBuffer);

            var hasDouble = false;
            for (int i = 0; i < _intBuffer.Length - 1; i++)
            {
                if (_intBuffer[i] > _intBuffer[i + 1]) return false;
                if (_intBuffer[i] == _intBuffer[i + 1])
                {
                    if (i + 2 < _intBuffer.Length && _intBuffer[i] == _intBuffer[i + 2])
                    { }
                    else
                    {
                        hasDouble = true;
                    }
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
                if (IsValidPassword(i))
                {
                    validPasswords.Add(i);
                    Console.WriteLine($"Valid password: {i}");
                }
            }

            return validPasswords.Count();
        }

        public int GetAnswer2()
        {
            return 0;
        }

        public static void ToDigitArray(int pw, in int[] result)
        {
            for (int i = result.Length-1; i >= 0; i--)
            {
                result[i] = pw % 10;
                pw /= 10;
            }
            //while (i != 0)
            //{
            //    result.Add(i % 10);
            //    i /= 10;
            //}

            //result.Reverse();
        }
    }
}
