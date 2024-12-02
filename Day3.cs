using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day3 : IDay
    {
        private string _prevLine;
        private string _currLine;
        private string _nextLine;
        private Dictionary<string, List<int>> _gears = new Dictionary<string, List<int>>();

        public long DoIt(string[] input)
        {
            _currLine = new string('.', input[0].Length + 2);
            _nextLine = $".{input[0]}.";
            int partSum = 0;
            for (int rowCounter = 0; rowCounter < input.Length + 1; ++rowCounter)
            {
                _prevLine = _currLine;
                _currLine = _nextLine;
                _nextLine = rowCounter + 1 < input.Length ? $".{input[rowCounter + 1]}." : new string('.', input[0].Length + 2);

                string currNumber = String.Empty;

                for (int charCounter = 0; charCounter < _currLine.Length; ++charCounter)
                {
                    if (char.IsDigit(_currLine[charCounter]))
                    {
                        currNumber += _currLine[charCounter];
                    }
                    else if (currNumber != String.Empty)
                    {
                        // finalise number
                          // IsPartNo
                          // HasGear

                        if (IsPartNo(charCounter - currNumber.Length, currNumber.Length))
                        {
                            // add to gears
                            AddToGears(rowCounter, charCounter - currNumber.Length, currNumber);
                            partSum += int.Parse(currNumber);
                        }

                        currNumber = string.Empty;
                    }
                }
            }

            int answer = 0;


            foreach (var g in _gears.Where(g=>g.Value.Count == 2))
            {
                answer += g.Value[0] * g.Value[1];
            }
            System.Diagnostics.Debug.WriteLine(partSum);
            return answer;
            //DumpGears();
        }

        private bool IsPartNo(int index, int length)
        {
            return _prevLine.Substring(index - 1, length + 2).Any(IsSymbol) ||
                IsSymbol(_currLine[index - 1]) || IsSymbol(_currLine[index + length]) ||
                _nextLine.Substring(index - 1, length + 2).Any(IsSymbol);
        }

        private void DumpGears()
        {
            foreach (var g in _gears.OrderBy(kvp => kvp.Key))
            {
                System.Diagnostics.Debug.WriteLine($"{g.Key} {String.Join(" ", g.Value)}");
            }
        }

        private void AddToGears(int row, int col, string partNo)
        {
            string s = _prevLine.Substring(col - 1, partNo.Length + 2);
            for (int i = 0; i < s.Length; ++i)
            {
                if (IsGearSymbol(s[i]))
                {
                    var id = CreateGearID(row - 1, i + col - 1);
                    if (!_gears.ContainsKey(id))
                    {
                        _gears[id] = new List<int>();
                    }
                    _gears[id].Add(int.Parse(partNo));
                }
            }

            if (IsGearSymbol(_currLine[col - 1]))
            {
                var id = CreateGearID(row, col - 1);
                if (!_gears.ContainsKey(id))
                {
                    _gears[id] = new List<int>();
                }
                _gears[id].Add(int.Parse(partNo));
            }

            if (IsGearSymbol(_currLine[col + partNo.Length]))
            {
                var id = CreateGearID(row, col + partNo.Length);
                if (!_gears.ContainsKey(id))
                {
                    _gears[id] = new List<int>();
                }
                _gears[id].Add(int.Parse(partNo));
            }

            s = _nextLine.Substring(col - 1, partNo.Length + 2);

            for (int i = 0; i < s.Length; ++i)
            {
                if (IsGearSymbol(s[i]))
                {
                    var id = CreateGearID(row + 1, i + col - 1);
                    if (!_gears.ContainsKey(id))
                    {
                        _gears[id] = new List<int>();
                    }
                    _gears[id].Add(int.Parse(partNo));
                }
            }
        }

        private bool IsValidDigit(int index, List<int> prev, List<int> curr, List<int> next) 
        {
            return prev.Contains(index - 1) || prev.Contains(index) || prev.Contains(index + 1) ||
                curr.Contains(index - 1) || curr.Contains(index + 1) ||
                next.Contains(index - 1) || next.Contains(index) || next.Contains(index + 1);
        }

        public List<int> GatherSymbolPositions(string row)
        {
            var r = new List<int>();
            for (int i = 0; i < row.Length; ++i)
            {
                if (IsSymbol(row[i]))
                {
                    r.Add(i);
                }
            }

            return r;
        }

        public bool IsSymbol(char c)
        {
            return !char.IsLetterOrDigit(c) && !char.IsControl(c) && c != '.';
        }

        public bool IsGearSymbol(char c)
        {
            return c == '*';
        }

        string CreateGearID(int row, int column)
        {
            return $"{row}:{column}";
        }



        public long DoIt_1(string[] input)
        {
            int answer = 0;

            List<int> prevSym;
            List<int> currSym = new List<int>();
            List<int> nextSym = GatherSymbolPositions(input[0]);

            for (int rowCounter = 0; rowCounter < input.Length; ++rowCounter)
            {
                prevSym = currSym;
                currSym = nextSym;
                nextSym = rowCounter + 1 < input.Length ? GatherSymbolPositions(input[rowCounter + 1]) : new List<int>();

                string currRow = input[rowCounter];

                string currNumber = String.Empty;
                bool isValid = false;
                for (int charCounter = 0; charCounter < currRow.Length; ++charCounter)
                {
                    // curr character is NOT digit
                    // we had a number -> close it (convert and add to sum or discard)
                    // else -> skip
                    // else
                    // already valid -> add number to string
                    // else -> check isvalid
                    if (char.IsDigit(currRow[charCounter]))
                    {
                        isValid |= IsValidDigit(charCounter, prevSym, currSym, nextSym);
                        currNumber += currRow[charCounter];
                    }
                    else if (currNumber != String.Empty)
                    {
                        //   System.Diagnostics.Debug.WriteLine($"{int.Parse(currNumber)} - {isValid}");
                        if (isValid)
                        {
                            answer += int.Parse(currNumber);
                        }

                        currNumber = string.Empty;
                        isValid = false;
                    }
                }
                if (isValid)
                {
                    answer += int.Parse(currNumber);
                }
            }


            return answer;
        }
    }
}
