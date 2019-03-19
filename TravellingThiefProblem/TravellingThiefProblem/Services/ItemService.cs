using System.Collections.Generic;
using System.Linq;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services
{
    public class ItemService
    {
        private Problem _problem;

        public ItemService(Problem problem)
        {
            _problem = problem;
        }

        public void SortItemsBySmallestWeight()
        {
            foreach (var city in _problem.Cities)
            {
                city.Items = city.Items.OrderBy(x => x.Weights).ThenByDescending(f => f.Profit).ToList();
            }
        }

        public void SortItemsByBiggestValue()
        {
            foreach (var city in _problem.Cities)
            {
                city.Items = city.Items.OrderByDescending(x => x.Profit).ThenBy(f => f.Weights).ToList();
            }
        }

        public void SortItemsByBestValueWeightRatio()
        {
            foreach (var city in _problem.Cities)
            {
                city.Items = city.Items.OrderByDescending(x => x.Profit/x.Weights).ToList();
            }
        }



    }
}