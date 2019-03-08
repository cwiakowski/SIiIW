using System.Collections.Generic;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem
{
    public class TPPSolver
    {
        private int _populationSize = 100;
        private int _generations = 100;
        private double _px = 0.7;
        private double _pm = 0.01;
        private int _tour = 5;
        private List<Thief> _thieves;



        public void GenerateRandomThieves()
        {
            _thieves = new List<Thief>();
            for (int i = 0; i < _populationSize; i++)
            {
                
            }
        }
    }
}