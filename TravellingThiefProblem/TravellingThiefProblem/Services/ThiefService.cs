﻿using System;
using System.Collections.Generic;
using System.Linq;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services
{
    public static class ThiefService
    {
        public static List<int> GenerateRandomPath(IEnumerable<City> list)
        {
            var path = list.Select(x => x.Id).OrderBy(a => Guid.NewGuid()).ToList();
            return path;
        }

        public static double CalculatePathLength(List<int> path, List<City> cities)
        {
            var pathLength = 0.0;

            for (int i = 0; i < path.Count(); i++)
            {
                var city1 = cities.FirstOrDefault(x => x.Id == path[i]);
                var city2 = cities.FirstOrDefault(x => x.Id == path[(i + 1) % path.Count()]);
                pathLength += city1.CalculateDistance(city2);
            }
            return pathLength;
        }

        public static List<int> GenerateTestPath(IEnumerable<City> list)
        {
            var path = new List<int>
            {
                1,
                2,
                3,
                4
            };
            return path;
        }
    }
}