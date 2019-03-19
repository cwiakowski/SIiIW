using System;
using System.Linq;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Services.Interfaces;

namespace TravellingThiefProblem.Services
{
    public class CityService
    {
        /// <summary>
        /// Returns Calculated Distance between two cities
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int CalculateDistance(City a, City b)
        {
            var c = Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
            return (int)Math.Ceiling(Math.Sqrt(c));
        }

        public static int CalculateDistance(int i1, int i2, Problem problem)
        {
            var a = problem.Cities.FirstOrDefault(x => x.Id == i1);
            var b = problem.Cities.FirstOrDefault(x => x.Id == i2);
            var c = Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
            return (int)Math.Ceiling(Math.Sqrt(c));
        }
    }
}