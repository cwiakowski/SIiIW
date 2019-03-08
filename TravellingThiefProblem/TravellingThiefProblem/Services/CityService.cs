using System;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Services.Interfaces;

namespace TravellingThiefProblem.Services
{
    public class CityService : ICityService
    {
        /// <summary>
        /// Returns Calculated Distance between two cities
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int CalculateDistance(City a, City b)
        {
            var c = Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
            return (int)Math.Ceiling(Math.Sqrt(c));
        }

    }
}