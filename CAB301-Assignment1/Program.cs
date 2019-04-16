using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Engines;
using System.Linq;
using System.Collections.Generic;


namespace CAB301_Assignment1
{
    /// <summary>
    /// The part of the assignment that builds, 
    /// compiles and runs the code and algorithm
    /// </summary>
    class Program
    {
        /// <summary>
        /// Runs the benchmark and subsequent tests
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RunRandomLists>(new Config());
            
            Console.ReadLine();
        }
    }

    /// <summary>
    /// This class overides the config to enable R Lang support, 
    /// graphing, and exporting of documents
    /// </summary>
    class Config : ManualConfig
    {
        public Config()
        {
            Add(CsvMeasurementsExporter.Default);
            Add(RPlotExporter.Default);
            Add(DefaultConfig.Instance.GetExporters().ToArray());
            Add(DefaultConfig.Instance.GetLoggers().ToArray());
            Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
        }
    }

    /// <summary>
    /// A class that runs the algorithm, setup and testing functions for the testing 
    /// of the Brute Force Median algorithm
    /// </summary>
    [MinColumn, MaxColumn, HtmlExporter, CsvMeasurementsExporter, RPlotExporter]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 0, targetCount: 5)]
    public class RunRandomLists
    {
        #region Functional Testing


        ///// <summary>
        ///// Function to check the correct application and 
        ///// results of the Brute Force Median Algorithm
        ///// </summary>
        //public void FunctionalTesting()
        //{
        //    List<int[]> testing_array = new List<int[]>
        //    {
        //        new int[] { 9, 2, 1, 4, 10, 5 },
        //        new int[] { 1, 2, 3, 4, 5, 6 },
        //        new int[] { 6, 5, 4, 3, 2, 1 },
        //        new int[] { 9, 9, 9, 9, 9, 9 },
        //        new int[] { 1, 1, 1, 1, 1, 1 },
        //        new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 },
        //        new int[] { 1, 3, 5, 7, 9 },
        //        new int[] { 2, 4, 6, 8, 10 },
        //        new int[] { 1, 1, 1, 5, 9, 9, 9 },
        //        new int[] { 5, 1, 1, 1, 9, 9, 9 },
        //        new int[] { 1, 2, 3, 4, 6, 7, 5 },
        //        new int[] { 1, 232, 3, 40, 5, 6, 74, 7, 100},
        //        new int[] { 1 }
        //    };


        //    int i = 0;


        //    // Loops through each of the above arrays to test and
        //    // output the results of the algorithm
        //    foreach (int[] test in testing_array)
        //    {
        //        i++;
        //        int result = BruteForceMedian(test);
        //        var stringOutput = string.Join(", ", test);
        //        Console.WriteLine("Array Contents: " + stringOutput);
        //        Console.WriteLine("Output: " + result);
        //        Console.WriteLine();
        //    }
        //}


        //public RunRandomLists()
        //{
        //    FunctionalTesting();
        //}

        #endregion

        #region Algorithm Testing


        /// <summary>
        /// This method reads a CSV of numbers into an array
        /// </summary>
        /// <param name="path"> a file path to a particular dataset </param>
        /// <returns> an array of integers </returns>
        private static int[] ReadCSV(string path)
        {
            string[] stringData = File.ReadAllLines(path);
            int[] data = Array.ConvertAll(stringData, int.Parse);
            return data;
        }


        /// <summary>
        /// The enumerable of type int array so the 
        /// application can loop through and test each of the arrays
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int []> Data()
        { 
            yield return ReadCSV(test_data[0]);
            yield return ReadCSV(test_data[1]);
            yield return ReadCSV(test_data[2]);
            yield return ReadCSV(test_data[3]);
            yield return ReadCSV(test_data[4]);
            yield return ReadCSV(test_data[5]);
            yield return ReadCSV(test_data[6]);
            yield return ReadCSV(test_data[7]);
            yield return ReadCSV(test_data[8]);
            yield return ReadCSV(test_data[9]);
            yield return ReadCSV(test_data[10]);
            yield return ReadCSV(test_data[11]);
        }


        /// <summary>
        /// An array of strings that locate the integer arrays in CSV files
        /// </summary>
        public string[] test_data = new string[]
        {
            "C:\\Users\\seano\\Desktop\\Test_Data\\1kasc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\1kdesc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\1krand.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\5kasc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\5kdesc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\5krand.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\10kasc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\10kdesc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\10krand.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\15kasc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\15kdesc.csv",
            "C:\\Users\\seano\\Desktop\\Test_Data\\15krand.csv"
        };


        /// <summary>
        /// The algorithm that finds the median of an array of integers.
        /// This is the function that is tested for the adjourning assignment.
        /// </summary>
        /// <param name="array"></param>
        /// <returns> the median value or a value of -1 if there is no correct answer </returns>
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public int BruteForceMedian(int[] array)
        {
            int OpCounter = 0;
            int k = (int)Math.Ceiling((decimal)array.Length/(decimal)2);
            for (int i = 0; i < array.Length-1; i++)
            {
                int numsmaller = 0, numequal = 0;

                for (int j = 0; j < array.Length-1; j++)
                {
                    if (array[j] < array[i])
                    { numsmaller = numsmaller + 1; }
                    else if (array[j] == array[i])
                    { numequal = numequal + 1; }
                    OpCounter++;
                }
                if (numsmaller < k && k <= (numsmaller + numequal))
                {
                    Console.WriteLine("Operation Counter: " + OpCounter);
                    return array[i];
                }
            }
            Console.WriteLine("Operation Counter: " + OpCounter);
            return -1;
        }


        #endregion
    }
}
