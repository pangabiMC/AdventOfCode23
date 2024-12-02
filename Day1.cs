using System.Linq;

namespace AdventOfCode2023
{
    internal class Day1 : IDay
    {
        public long DoIt(string[] input)
        {
            var digistrings = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }.ToList();
            int answer = 0;
            foreach (var l in input)
            {
                int firstNum = -1;
                int lastNum = -1;

                for (int i = 0; i < l.Length; i++)
                {
                    var spelled = digistrings.FirstOrDefault(d => l.Substring(i).StartsWith(d));
                    int numberFound = -1;
                    if (spelled != null)
                    {
                        numberFound = digistrings.IndexOf(spelled) + 1;
                    }
                    else if (char.IsDigit(l[i]))
                    {
                        numberFound = int.Parse(l[i].ToString());
                    }

                    if (numberFound != -1)
                    {
                        if (firstNum == -1)
                        {
                            firstNum = numberFound;
                        }

                        lastNum = numberFound;
                    }
                }
                if (firstNum != -1 && lastNum != -1)
                {
                    //Console.WriteLine($"{l} : {firstNum * 10 + lastNum}");
                    answer += firstNum * 10 + lastNum;
                }
            }

            return answer;
        }
    }
}
