using System;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day9: IDay
    {
        public long DoIt(string[] input)
        {
            long answer = 0;

            foreach (string s in input)
            {
                long[] nums = s.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).Reverse().ToArray();

                for (int j = 0; j < nums.Length; j++)
                {
                    for (int i = 0; i < nums.Length - 1 - j; i++)
                    {
                        nums[i] = nums[i] - nums[i + 1];
                    }
                }
                answer += nums.Reverse().Where((x, i) => i % 2 == 0).Sum() - nums.Reverse().Where((x, i) => i % 2 == 1).Sum();
            }

            return answer;
        }
    }



    internal class Day9_A : IDay
    {
        public long DoIt(string[] input)
        {
            long answer = 0;

            foreach (string s in input)
            {
                long[] nums = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

                for (int j = 0; j < nums.Length; j++)
                {
                    for (int i = 0; i < nums.Length - 1 - j; i++)
                    {
                        nums[i] = nums[i + 1] - nums[i];
                    }
                }

                answer += nums.Sum();
            }


            return answer;
        }
    }
}
