using System;
using System.Collections.Generic;

namespace Morris.Models
{
    public class ScoreHolder : IDisposable
    {
        public Board Board { get; set; }
        public double Score { get; set; }
        public string Decision { get; set; }
        public List<Mill> Mills { get; set; }
        public int PlacedStones { get; set; }
        public int Moves { get; set; }

        public ScoreHolder()
        {
            Score = double.MinValue;
        }

        public void Dispose()
        {
            Board = null;
            Decision = null;
            Mills = null;
            GC.SuppressFinalize(this);
        }
    }
}