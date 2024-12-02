using System.Collections;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2023
{
    internal class Day12: IDay
    {
        List<int> _controls;
        int _controlSum => _controls.Sum();
        long _nextPatternCallCount = 0;
        long _totalCallCount = 0;
        static List<char> charMap = new List<char>(){ '.', '?', '#' };

        public long DoIt(string[] input)
        {
            long answer = 0;
            int lineCount = 0;
            foreach (string s in input)
            {
                Utils.LogDebug($"{s}");
                long rowAnser = 0;

                // TEST
                rowAnser = RunPermutations(s, 1);

                long factor = 0;
                //factor = LinearFactor(s);
                //if (factor != -1)
                //{
                //    rowAnser = RunPermutations(s, 1) * (long)Math.Pow(factor, 4);
                //}
                //else
                //{
                //    rowAnser = RunPermutations(s, 5);
                //}

                Utils.Log($"Line {++lineCount} | {s} | ANS: {rowAnser} | T: {Program._runningSw.Elapsed} | Call count: {_nextPatternCallCount} | {factor}");
                Utils.LogDebug("-------------------------------" + Environment.NewLine);

                answer += rowAnser;

                _totalCallCount += _nextPatternCallCount;
                _nextPatternCallCount = 0;
            }

            Utils.Log($"TOTAL Call Count: {_totalCallCount}");
            return answer;
        }

        private long LinearFactor(string s)
        {
            long b = RunPermutations(s, 1);
            long sec = RunPermutations(s, 2);
            long check = RunPermutations(s, 3);

            long factor = sec / b;
            if(sec * factor == check)
            {
                return factor;
            }
            return -1;
        }

        private long RunPermutations(string input, int repeat)
        {
            string pattern = string.Join("?", Enumerable.Repeat(input.Split(' ')[0], repeat));
            _controls = string.Join(",", Enumerable.Repeat(input.Split(' ')[1], repeat)).Split(',').Select(int.Parse).ToList();

            return CountMatches(new Pattern(pattern), 0, 0);
        }

        private long CountMatches(Pattern pattern, int controlIndex, int startIndex)
        {
            Utils.LogDebug($"{pattern} - {controlIndex} - i: {startIndex}");
            _nextPatternCallCount++;
            long matchCount = 0;
            int  i = startIndex; 

            while(i < pattern.Length)
            {
                if(pattern[i] != 0)
                {
                    if (TryPutControl(pattern, i, _controls[controlIndex], out Pattern next))
                    {
                        int nextStartIndex = i + _controls[controlIndex] + 1;
                        if (controlIndex + 1 >= _controls.Count)
                        {
                            if (nextStartIndex >= pattern.Length || Enumerable.Range(nextStartIndex, pattern.Length - nextStartIndex).All(j => !next._springList[j]))
                            {
                                if(!IsMatch(next))
                                {

                                }
                                Utils.LogDebug($"{next} - {controlIndex} - i: {startIndex} ***");
                                ++matchCount;
                            }
                        }
                        else
                        {
                            if (_controls[(controlIndex + 1)..].Sum() + _controls.Count - controlIndex - 2 <= pattern.Length - nextStartIndex)
                            {
                                matchCount += CountMatches(next, controlIndex + 1, nextStartIndex);
                            }
                            else
                            {

                            }
                        }
                    }
                    else
                    {
                        if (pattern[i] == 2)
                        {
                            // invalid pattern
                            return matchCount;
                        }

                        pattern[i] = 0;
                    }
                }
                ++i;
            }

            return matchCount;
        }

        private bool TryPutControl(Pattern pattern, int startIndex, int control, out Pattern next)
        {
            next = null;
            if (startIndex + control > pattern.Length) return false;
            if(startIndex > 0 && pattern[startIndex - 1] == 2) return false;


            for (int i = startIndex; i < startIndex + control; ++i)
            {
                if (pattern[i] == 0)
                {
                    return false;
                }
            }

            if (startIndex + control < pattern.Length && pattern[startIndex + control] == 2)
            {
                return false;
            }

            next = new Pattern(pattern);
            if (startIndex + control + 1 < pattern.Length)
            {

                next[startIndex + control] = 0;
            }

            for (int i = startIndex; i < startIndex + control; ++i)
            {
                next[i] = 2;
            }

            return true;
        }

        private bool IsMatch(Pattern p)
        {
            return p.ControlList().SequenceEqual(_controls);
        }

        public class Pattern
        {
            public BitArray _springList; // # == 2
            public BitArray _unknownList; // ? == 1

            public int Length => _springList?.Length ?? 0;

            public Pattern(string s)
            {
                _unknownList = new BitArray(s.Select(ch => ch == '?').ToArray());
                _springList = new BitArray(s.Select(ch => ch == '#').ToArray());
            }

            public Pattern(Pattern other)
            {
                _springList = (BitArray)other._springList.Clone();
                _unknownList = (BitArray)other._unknownList.Clone();
            }

            public int this[int i]
            {
                get { return i < Length ? _springList[i] ? 2 : _unknownList[i] ? 1 : 0 : 0; }
                set
                {
                    _springList[i] = false;
                    _unknownList[i] = false;
                    switch (value)
                    {
                        case 1:
                            _unknownList[i] = true;
                            break;
                        case 2:
                            _springList[i] = true;
                            break;
                    }
                }
            }

            public bool IsBlockAllSetTo(int start, int length, int value)
            {
                if(start + length > Length)
                {
                    return false;
                }
                for(int i = start; i < start + length; ++i)
                {
                    if (this[i] != value)
                        return false;
                }
                return true;
            }

            public IEnumerable<int> ControlList()
            {
                int lastBlock = 0;
                for(int i = 0; i < Length; ++i) 
                {
                    if (_springList[i])
                    {
                        lastBlock++;
                    }
                    else if(lastBlock != 0)
                    {
                        yield return lastBlock;
                        lastBlock = 0;
                    }
                }
                if (_springList[Length - 1])
                {
                    yield return lastBlock;
                }
            }

            public bool HasUnknown => _unknownList.HasAnySet();

            public override bool Equals(object? obj)
            {
                if(obj is Pattern)
                {
                    Pattern other = (Pattern)obj;
                    if(other.Length != Length) return false;
                    for (int i = 0; i < Length; ++i)
                    {
                        if (other._unknownList[i] != _unknownList[i] || other._springList[i] != _springList[i]) return false;
                    }

                    return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    hash = hash * 486187739 + GetHashCode(_springList);
                    hash = hash * 486187739 + GetHashCode(_unknownList);
                    return hash;
                }
            }

            public int GetHashCode(BitArray array)
            {
                UInt32 hash = 17;
                int bitsRemaining = array.Length;
                foreach (int value in array.GetInternalValues())
                {
                    UInt32 cleanValue = (UInt32)value;
                    if (bitsRemaining < 32)
                    {
                        //clear any bits that are beyond the end of the array
                        int bitsToWipe = 32 - bitsRemaining;
                        cleanValue <<= bitsToWipe;
                        cleanValue >>= bitsToWipe;
                    }

                    hash = hash * 23 + cleanValue;
                    bitsRemaining -= 32;
                }
                return (int)hash;
            }
            public override string ToString()
            {
                char[] ret = new char [Length];
                for(int i = 0; i < Length; i++) 
                {
                    if (_springList[i]) { ret[i] = '#'; }
                    else if (_unknownList[i]) { ret[i] = '?'; }
                    else { ret[i] = '.'; }
                }

                return new string(ret);
            }
        }
    }
}
