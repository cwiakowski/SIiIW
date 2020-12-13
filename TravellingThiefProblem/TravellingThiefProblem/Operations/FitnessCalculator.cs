using System.Linq;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Services;

namespace TravellingThiefProblem.Operations
{
    public class FitnessCalculator : IFitnessCalculator
    {
        /// <summary>
        /// Calculates thief's fitness
        /// </summary>
        /// <param name="thief"></param>
        /// <param name="problem"></param>
        /// <returns></returns>
        public double CalculateFitness(Thief thief, Problem problem)
        {
            thief.Fitness = ThiefService.CalculatePathLength(thief.Path, problem.Cities);
            return ThiefService.CalculatePathLength(thief.Path, problem.Cities);

            thief.Reset();
            var time = TimeTravel(thief, problem);
            var profit = Profit(thief);
            
            thief.Fitness = profit - time;
            return thief.Fitness;
        }

        private double TimeTravel(Thief thief, Problem problem)
        {
            var time = 0.0;

            for (int i = 0; i < thief.Path.Count; i++)
            {
                thief.UpdateVelocity();
                //finds starting city and destination in this step
                var start = problem.Cities.FirstOrDefault(x => x.Id == thief.Path[i]);
                var stop = problem.Cities.FirstOrDefault(x => x.Id == thief.Path[(i+1)% thief.Path.Count]);
                //sums travel time
                time = time + CityService.CalculateDistance(start, stop)/thief.CurrentSpeed;
                //Add item to knapsack
                thief.Knapsack.AddItem(stop.Items);
            }

            return time;
        }

        private double Profit(Thief thief)
        {
            return thief.Knapsack.GetValue();
        }
    }
}