using System.Collections.Generic;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services.Interfaces
{
    public interface IThiefService
    {
        List<int> GenerateRandomPath(IEnumerable<City> list);
        //int CalculatePathLength(List<int> path, List<City> cities);
    }
}