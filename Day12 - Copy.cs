using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class Day12A : IDay
    {
        string _pattern;
        List<int> _controls;
        Regex _fullCheck;
        long _nextPatternCallCount = 0;
        long _totalCallCount = 0;
        public long DoIt(string[] input)
        {
            long silverAnswer = 0;
            long goldAnswer = 0;
            foreach (string s in input)
            {
                Utils.LogDebug($"{s}");
                long start = RunPermutations(s, 1);
                Utils.Log($"ANS: {start}");

                silverAnswer += start;

                Utils.Log($"Call count: {_nextPatternCallCount}");
                _totalCallCount += _nextPatternCallCount;
                _nextPatternCallCount = 0;
            }

            Utils.Log($"TOTAL Call Count: {_totalCallCount}");
            Utils.Log($"SILVERANSWER: {silverAnswer}");
            return goldAnswer;
        }

        private long RunPermutations(string input, int repeat)
        {
            _pattern = string.Join("?", Enumerable.Repeat(input.Split(' ')[0], repeat));
            _controls = string.Join(",", Enumerable.Repeat(input.Split(' ')[1], repeat)).Split(',').Select(int.Parse).ToList();
            _fullCheck = new Regex($"^\\.*{String.Join("\\.+", _controls.Select(c => $"#{{{c}}}"))}[\\?|\\.]*$");

            var workSet = new List<Tuple<string, int>> { new Tuple<string, int>(_pattern, 0) };
            HashSet<string> matches = new HashSet<string>();
            for (int c = 0; c < _controls.Count; ++c)
            {
                List<string> nextPatterns = workSet.Where(p => p.Item1.Contains('?')).SelectMany(p => NextPatterns(p.Item1, c, p.Item2)).ToList();
                Utils.LogDebug($"B{c} - {nextPatterns.Count}");
                //Utils.LogDebug(String.Join(Environment.NewLine, nextPatterns.Select(p => _fullCheck.Match(p).Success ? p + " *" : p)));
                var thisRunMatches = nextPatterns.Where(p => _fullCheck.Match(p).Success).ToList();
                foreach (var m in thisRunMatches)
                {
                    matches.Add(m.Replace('?', '.'));
                }

                workSet = nextPatterns.Except(thisRunMatches).Select(p => new Tuple<string, int>(p, DoPartialCheck(p, c))).Where(t => t.Item2 != -1).ToList();

                Utils.LogDebug($"A{c} - {workSet.Count}{Environment.NewLine}");
            }

            return matches.Count;
        }

        private int DoPartialCheck(string pattern, int controlCount)
        {
            int c = 0;
            int i = 0;
            int lastBlock = 0;
            while (c <= controlCount && i < pattern.Length)
            {
                if (pattern[i] == '.' || i + 1 == pattern.Length)
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
                else if (pattern[i] == '?')
                {
                    return i;
                }
                else if (pattern[i] == '#')
                {
                    lastBlock++;
                }

                i++;
            }

            return i;
        }

        IEnumerable<string> NextPatterns(string patternStr, int c, int startIndex)
        {
            _nextPatternCallCount++;
            //Utils.LogDebug(patternStr);
            int controlCount = _controls[c];
            char[] pattern = patternStr.ToCharArray();
            int lastBlock = 0;
            for (int i = startIndex; i < pattern.Length; i++)
            {
                if (pattern[i] == '?')
                {
                    long blockEnd = i - lastBlock + controlCount;
                    if (blockEnd > pattern.Length)
                    {
                        break;
                    }

                    if (pattern.Skip(i - lastBlock).Take(controlCount).All(ch => ch == '#' || ch == '?') && (blockEnd == pattern.Length || pattern[blockEnd] == '?' || pattern[blockEnd] == '.'))
                    {
                        var parr = pattern.ToArray();
                        for (long k = 0; k < controlCount - lastBlock; ++k)
                        {
                            parr[i + k] = '#';
                        }

                        if (blockEnd < pattern.Length)
                        {
                            parr[i + controlCount - lastBlock] = '.';
                        }

                        yield return new string(parr);
                    }

                    if (lastBlock == 0)
                    {
                        pattern[i] = '.';
                    }
                    else
                    {
                        // invalid pattern
                        break;
                    }

                }
                else if (pattern[i] == '#')
                {
                    lastBlock++;
                }
                else
                {
                    if (lastBlock == controlCount)
                    {
                        yield return new string(pattern);
                    }
                    lastBlock = 0;
                }
            }

            if (lastBlock == controlCount)
            {
                yield return new string(pattern);
            }
        }
    }
}
