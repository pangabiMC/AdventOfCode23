using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AdventOfCode2023
{
    public interface IDay
    {
        long DoIt(string[] input);
    }

    internal class Program
    {
        public static System.Diagnostics.Stopwatch _runningSw = new System.Diagnostics.Stopwatch();
        static void Main(string[] args)
        {
            int day = 15;

            string[] input = File.ReadAllLines($@"..\..\..\input{day}.txt");
            IDay d = Activator.CreateInstance(System.Reflection.Assembly.GetExecutingAssembly().FullName, $"AdventOfCode2023.Day{day}").Unwrap() as IDay;

            _runningSw.Start();

            long answer = d.DoIt(input);

            _runningSw.Stop();

            Utils.Log($"SOLUTION:");
            Utils.Log(answer.ToString());
            Utils.Log($"Execution Time: {_runningSw.Elapsed.ToString()}");
        }
    }

    public static class Utils
    {
        public static string[] TransposeInput(string[] currRows)
        {
            string[] result = new string[currRows[0].Length];
            for (int i = 0; i < currRows[0].Length; ++i)
            {
                result[i] = new string(currRows.Select(s => s[i]).ToArray());
            }
            return result;
        }

        public static void Log(string s)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(s);
#else
            Console.WriteLine(s);

#endif

        }

        [Conditional("DEBUG")]
        public static void LogDebug(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
    }

    public static class BitArrayExtensions
    {
        static FieldInfo _internalArrayGetter = GetInternalArrayGetter();

        static FieldInfo GetInternalArrayGetter()
        {
            return typeof(BitArray).GetField("m_array", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        static int[] GetInternalArray(BitArray array)
        {
            return (int[])_internalArrayGetter.GetValue(array);
        }

        public static IEnumerable<int> GetInternalValues(this BitArray array)
        {
            return GetInternalArray(array);
        }
    }
}
