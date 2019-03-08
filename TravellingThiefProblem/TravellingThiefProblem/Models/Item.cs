using System.Runtime.InteropServices.ComTypes;

namespace TravellingThiefProblem.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int Profit { get; set; }
        public int Weights { get; set; }
        public int AssignedNodeNumber { get; set; }

        public Item(int id, int profit, int weights, int assignedNodeNumber)
        {
            Id = id;
            Profit = profit;
            Weights = weights;
            AssignedNodeNumber = assignedNodeNumber;
        }

        public override string ToString()
        {
            return $"{Id}\t{Profit}\t{Weights}\t{AssignedNodeNumber}";
        }
    }
}