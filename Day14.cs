namespace AdventOfCode2023
{
    internal class Day14: IDay
    {
        public long DoIt(string[] input)
        {
            long answer = 0;

            foreach(string s in Utils.TransposeInput(input))
            {
                answer += CountColumn(s);
            }

            return answer;
        }

        public int CountColumn(string col)
        {
            int count = 0;
            var sections = col.Split('#');
            int currSectionStartIndex = 0;
            for(int i = 0; i < sections.Length; ++i)
            {
                var s = sections[i];
                var c = s.Count(ch => ch == 'O');
                count += Enumerable.Range(col.Length - currSectionStartIndex - c + 1, c).Sum();
                currSectionStartIndex += s.Length + 1;
            }


            return count;
        }
    }

}
