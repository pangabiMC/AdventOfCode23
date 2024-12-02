using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class Day2 : IDay
    {
        public const int MaxReds = 12;
        public const int MaxGreens = 13;
        public const int MaxBlues = 14;

        public class Game
        {
            public int Id { get; }
            public List<int> Red { get; } = new List<int>();
            public List<int> Green { get; } = new List<int>();
            public List<int> Blue { get;  } = new List<int>();

            public int Power => Red.Max(x => x) * Green.Max(x => x) * Blue.Max(x => x);

            public Game(string input)
            {
                input = input.Substring("Game ".Length);
                Id = int.Parse(input.Substring(0, input.IndexOf(':')));

                input = input.Substring(input.IndexOf(":") + 1);
                foreach (var take in input.Split(';'))
                {
                    foreach(var c in take.Split(','))
                    {
                        var m = Regex.Match(c, @"\s*(\d+)\s*([a-z]+)");
                        if(m.Success)
                        {
                            var amount = int.Parse(m.Groups[1].Value);
                            switch(m.Groups[2].Value.ToLower())
                            {
                                case "red":
                                    Red.Add(amount);
                                    break;
                                case "green":
                                    Green.Add(amount);
                                    break;
                                case "blue":
                                    Blue.Add(amount);
                                    break;
                            }
                        }
                    }
                }
            }

            public bool IsValid(int maxRed, int maxGreen, int maxBlue)
            {
                return Red.All(i => i <= maxRed) && Blue.All(i => i <= maxBlue) && Green.All(i => i <= maxGreen);
            }

            public override string ToString()
            {
                return $"{Id}: Reds: {String.Join(",", Red)}, Greens: {String.Join(",", Green)}, Blues: {String.Join(",", Blue)}";
            }
        }

        public long DoIt(string[] input)
        {
            int validGamesSum = 0;
            int allPowerSum = 0;
            foreach (var l in input)
            {
                Game g = new Game(l);
                if(g.IsValid(MaxReds, MaxGreens, MaxBlues))
                {
                    validGamesSum += g.Id;
                    //System.Diagnostics.Debug.WriteLine(g.Id);
                }
                allPowerSum += g.Power;
            }

            System.Diagnostics.Debug.WriteLine(validGamesSum);
            return allPowerSum;
        }
    }
}
