using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services.Interfaces
{
    public interface ICityService
    {
        int CalculateDistance(City a, City b);
    }
}