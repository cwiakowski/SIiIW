using System;
using System.Collections.Generic;
using AntColonySystem;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Operations;

namespace TravellingThiefProblem.Services
{
    public class Antek
    {
        private Problem problem;
        IFitnessCalculator _calc;

        public Antek(Problem problem, IFitnessCalculator calc)
        {
            this.problem = problem;
            _calc = calc;
        }
        public void Run()
        {
            Graph graph = new Graph(problem.Cities, true);
            GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm(graph);
            double greedyShortestTourDistance = greedyAlgorithm.Run();  // get shortest tour using greedy algorithm

            Parameters parameters = new Parameters()  // Most parameters will be default. We only have to set T0 (initial pheromone level)
            {
                T0 = (1.0 / (graph.Dimensions * greedyShortestTourDistance))
            };
            parameters.Show();

            Solver solver = new Solver(parameters, graph);
            List<double> results = solver.RunACS(); // Run ACS

            Console.WriteLine("Time: " + solver.GetExecutionTime());
            Console.ReadLine();
        }

    }
}