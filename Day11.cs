using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day11: IDay
    {
        public long DoIt(string[] input)
        {
            int expansionFactor = 1000000; // 2; -- Q1
            long answer = 0;
            int r = 0;
            List<int[]> map = new List<int[]>();
            
            List<int> emptyRows = new List<int>();
            List<int> emptyColumns = new List<int>();

            // gather galaxy coordinates
            foreach (var s in input)
            {
                bool found = false;
                for(int i = 0; i < s.Length; i++) 
                {
                    if (s[i] == '#')
                    {
                        found = true;
                        map.Add(new[] { r, i });
                    }
                }
                if (!found)
                {
                    emptyRows.Add(r);
                }
                r++;
            }

            // collect empty columns
            for(int i = 0; i < input[0].Length; ++i)
            {
                if(!map.Any(m => m[1] == i))
                {
                    emptyColumns.Add(i);
                }
            }

            // expand map with empty rows/columns
            foreach(var m in map)
            {
                m[0] += (expansionFactor - 1) * emptyRows.Count(e => e < m[0]);
                m[1] += (expansionFactor - 1) * emptyColumns.Count(e => e < m[1]);
            }

            // sum distances
            for (int i = 0; i < map.Count; ++i)
            {
                for (int j = i; j < map.Count; ++j)
                {
                    answer += Math.Abs(map[i][0] - map[j][0]) + Math.Abs(map[i][1] - map[j][1]);
                }
            }

            return answer;
        }
    }

}
