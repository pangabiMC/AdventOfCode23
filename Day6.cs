using System;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day6 : IDay
    {
        public long DoIt(string[] input)
        {
            //Time: 7  15   30
            //Distance: 9  40  200
            var time = long.Parse(input[0].Substring("Time: ".Length).Replace(" ", ""));
            var dist = long.Parse(input[1].Substring("Distance: ".Length).Replace(" ", ""));

            double t = time;
            double d = dist;

            double e = Math.Sqrt(t * t - 4 * d);
            double min = 0.5 * (t - e);
            double max = 0.5 * (t + e);
            int r = (int)Math.Ceiling(max) - (int)Math.Floor(min) - 1;
            return r;
        }



        public long DoIt_B(string[] input)
        {
            //Time: 7  15   30
            //Distance: 9  40  200
            var times = input[0].Substring("Time: ".Length).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var dists = input[1].Substring("Distance: ".Length).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            int answer = 1;
            for (int i = 0; i < times.Count; i++)
            {
                double t = times[i];
                double d = dists[i];

                double e = Math.Sqrt(t * t - 4*d);
                double min = 0.5 * (t - e);
                double max = 0.5 * (t + e);
                int r = (int)Math.Ceiling(max) - (int)Math.Floor(min) - 1;
                //System.Diagnostics.Debug.WriteLine(r);
                answer *= r;
            }

            return answer;
        }

    }

}
