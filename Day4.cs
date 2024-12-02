using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class Day4 : IDay
    {
        public long DoIt(string[] input)
        {
            int answer = 0;
            int[] cards = Enumerable.Repeat<int>(1, input.Length).ToArray();

            foreach (string row in input)
            {
                //Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
                if (row.Contains('|'))
                {
                    int id = int.Parse(Regex.Match(row, @"(\d+):.+").Groups[1].Value) - 1;
                    var pre = row.Split('|')[0].Split(':')[1].Trim();
                    var winning = pre.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim())).ToHashSet();
                    var ourNumbers = row.Split('|')[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim())).ToHashSet();
                    int matches = ourNumbers.Count(i => winning.Contains(i));
                    
                    for(int i = 0; i < matches; ++i)
                    {
                        cards[id + i + 1] += cards[id];
                    }
                }
            }

            answer = cards.Sum();
            return answer;
        }




        public long DoIt_1(string[] input)
        {
            int answer = 0;
            foreach (string row in input)
            {
                //Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
                if (row.Contains('|'))
                {
                    var pre = row.Split('|')[0].Split(':')[1].Trim();
                    var winning = pre.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim())).ToHashSet();
                    var ourNumbers = row.Split('|')[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim())).ToHashSet();
                    int matches = ourNumbers.Count(i => winning.Contains(i));
                    answer += (int)Math.Pow(2, matches - 1);

                    //int.Parse(Regex.Match(pre, @"(\d+):.+").Groups[1].Value)
                }
            }
            return answer;
        }
    }
}
