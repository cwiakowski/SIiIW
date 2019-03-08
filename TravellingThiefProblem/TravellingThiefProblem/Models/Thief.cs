using System.Collections.Generic;
using TravellingThiefProblem.Services;

namespace TravellingThiefProblem.Models
{
    public class Thief
    {
        public double CurrentSpeed { get; set; }
        public int KnapsackWeight { get; set; }
        public int KnapsackMaxWeight { get; set; }
        public List<int> Path { get; set; }
        public double SpeedMin { get; set; }
        public double SpeedMax { get; set; }

        public Thief(int knapsackMaxWeight, int speedMin, int speedMax)
        {
            Path = new List<int>();
            KnapsackWeight = 0;
            CurrentSpeed = UpdateVelocity();
            KnapsackMaxWeight = knapsackMaxWeight;
            SpeedMin = speedMin;
            SpeedMax = speedMax;
        }

        public Thief(Problem problem)
        {
            Path = new List<int>();
            KnapsackWeight = 0;
            CurrentSpeed = UpdateVelocity();
            KnapsackMaxWeight = problem.KnapsackCapacity;
            SpeedMin = problem.SpeedMin;
            SpeedMax = problem.SpeedMax;
        }

        public void GenerateRandomPath(IEnumerable<City> list)
        {
            Path = ThiefService.GenerateRandomPath(list);
        }

        public void GenerateTestPath(IEnumerable<City> list)
        {
            Path = ThiefService.GenerateTestPath(list);
        }

        public double UpdateVelocity()
        {
            return SpeedMax - KnapsackWeight * (SpeedMax - SpeedMin) / KnapsackMaxWeight;
        }
    }
}