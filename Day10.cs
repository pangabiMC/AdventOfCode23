using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day10: IDay
    {
        // DIRECTION BITS:
        // North|South|West|East
        [Flags]
        public enum Directions : int
        {
            None = 0,
            East = 0b0001,
            West = 0b0010,
            South = 0b0100,
            North = 0b1000,
            Start = 0b1111
        }

        public static Dictionary<char, int> SYM = new Dictionary<char, int>()
        {
            {'|', (int)(Directions.North | Directions.South) },
            {'-', (int)(Directions.East  | Directions.West) },
            {'L', (int)(Directions.North | Directions.East) },
            {'J', (int)(Directions.North | Directions.West) },
            {'7', (int)(Directions.South | Directions.West) },
            {'F', (int)(Directions.South | Directions.East) },
            {'S', 0b1111 },
            {'.', 0b0000 },
        };

        private List<int> _map = new List<int>();
        private int _rowLength;
        private HashSet<int> _pipeIndices;

        public long DoIt(string[] input)
        {
            long answer = 0;

            _rowLength = input[0].Length;
            _map = input.SelectMany(s => s.Where(SYM.ContainsKey).Select(ch => SYM[ch])).ToList();

            _pipeIndices = FindPipeIndices();

            answer = Enumerable.Range(0, _map.Count - 1).Count(IsInside);

            // // DEBUG print of pipe only
            for (int i = 0; i < input.Length; ++i)
            {
                string row = String.Empty;
                for (int j = 0; j < input[0].Length; ++j)
                {
                    int index = i * input[0].Length + j;
                    if (_pipeIndices.Contains(index))
                    {
                        row += SYM.Where(kvp => kvp.Value == _map[index]).Select(kvp => kvp.Key).First();
                    }
                    else
                    {
                        row += IsInside(index) ? 'I' : '.';
                    }
                }
                Utils.Log(row);
            }


            return answer;
        }

        private bool IsInside(int index)
        {
            if (_pipeIndices.Contains(index))
            {
                return false;
            }
            
            index++;
            int cross = 0;
            Directions squeezing = Directions.None;
            for (; index % _rowLength != 0; ++index)
            {
                if (_pipeIndices.Contains(index))
                {
                    if (_map[index] == SYM['|'])
                    {
                        cross++;
                    }
                    else if (((Directions)_map[index]).HasFlag(Directions.North))
                    {
                        if (squeezing == Directions.None)
                        {
                            squeezing = Directions.North;
                        }
                        else if (squeezing == Directions.North)
                        {
                            squeezing = Directions.None;
                        }
                        else
                        {
                            cross++;
                            squeezing = Directions.None;
                        }
                    }
                    else if (((Directions)_map[index]).HasFlag(Directions.South))
                    {
                        if (squeezing == Directions.None)
                        {
                            squeezing = Directions.South;
                        }
                        else if (squeezing == Directions.South)
                        {
                            squeezing = Directions.None;
                        }
                        else
                        {
                            cross++;
                            squeezing = Directions.None;
                        }
                    }
                }

            }

            return cross % 2 == 1;
        }

        private HashSet<int> FindPipeIndices()
        {
            FindStartPipe(out int startIndex, out int startPipe);
            _map[startIndex] = startPipe;
            int step = startPipe & -startPipe; // rightmost 1
            HashSet<int> pipeIndices = new HashSet<int>() { startIndex };
            int currIndex = startIndex;
            do
            {
                // make step - where we are and where we came from
                // e.g. current: west-south:0011 and we entered from west:0010 ==> next step is (0011 & (~0010)) == 0001 (east)
                step = _map[currIndex] & ~step;

                switch ((Directions)step)
                {
                    case Directions.North:
                        currIndex -= _rowLength;
                        step = (int)Directions.South; // where we entered from for next step
                        break;
                    case Directions.South:
                        currIndex += _rowLength;
                        step = (int)Directions.North;
                        break;
                    case Directions.West:
                        currIndex -= 1;
                        step = (int)Directions.East;
                        break;
                    case Directions.East:
                        currIndex += 1;
                        step = (int)Directions.West;
                        break;
                    default:
                        throw new InvalidOperationException($"map invalid at {currIndex}");
                }
                pipeIndices.Add(currIndex);
            }
            while (currIndex != startIndex);

            return pipeIndices;
        }

        private void FindStartPipe(out int startIndex, out int startPipe)
        {
            startIndex = _map.IndexOf(SYM['S']);
            startPipe = 0;

            if ((startIndex + 1) % _rowLength != 0 && startIndex + 1 < _map.Count && (_map[startIndex + 1] & (int)Directions.West) != 0)
            {
                startPipe |= (int)Directions.East;
            }

            if ((startIndex - 1) % _rowLength != 0 && startIndex - 1 >= 0 && (_map[startIndex - 1] & (int)Directions.East) != 0)
            {
                startPipe |= (int)Directions.West;
            }

            if (startIndex - _rowLength >= 0 && (_map[startIndex - _rowLength] & (int)Directions.South) != 0)
            {
                startPipe |= (int)Directions.North;
            }

            if (startIndex + _rowLength < _map.Count && (_map[startIndex + _rowLength] & (int)Directions.North) != 0)
            {
                startPipe |= (int)Directions.South;
            }
        }
    }


    internal class Day10_A : IDay
    {
        //| is a vertical pipe connecting north and south.
        //- is a horizontal pipe connecting east and west.
        //L is a 90-degree bend connecting north and east.
        //J is a 90-degree bend connecting north and west.
        //7 is a 90-degree bend connecting south and west.
        //F is a 90-degree bend connecting south and east.
        //. is ground; there is no pipe in this tile.
        //S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.

        // DIRECTION BITS:
        // North|South|West|East
        [Flags]
        public enum Directions : byte
        {
            East = 0b0001,
            West = 0b0010,
            South = 0b0100,
            North = 0b1000,
            Start = 0b1111
        }

        public static Dictionary<char, byte> SYM = new Dictionary<char, byte>()
        {
            {'|', (byte)(Directions.North | Directions.South) },
            {'-', (byte)(Directions.East  | Directions.West) },
            {'L', (byte)(Directions.North | Directions.East) },
            {'J', (byte)(Directions.North | Directions.West) },
            {'7', (byte)(Directions.South | Directions.West) },
            {'F', (byte)(Directions.South | Directions.East) },
            {'S', 0b1111 },
            {'.', 0b0000 },
        };

        public long DoIt(string[] input)
        {
            long answer = 0;

            int rowLength = input[0].Length;
            List<byte> map = input.SelectMany(s => s.Where(SYM.ContainsKey).Select(ch => SYM[ch])).ToList();
            int currIndex = map.IndexOf(0b1111);
            byte step = 0;
            do
            {
                // make step - where we are and where we came from
                // e.g. current: west-south:0011 and we entered from west:0010 ==> next step is (0011 & (~0010)) == 0001 (east)
                step = answer == 0 ? FindStartStep(currIndex, map, rowLength) : (byte)(map[currIndex] & (byte)~step);

                switch ((Directions)step)
                {
                    case Directions.North:
                        currIndex -= rowLength;
                        step = (byte)Directions.South; // where we entered from for next step
                        break;
                    case Directions.South:
                        currIndex += rowLength;
                        step = (byte)Directions.North;
                        break;
                    case Directions.West:
                        currIndex -= 1;
                        step = (byte)Directions.East;
                        break;
                    case Directions.East:
                        currIndex += 1;
                        step = (byte)Directions.West;
                        break;
                    default:
                        throw new InvalidOperationException($"map invalid at {currIndex}");
                }

                answer++;
            }
            while (map[currIndex] != 0b1111);

            answer /= 2;
            return answer;
        }

        private byte FindStartStep(int currIndex, List<byte> map, int rowLength)
        {
            byte firststep = 0;

            if ((currIndex + 1) % rowLength != 0 && currIndex + 1 < map.Count && (map[currIndex + 1] & (byte)Directions.West) != 0)
            {
                firststep = (byte)Directions.East;
            }
            else if ((currIndex - 1) % rowLength != 0 && currIndex - 1 >= 0 && (map[currIndex - 1] & (byte)Directions.East) != 0)
            {
                firststep = (byte)Directions.West;
            }
            else if (currIndex - rowLength >= 0 && (map[currIndex - rowLength] & (byte)Directions.South) != 0)
            {
                firststep = (byte)Directions.North;
            }
            else if (currIndex + rowLength < map.Count && (map[currIndex + rowLength] & (byte)Directions.North) != 0)
            {
                firststep = (byte)Directions.South;
            }
            else
            {
                throw new InvalidOperationException($"invalid start");
            }

            return firststep;
        }
    }

}
