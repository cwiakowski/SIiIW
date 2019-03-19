using System;
using System.Collections.Generic;

namespace TravellingThiefProblem.Models
{
    /// <summary>
    /// Model class
    /// </summary>
    public class City
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public List<Item> Items { get; set; }

        public City(int id ,double x, double y)
        {
            Id = id;
            X = x;
            Y = y;
            Items = new List<Item>();
        }
        public override string ToString()
        {
            var s = $"{Id}\t({X},\t{Y})\n";
            foreach (var it in Items)
            {
                s = $"{s}\t({it.Id}, {it.Weights}, {it.Profit}), {Items.Count}";
            }
            return s;
        }
    }
}