using System;
using System.Collections.Generic;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Services;

namespace TravellingThiefProblem.Operations
{
    public class TPPOperations : ITPPOperations
    {
        public List<Thief> Selection(List<Thief> thieves, int tour)
        {
            throw new System.NotImplementedException();
        }

        public List<Thief> Crossover(List<Thief> thieves, double px)
        {
            throw new System.NotImplementedException();
        }

        public List<Thief> Mutation(List<Thief> thieves, double pm)
        {
            var rnd = new Random();
            foreach (var t in thieves)
            {
                if (rnd.NextDouble() < pm)
                {
                    PathOperations.MutatePath(t.Path);
                }
            }

            return thieves;
        }
    }
}