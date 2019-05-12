using System.Collections.Generic;

namespace Morris.Models
{
    public class ScoreHolder
    {
        public Board Board { get; set; }
        public double Score { get; set; }
        public string Decision { get; set; }
        public List<Mill> Mills { get; set; }
        public int PlacedStones { get; set; }
        public FieldState PlayersTurn { get; set; }
    }
}