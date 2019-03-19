using System.Collections.Generic;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Operations
{
    public interface ITPPOperations
    {
        List<Thief> Selection(List<Thief> thieves, int tour);
        List<Thief> Crossover(List<Thief> thieves, double px);
        List<Thief> Mutation(List<Thief> thieves, double pm);
    }
}