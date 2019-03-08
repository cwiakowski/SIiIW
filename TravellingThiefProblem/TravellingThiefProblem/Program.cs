using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Services;
using TravellingThiefProblem.Utilities;

namespace TravellingThiefProblem
{
    class Program
    {
        static void Main(string[] args)
        {

            DataReader.GenerateFilePaths();
            //DataReader.PrintFilePaths();
//            var list = new List<City>
//            {
//                new City(1, 0.0, 0.0),
//                new City(2, 10.0, 0.0),
//                new City(3, 10.0, 10.0),
//                new City(4, 0.0, 10.0)
//            };

            var factory = new ProblemProblemFactory();
            var problem = factory.Generate(DataReader.FilePaths[DataReader.FilePaths.Count-1]);


            var thief = new Thief(problem);
            thief.GenerateRandomPath(problem.Cities);
            
            for (int i = 0; i < 10; i++)
            {
                foreach (var city in thief.Path)
                {
                    Console.Write($"{city}\t");
                }

                Console.WriteLine(ThiefService.CalculatePathLength(thief.Path, problem.Cities));
                //Console.WriteLine();
                PathOperations.MutatePath(thief.Path);
            }
            

            //Console.WriteLine(ThiefService.CalculatePathLength(thief.Path));
            Console.Read();
        }
    }
}
