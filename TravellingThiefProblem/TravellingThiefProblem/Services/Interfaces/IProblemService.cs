using System.Collections.Generic;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services.Interfaces
{
    public interface IProblemService
    {
        Problem Generate(string filepath);
        int[,] GenerateDistanceMatrix(int dim, List<City> cities);
    }
}