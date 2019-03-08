using System.Collections.Generic;
using System.Drawing;

namespace TravellingThiefProblem.Models
{
    public class Problem
    {
        public string Name { get; set; }
        public string KnapsackDataType { get; set; }
        public int Dimension { get; set; }
        public int NumberOfItems { get; set; }
        public int KnapsackCapacity { get; set; }
        public double SpeedMin { get; set; }
        public double SpeedMax { get; set; }
        public double RentingRatio { get; set; }
        public string EdgeWeightType { get; set; }
        public List<City> Cities { get; set; }
        public List<Item> Items { get; set; }

    }
}