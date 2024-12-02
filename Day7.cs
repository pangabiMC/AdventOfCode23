using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day7 : IDay
    {
        
        public static List<char> CardValues = new List<char>() { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'};

        public class HandType : IComparable<HandType>
        {
            private List<int> _counts;
            public HandType(int[] cards) 
            {
                int[] v = Enumerable.Repeat(0, CardValues.Count).ToArray();
                int j = 0;
                foreach(var c in cards)
                {
                    if (c == 0)
                    {
                        j++;
                    }
                    else
                    { 
                        v[c]++;
                    }
                }

                _counts = v.Where(i => i != 0).OrderByDescending(i => i).ToList();
                if(_counts.Count ==0)
                {
                    _counts.Add(0);
                }
                _counts[0] = _counts[0] + j;
            }
            
            // 5, 41, 32, 311, 221, 2111, 11111
            // 4J, 31J, 22J, 211J, 1111J
            // 3JJ, 21JJ, 11JJJ
            // 2JJJJ, 1JJJJ,
            // JJJJJ
            public int CompareTo(HandType other)
            {
                if(other._counts.Count != _counts.Count)
                {
                    return - _counts.Count + other._counts.Count;
                }
                else
                {
                    return _counts[0] - other._counts[0];
                }
            }
        }

        public class Hand : IComparable<Hand>
        {
            private HandType _type;
            public Hand(string input)
            {
                var s = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Cards = s[0].Select(ch => CardValues.IndexOf(ch)).ToArray();
                Bid = int.Parse(s[1].Trim());
                _type = new HandType(Cards);
            }

            public int[] Cards { get; }
            public int Bid { get; }

            public int TotalValue => Cards?.Length > 0 ?  (int)Math.Pow(14,5) * Cards[0] + (int)Math.Pow(14, 4) * Cards[1] + (int)Math.Pow(14, 3) * Cards[2] + (int)Math.Pow(14, 2) * Cards[3] + (int)Math.Pow(14, 1) * Cards[4] : 0;

            public int CompareTo(Hand other)
            {
                if (_type.CompareTo(other._type) != 0)
                {
                    return _type.CompareTo(other._type);
                }
                else
                {
                    return TotalValue - other.TotalValue;
                }

                // if Type(this) > Type(other) => +1
                // else if < => -1
                // else TotalVAlue.Compare
                //KTJJT < KK677 --> 53443 < 55122

            }
        }

        public long DoIt(string[] input)
        {
            int answer = 0;

            var hands = input.Select(s => new Hand(s)).ToList();
            hands.Sort();
            for (int i = 0; i < hands.Count; ++i)
            {
                answer += (i + 1) * hands[i].Bid;
            }

            return answer;
        }

    }





    internal class Day7_A : IDay
    {

        public static List<char> CardValues = new List<char>() { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

        public class HandType : IComparable<HandType>
        {
            private List<int> _counts;
            public HandType(int[] cards)
            {
                int[] v = Enumerable.Repeat(0, CardValues.Count).ToArray();
                foreach (var c in cards)
                {
                    v[c]++;
                }

                _counts = v.Where(i => i != 0).OrderByDescending(i => i).ToList();

            }

            public int CompareTo(HandType other)
            {
                if (other._counts.Count != _counts.Count)
                {
                    return -_counts.Count + other._counts.Count;
                }
                else
                {
                    return _counts[0] - other._counts[0];
                }
            }
        }

        public class Hand : IComparable<Hand>
        {
            private HandType _type;
            public Hand(string input)
            {
                var s = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Cards = s[0].Select(ch => CardValues.IndexOf(ch)).ToArray();
                Bid = int.Parse(s[1].Trim());
                _type = new HandType(Cards);
            }

            public int[] Cards { get; }
            public int Bid { get; }

            public int TotalValue => Cards?.Length > 0 ? (int)Math.Pow(14, 5) * Cards[0] + (int)Math.Pow(14, 4) * Cards[1] + (int)Math.Pow(14, 3) * Cards[2] + (int)Math.Pow(14, 2) * Cards[3] + (int)Math.Pow(14, 1) * Cards[4] : 0;

            public int CompareTo(Hand other)
            {
                if (_type.CompareTo(other._type) != 0)
                {
                    return _type.CompareTo(other._type);
                }
                else
                {
                    return TotalValue - other.TotalValue;
                }

                // if Type(this) > Type(other) => +1
                // else if < => -1
                // else TotalVAlue.Compare
                //KTJJT < KK677 --> 53443 < 55122

            }
        }

        public long DoIt(string[] input)
        {
            int answer = 0;

            var hands = input.Select(s => new Hand(s)).ToList();
            hands.Sort();
            for (int i = 0; i < hands.Count; ++i)
            {
                answer += (i + 1) * hands[i].Bid;
            }

            return answer;
        }
    }
}
