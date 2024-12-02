using System.Collections;

namespace AdventOfCode2023
{
    internal class Day12asd: IDay
    {
        List<int> _controls;
        long _nextPatternCallCount = 0;
        long _totalCallCount = 0;
        static List<char> charMap = new List<char>(){ '.', '?', '#' };

        public long DoIt(string[] input)
        {
            long silverAnswer = 0;
            long goldAnswer = 0;
            int lineCount = 0;
            foreach (string s in input)
            {
                Utils.LogDebug($"{s}");
                long rowAnser = RunPermutations(s, 5);
                Utils.Log($"Line {++lineCount} | {s} | ANS: {rowAnser} | T: {Program._runningSw.Elapsed} | Call count: {_nextPatternCallCount}");
                Utils.LogDebug("-------------------------------" + Environment.NewLine);

                silverAnswer += rowAnser;

                _totalCallCount += _nextPatternCallCount;
                _nextPatternCallCount = 0;
            }

            Utils.Log($"TOTAL Call Count: {_totalCallCount}");
            Utils.Log($"SILVERANSWER: {silverAnswer}");
            return goldAnswer;
        }

        private long RunPermutations(string input, int repeat)
        {
            string pattern = string.Join("?", Enumerable.Repeat(input.Split(' ')[0], repeat));
            _controls = string.Join(",", Enumerable.Repeat(input.Split(' ')[1], repeat)).Split(',').Select(int.Parse).ToList();

            var workSet = new List<Tuple<Pattern, int>> { new Tuple<Pattern, int>(new Pattern(pattern), 0) };
            HashSet<Pattern> matches = new HashSet<Pattern>();
            for (int c = 0; c < _controls.Count; ++c)
            {
                List<Tuple<Pattern, int>> nextPatterns = new List<Tuple<Pattern, int>>();
                foreach(var p in workSet)
                {
                    foreach(var m in NextPatterns(new Pattern(p.Item1), c, p.Item2))
                    {
                        if(IsMatch(m))
                        {
                            m._unknownList.SetAll(false);
                            Utils.LogDebug($"{m} *");
                            bool b = matches.Add(m);
                            if (!b) Utils.LogDebug($"{m} ***********");
                        }
                        else if(m.HasUnknown)
                        {
                            int part = DoPartialCheck(m, c);
                            if (part != -1)
                            {
                                nextPatterns.Add(new Tuple<Pattern, int>(m, part));
                            }
                        }
                    }
                }

                workSet = nextPatterns;

                Utils.LogDebug($"A{c} - {workSet.Count}");
                if(workSet.Count == 0)
                {
                    break;
                }

                foreach(var m in workSet) Utils.LogDebug($"{m} N");
            }

            return matches.Count;
        }

        private bool IsMatch(Pattern p)
        {
            Utils.LogDebug($"{p} W");
            return p.ControlList().SequenceEqual(_controls);
        }

        private int DoPartialCheck(Pattern pattern, int controlCount)
        {
            int c = 0;
            int i = 0;
            int lastBlock = 0;
            while(c <= controlCount && i < pattern.Length)
            {
                if (pattern[i] == 0 || i + 1 == pattern.Length)
                {
                    if (lastBlock != 0)
                    {
                        if (_controls[c] == lastBlock)
                        {
                            lastBlock = 0;
                            c++;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
                else if (pattern[i] == 1)
                {
                    return i;
                }
                else if (pattern[i] == 2)
                {
                    lastBlock++;
                }

                i++;
            }

            return i;
        }

        IEnumerable<Pattern> NextPatterns(Pattern pattern, int c, int startIndex)
        {
            _nextPatternCallCount++;
            int controlCount = _controls[c];
            int lastBlock = 0;
            for (lastBlock = 0; lastBlock < startIndex && pattern._springList[startIndex - lastBlock - 1]; lastBlock++) ;
            
            for (int i = startIndex; i < pattern.Length; i++)
            {
                if (pattern[i] == 1) // ?
                {
                    int blockEnd = i - lastBlock + controlCount;
                    if (blockEnd > pattern.Length)
                    {
                        break;
                    }

                    if((blockEnd == pattern.Length || pattern[blockEnd] != 2) && pattern.IsBlockAllSet(i - lastBlock, controlCount))
                    {
                        Pattern next = new Pattern(pattern);
                        for (int k = 0; k < controlCount - lastBlock; ++k)
                        {
                            next[i + k] = 2;
                        }

                        if (blockEnd < pattern.Length)
                        {
                            next[i + controlCount - lastBlock] = 0;
                        }

                        yield return next;
                    }

                    if (lastBlock == 0)
                    {
                        pattern[i] = 0;
                    }
                    else
                    {
                        // invalid pattern
                        break;
                    }

                }
                else if (pattern[i] == 2) // #
                {
                    lastBlock++;
                }
                else
                {
                    lastBlock = 0;
                }
            }

            if (lastBlock == controlCount)
            {
                yield return new Pattern(pattern);
            }
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

            public bool IsBlockAllSet(int start, int length)
            {
                if(start + length > Length)
                {
                    return false;
                }
                for(int i = start; i < start + length; ++i)
                {
                    if (!(_springList[i] || _unknownList[i]))
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
