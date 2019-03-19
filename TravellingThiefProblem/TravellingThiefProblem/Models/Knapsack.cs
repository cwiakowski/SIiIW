using System;
using System.Collections.Generic;
using System.Linq;

namespace TravellingThiefProblem.Models
{
    public class Knapsack
    {
        public int CurrentWeight { get; set; }
        public int MaxWeight { get; private set; }
        public List<Item> Loot { get; set; }

        public Knapsack(int maxWeight)
        {
            CurrentWeight = 0;
            MaxWeight = maxWeight;
            Loot = new List<Item>();
        }

        public void AddItem(List<Item> items)
        {
            if (items.Count == 0) return;
            var item = items.FirstOrDefault(x => x.Weights <= MaxWeight - CurrentWeight);

            if (item == null) return;
            Loot.Add(item);
            CurrentWeight += item.Weights;
        }

        public int GetValue()
        {
            return Loot.Sum(x => x.Profit);
        }

        public override string ToString()
        {
            var s = $"PROFIT: {GetValue()}; CAPACITY: {CurrentWeight}/{MaxWeight} ";
            foreach (var l in Loot)
            {
                s = $"{s} {l.Id}";
            }
            return s;
        }
    }
}