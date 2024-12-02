using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class Day8 : IDay
    {
        public class Node
        {
            public Node(string id)
            {
                Id = id;
            }

            public string Id { get; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        public long DoIt(string[] input)
        {
            long answer = 0;
            List<Node> currList = new List<Node>();

            Dictionary<string, Node> map = new Dictionary<string, Node>();
            foreach (var s in input.Skip(2))
            {
                var id = s.Split('=')[0].Trim();
                map[id] = new Node(id);
                if (id[2] == 'A')
                {
                    currList.Add(map[id]);
                }
            }

            foreach (var s in input.Skip(2))
            {
                var m = Regex.Match(s, @"([0-9|A-Z]+)\s*=\s*\(([0-9|A-Z]+)\s*,\s*([0-9|A-Z]+)");
                var id = m.Groups[1].Value;
                var left = m.Groups[2].Value;
                var right = m.Groups[3].Value;
                map[id].Left = map[left];
                map[id].Right = map[right];
            }

            bool[] route = input[0].Select(ch => ch == 'L').ToArray(); // true is left
            int ip = 0;

            List<long> loops = new List<long>();

            for(int c = 0; c < currList.Count; ++c)
            {
                var curr = currList[c];
                answer = 0;
                ip = 0;

                while (true)
                {
                    if (route[ip])
                    {
                        curr = curr.Left;
                    }
                    else
                    {
                        curr = curr.Right;
                    }

                    if (curr.Id[2] == 'Z')
                    {
                        loops.Add(answer+1);
                        break;
                    }
                    else
                    {
                        answer++;
                    }

                    ip = ip + 1 < route.Length ? ip + 1 : 0;
                }
            }

            answer = lcmx(loops);

            return answer;
        }

        static long lcmx(IEnumerable<long> numbers)
        {
            return numbers.Aggregate(lcm);
        }
        static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }



    internal class Day8_A: IDay
    {

        public class Node
        {
            public Node(string id) 
            {
                Id = id;
            }

            public string Id { get; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        public long DoIt(string[] input)
        {
            int answer = 0;
            Dictionary<string, Node> map = new Dictionary<string, Node>();
            foreach (var s in input.Skip(2))
            {
                var id = s.Split('=')[0].Trim();
                map[id] = new Node(id);
            }

            foreach (var s in input.Skip(2))
            {
                var m = Regex.Match(s, @"([A-Z]+)\s*=\s*\(([A-Z]+)\s*,\s*([A-Z]+)");
                var id = m.Groups[1].Value;
                var left = m.Groups[2].Value;
                var right = m.Groups[3].Value;
                map[id].Left = map[left];
                map[id].Right = map[right];
            }

            bool[] route = input[0].Select(ch => ch == 'L').ToArray(); // true is left
            int ip = 0;
            Node curr = map["AAA"];
            while(curr?.Id != "ZZZ")
            {
                if (route[ip])
                {
                    curr = curr.Left;
                }
                else
                {
                    curr = curr.Right;
                }

                ip = ip + 1 < route.Length ? ip +  1 : 0;
                answer++;
            }

            return answer;
        }
    }

}
