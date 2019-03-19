using System;
using System.Collections.Generic;
using System.Linq;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Operations;
using TravellingThiefProblem.Services.Interfaces;

namespace TravellingThiefProblem.Services
{
    public static class ThiefService
    {
        public static List<int> GenerateRandomPath(IEnumerable<City> list)
        {
            var path = list.Select(x => x.Id).OrderBy(a => Guid.NewGuid()).ToList();
            return path;
        }

        public static int CalculatePathLength(List<int> path, List<City> cities)
        {
            var pathLength = 0;
            for (int i = 0; i < path.Count(); i++)
            {
                var city1 = cities.FirstOrDefault(x => x.Id == path[i]);
                var city2 = cities.FirstOrDefault(x => x.Id == path[(i + 1) % path.Count()]);
                pathLength += CityService.CalculateDistance(city1,city2);
            }
            return pathLength;
        }

        public static List<Thief> GenerateRandomThieves(Problem problem, int popSize)
        {
            var thieves = new List<Thief>();

            for (int i = 0; i < popSize; i++)
            {
                thieves.Add(new Thief(problem));
            }
            foreach (var thief in thieves)
            {
                thief.GenerateRandomPath(problem.Cities);
            }

            return thieves;
        }
    }
}