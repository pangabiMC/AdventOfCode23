using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2023
{
    internal class Day15: IDay
    {
        public long DoIt(string[] input)
        {
            long answer = 0;
            foreach(var b in input[0].Split(','))
            {
                answer += HASH(Encoding.ASCII.GetBytes(b));
            }

            return answer;
        }

        private uint HASH(byte[] input)
        {
            uint ret = 0;

            //Determine the ASCII code for the current character of the string.
            //Increase the current value by the ASCII code you just determined.
            //Set the current value to itself multiplied by 17.
            //Set the current value to the remainder of dividing itself by 256.
            for(int i = 0; i < input.Length; i++) 
            {
                ret += input[i];
                ret *= 17;
                ret = ret << 24 >> 24;
            }
            return ret;
        }
    }

}
