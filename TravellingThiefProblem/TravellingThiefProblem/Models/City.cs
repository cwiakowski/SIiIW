using System;

namespace TravellingThiefProblem.Models
{
    public class City
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public City(int id ,double x, double y)
        {
            Id = id;
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"{Id}\t({X},\t{Y})";
        }
    }
}