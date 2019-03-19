using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TravellingThiefProblem.Utilities
{
    public class Logger
    {
        //<nr_pokolenia, najlepsza_ocena, średnia_ocen, najgorsza_ocena> 

        private int _populationSize;
        private int _generations;
        private double _px;        //0-100
        private double _pm;         //0-100
        private int _selectionType;
        private int _tour;
        private int _greedyAlgorithmType;

        // px od 0.5 do 1/       0.3, 1
        // pm 0.01 do 0.1       0,3, 0,7

        public List<string> Stats { get; private set; }

        public Logger(int populationSize, int generations, double px, double pm, int selectionType, int tour, int greedyAlgorithmType)
        {
            _populationSize = populationSize;
            _generations = generations;
            _px = px;
            _pm = pm;
            _selectionType = selectionType;
            _tour = tour;
            _greedyAlgorithmType = greedyAlgorithmType;
            Stats = new List<string>();
        }

        public Logger()
        {
            Stats = new List<string>();
        }

        //Generate filename based on date and source file
        private string GenerateFileName(string source)
        {
            var name = $"{DateTime.Now:yyyyMMddHHmmss}_{source.Substring(5, source.Length-9)}";

            return name;
        }

        // Adds values as a new element in list
        public void AddToLogger(double bestFitness, double averageFitness, double worstFitness, bool print)
        {
            Stats.Add($"{Stats.Count};{bestFitness.ToString(CultureInfo.CurrentCulture)};{averageFitness.ToString(CultureInfo.CurrentCulture)};{worstFitness.ToString(CultureInfo.CurrentCulture)}");
            if (print) Console.WriteLine($"{Stats.Count-1}, {bestFitness.ToString(CultureInfo.InvariantCulture)}, {averageFitness.ToString(CultureInfo.InvariantCulture)}, {worstFitness.ToString(CultureInfo.InvariantCulture)}");
        }

        
        // Saves list stats and config in files
        public void SaveToFile(string source, string time)
        {
            var filename = GenerateFileName(source);
            var path = $@"D:\Projekty\SIiIW\TravellingThiefProblem\TravellingThiefProblem\Data\Output\{filename}.csv";

            if (File.Exists(path)) return;
            File.Create(path).Dispose();

            using (TextWriter tw = new StreamWriter(path))
            {
                foreach (var stat in Stats)
                {
                    tw.WriteLine(stat);
                }
            }

            SaveConfigFile(filename, time);
        }

        //saves confing file
        private void SaveConfigFile(string source, string time)
        {
            var path = $@"D:\Projekty\SIiIW\TravellingThiefProblem\TravellingThiefProblem\Data\Output\{source}_config.txt";

            if (File.Exists(path)) return;
            File.Create(path).Dispose();

            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine($"pop\t{_populationSize}");
                tw.WriteLine($"gen\t{_generations}");
                tw.WriteLine($"px\t{_px}");
                tw.WriteLine($"pm\t{_pm}");
                tw.WriteLine($"sel\t{-_selectionType}");
                tw.WriteLine($"tour\t{_tour}");
                tw.WriteLine($"time\t{time}");
                tw.WriteLine($"sort\t{_greedyAlgorithmType}");
                tw.WriteLine($"score\t{source}");
            }
        }
    }
}