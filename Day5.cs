using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day5 : IDay
    {
        public long DoIt(string[] input)
        {
            //seeds: 79 14 55 13

            //seed - to - soil map:
            //50 98 2
            //52 50 48

            List<Mapping> mappings = new List<Mapping>();
            List<long> seeds = input[0].Substring("seeds: ".Length).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            for (int i = 1; i < input.Length; i++)
            {
                string l = input[i];
                if (!String.IsNullOrWhiteSpace(l))
                {
                    if (!char.IsDigit(l[0]))
                    {
                        // new map
                        mappings.Add(new Mapping());
                    }
                    else
                    {
                        mappings[mappings.Count - 1].Ranges.Add(new Range(l));
                    }
                }
            }

            long min = long.MaxValue;
            long progress = 0;
            for(int i = 0; i < seeds.Count; i += 2)
            {
                long seedBase = seeds[i];
                long seedRange = seeds[i + 1];

                for (long seed = seedBase; seed < seedBase + seedRange; ++seed)
                {
                    long curr = seed;
                    foreach (var mapping in mappings)
                    {
                        curr = mapping.DoMap(curr);
                    }
                    min = Math.Min(curr, min);

                    if ((seed - seedBase) / seedRange > progress)
                    {
                        Console.WriteLine($"@@@@ {(seed - seedBase) / seedRange * 100}%");
                        progress = (seed - seedBase) / seedRange;
                    }

                }
                Console.WriteLine($"@@@ {i} / {seeds.Count} DONE");
            }


            return min;
        }

        public class Mapping
        {
            public List<Range> Ranges { get; } = new List<Range>();
            public long DoMap(long input)
            {
                long result = input;
                foreach(var range in Ranges) 
                {
                    var m = range.MatchRange(input);
                    if(m != input) 
                    {
                        result = m;
                        break;
                    }
                }

                return result;
            }
        }

        public class Range
        {
            public Range(string s)
            {
                var a = s.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToArray();
                Dest = a[0];
                Src = a[1];
                Length = a[2];
            }

            public long Src { get; set; }
            public long Dest { get; set; }
            public long Length { get; set; }

            public long MatchRange(long input)
            {
                long ret = input;
                if(input >= Src && input < Src + Length) 
                {
                    ret = Dest + input - Src;
                }

                return ret;
            }

        }

        public void DoIt_1(string[] input)
        {
            //seeds: 79 14 55 13

            //seed - to - soil map:
            //50 98 2
            //52 50 48

            List<Mapping> mappings = new List<Mapping>();
            List<long> seeds = input[0].Substring("seeds: ".Length).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            for (int i = 1; i < input.Length; i++)
            {
                string l = input[i];
                if (!String.IsNullOrWhiteSpace(l))
                {
                    if (!char.IsDigit(l[0]))
                    {
                        // new map
                        mappings.Add(new Mapping());
                    }
                    else
                    {
                        mappings[mappings.Count - 1].Ranges.Add(new Range(l));
                    }
                }
            }

            long min = long.MaxValue;

            foreach (var seed in seeds)
            {
                long curr = seed;
                foreach (var mapping in mappings)
                {
                    curr = mapping.DoMap(curr);
                    //System.Diagnostics.Debug.Write($" {curr} ");
                }
                min = Math.Min(curr, min);
                //System.Diagnostics.Debug.WriteLine(min);
            }


            System.Diagnostics.Debug.WriteLine(min);
        }

    }

}
