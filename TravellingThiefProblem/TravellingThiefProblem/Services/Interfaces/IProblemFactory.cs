using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services.Interfaces
{
    public interface IProblemFactory
    {
        Problem Generate(string filepath);
        //int[][] GenerateDistanceMatrix();
    }
}