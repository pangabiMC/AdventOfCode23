using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode2023
{
    internal class Day13: IDay
    {
        bool isGold = true;
        public long DoIt(string[] input)
        {
            return string.Join(Environment.NewLine, input)
                    .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                    .Select(table => table.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                    .Sum(t => 100 * CalculateTable(t) + CalculateTable(Utils.TransposeInput(t)));
        }

        private long CalculateTable(string[] currRows)
        {
            var compressed = currRows.Select(row => Convert.ToUInt64($"{row.Replace('#', '1').Replace('.', '0')}", 2)).ToArray(); // convert to binary array
            Utils.Log(String.Join(",", compressed));

            for (int i = 1; i < compressed.Length; i++)
            {
                int maxLength = Math.Min(i, compressed.Length - i);
                var comparer = new MirrorComparer() {  _hasDesmudged = !isGold };
                if (compressed.Take(i).Reverse().Take(maxLength).SequenceEqual(compressed.Skip(i).Take(maxLength), comparer) && comparer._hasDesmudged)
                {
                    return i;
                }
            }

            return 0;
        }

        private class MirrorComparer : IEqualityComparer<ulong>
        {
            public bool _hasDesmudged = false;
            public bool Equals(ulong x, ulong y)
            {
                if(x == y) return true;
                if(! _hasDesmudged && differAtOneBitPos(x, y))
                {
                    _hasDesmudged = true;
                    return true;
                }
                return false;
            }

            public int GetHashCode([DisallowNull] ulong obj)
            {
                return obj.GetHashCode();
            }

            static bool isPowerOfTwo(ulong x)
            {
                return x != 0 && ((x & (x - 1)) == 0);
            }

            static bool differAtOneBitPos(ulong a, ulong b)
            {
                return isPowerOfTwo(a ^ b);
            }
        }
    }

}
