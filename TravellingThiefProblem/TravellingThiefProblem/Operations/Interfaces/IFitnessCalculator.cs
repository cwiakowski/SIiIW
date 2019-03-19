using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Operations
{
    public interface IFitnessCalculator
    {
        double CalculateFitness(Thief thief, Problem problem);
    }
}