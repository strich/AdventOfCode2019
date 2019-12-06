using System;
using System.Collections.Generic;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzle = new Day1(Input);
            Console.WriteLine($"Day 1 Puzzle 1 Answer: {puzzle.GetAnswer1()}");
            Console.WriteLine($"Day 1 Puzzle 2 Answer: {puzzle.GetAnswer2()}");
        }

        public static string Input =
        @"
54032
64433
71758
133884
76994
99596
90491
89188
142280
127352
62127
79849
96049
56527
148029
81386
149827
105377
91970
98708
88611
99785
99229
88460
80396
70097
91784
81733
75671
106787
77196
132234
98698
115243
119574
142851
58964
137814
127695
92139
106277
51240
121351
78316
129472
65201
116068
72803
52582
135433
87619
68096
116952
106437
70517
69840
89863
134618
83823
113436
103779
134819
107928
138503
82509
90104
98001
76202
136238
66426
74030
55075
124163
57133
79908
109977
66903
125400
130961
149293
99203
120307
142403
50262
52854
70851
142213
77567
149144
144582
58138
61765
116209
128192
137436
101406
69037
107389
112389
124402
";
    }

    public class Day1
    {
        List<int> _input;

        public Day1(string input)
        {
            _input = ParseInput(input);
        }

        public List<int> ParseInput(string input)
        {
            return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
        }

        public int GetAnswer1()
        {
            return _input.Select(i => CalcModuleFuel(i)).Sum();
        }

        public int GetAnswer2()
        {
            var totalFuelNeeded = 0;
            foreach (var moduleMass in _input)
            {
                var fuelNeeded = CalcModuleFuel(moduleMass);
                var totalModuleFuel = fuelNeeded;

                while (fuelNeeded > 0)
                {
                    fuelNeeded = Math.Max(CalcModuleFuel(fuelNeeded), 0);
                    totalModuleFuel += fuelNeeded;
                }

                totalFuelNeeded += totalModuleFuel;
            }
            return totalFuelNeeded;
        }

        int CalcModuleFuel(int modMass) => (modMass / 3) - 2;
    }
}
