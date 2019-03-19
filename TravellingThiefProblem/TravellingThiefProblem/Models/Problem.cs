using System;
using System.Collections.Generic;
using System.Drawing;

namespace TravellingThiefProblem.Models
{
    /// <summary>
    /// ModelClass
    /// </summary>
    public class Problem
    {
        public string Name { get; set; }
        public string KnapsackDataType { get; set; }
        public int Dimension { get; set; }
        public int NumberOfItems { get; set; }
        public int KnapsackCapacity { get; set; }
        public double SpeedMin { get; set; }
        public double SpeedMax { get; set; }
        public double RentingRatio { get; set; }
        public string EdgeWeightType { get; set; }
        public List<City> Cities { get; set; }
        public List<Item> Items { get; set; }

        public int[,] Distances { get; set; }

        public void PrintDistanceMatrix()
        {
            for (int i = 0; i < Dimension; i++)
            {
                Console.Write($"\t{i+1}.");
            }
            Console.WriteLine();
            
            for (int i = 0; i < Dimension; i++)
            {
                Console.Write($"{i+1}.\t");
                for (int j = 0; j < Dimension; j++)
                {
                    Console.Write($"{Distances[i, j]}\t");
                }

                Console.WriteLine();
            }
        }
    }
}