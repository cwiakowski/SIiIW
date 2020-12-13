using System;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Services;

namespace AntColonySystem
{
    public class Edge
    {
        public City Start { get; set; }
        public City End { get; set; }
        public double Length { get; set; }
        public double Pheromone { get; set; }
        public double Weight { get; set; }

        public Edge() { }

        public Edge(City start, City end)
        {
            Start = start;
            End = end;
            Length = CityService.CalculateDistance(Start, End);
        }
    }
}
