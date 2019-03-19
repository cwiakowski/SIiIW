using System.Collections.Generic;
using System.Linq;
using TravellingThiefProblem.Services;

namespace TravellingThiefProblem.Models
{
    /// <summary>
    /// Model class
    /// </summary>
    public class Thief
    {
        public double CurrentSpeed { get; set; }
        public List<int> Path { get; set; }
        public Knapsack Knapsack { get; set; }
        public double SpeedMin { get; set; }
        public double SpeedMax { get; set; }
        public double Fitness { get; set; }

        public Thief(int knapsackMaxWeight, int speedMin, int speedMax)
        {
            Path = new List<int>();
            Knapsack = new Knapsack(knapsackMaxWeight);
            UpdateVelocity();
            SpeedMin = speedMin;
            SpeedMax = speedMax;
            Fitness = double.MinValue;
        }

        public Thief(Problem problem)
        {
            Path = new List<int>();
            Knapsack = new Knapsack(problem.KnapsackCapacity);
            UpdateVelocity();
            SpeedMin = problem.SpeedMin;
            SpeedMax = problem.SpeedMax;
            Fitness = double.MinValue;
        }

        public Thief(Thief thief)
        {
            CurrentSpeed = thief.CurrentSpeed;
            Path = thief.Path.ToList();
            Knapsack = thief.Knapsack;
            SpeedMin = thief.SpeedMin;
            SpeedMax = thief.SpeedMax;
            Fitness = thief.Fitness;
        }

        public void GenerateRandomPath(IEnumerable<City> list)
        {
            Path = ThiefService.GenerateRandomPath(list);
        }

        public void UpdateVelocity()
        {
            CurrentSpeed =  SpeedMax - Knapsack.CurrentWeight * (SpeedMax - SpeedMin) / Knapsack.MaxWeight;
        }

        public void Reset()
        {
            Knapsack.CurrentWeight = 0;
            Knapsack.Loot.Clear();
        }

        public override string ToString()
        {
            var str = string.Empty;
            foreach (var city in Path)
            {
                str = $"{str}\t{city}";
            }

            return str;
        }
    }
}