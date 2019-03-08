using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TravellingThiefProblem.Utilities
{
    public class Logger
    {
        //<nr_pokolenia, najlepsza_ocena, średnia_ocen, najgorsza_ocena> 


        // px od 0.5 do 1/       0.3, 1
        // pm 0.01 do 0.1       0,3, 0,7

        public List<string> stats { get; set; }

        public Logger()
        {
            stats = new List<string>();
        }

        public string GenerateFileName(string source)
        {
            var name = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{source.Substring(5, source.Length-9)}";

            return name;
        }

        public void AddToLogger(int generationNumber, double bestFitness, double averageFitness, double worstFitness)
        {
            stats.Add($"{generationNumber}, {bestFitness.ToString(CultureInfo.InvariantCulture)}, {averageFitness.ToString(CultureInfo.InvariantCulture)}, {worstFitness.ToString(CultureInfo.InvariantCulture)}");
        }

        public void SaveToFile(string source)
        {
            string path = $@"D:\Projekty\SIiIW\TravellingThiefProblem\TravellingThiefProblem\Data\Output\{GenerateFileName(source)}.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();

                using (TextWriter tw = new StreamWriter(path))
                {
                    foreach (var stat in stats)
                    {
                        tw.WriteLine(stat);
                    }
                }

            }
        }
    }
}