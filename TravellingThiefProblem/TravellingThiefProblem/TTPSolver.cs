using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Operations;
using TravellingThiefProblem.Services;
using TravellingThiefProblem.Utilities;

namespace TravellingThiefProblem
{
    public class TTPSolver
    {
        private int _populationSize = 100;
        private int _generations = 500;
        private double _px = 70;                //0-100
        private double _pm = 5;                 //0-100
        private int _tour = 5;
        private int _greedyAlgorhitmType = 1;   //0 - Min(Weight); 1 - Max(Profit/Weight); 2 - Max(Profit)
        private int _selectionType = 0;         //0 - Tour, 1- Ruletka
        private int _fileIndex = 5;             //0-4 easy, 5-9 hard, 10-14 medium, 15-16 trival
        private IFitnessCalculator _fitc;
        private bool _printLoggerData = true;


        public List<Thief> Thieves { get; set; }
        public Logger Log { get; set; }
        public Problem Problem { get; set; }
        public Stopwatch Sw { get; set; }

        public TTPSolver()
        {
            
        }

        public TTPSolver(int fileIndex)
        {
            _fileIndex = fileIndex;
        }

        public void Start()
        {
            Initialize();
            Sw.Start();
            BeginEvaluation();
            Sw.Stop();
        }

        //Saves data in file
        public void SaveData()
        {
            Log.SaveToFile(DataReader.FilePaths[_fileIndex], Sw.Elapsed.ToString());
        }
        
        private void BeginEvaluation()
        {
            for (int i = 0; i < _generations; i++)
            {
                //Console.WriteLine($"Generation \t{i}");
                CalculateFitness();
                LogData();
                Selection();
                Crossover();
                Mutation();
            }
        }

        private void CalculateFitness()
        {
            foreach (var t in Thieves)
            {
                _fitc.CalculateFitness(t, Problem);
                //Console.WriteLine(t.Fitness);
            }
        }

        private void Selection()
        {
            if (_selectionType == 0)
                SelectionTournament();
            else if (_selectionType == 1) SelectionRoulette();
        }

        //Tournament selection of 
        public void SelectionTournament()
        {
            var newGen = new List<Thief>();
            var rnd = new Random();
            for (int i = 0; i < _populationSize; i++)
            {
                //losowanie uczestnikow
                var participants = new SortedSet<int>();
                while (participants.Count < _tour + 1)
                {
                    participants.Add(rnd.Next(0, _populationSize));
                }
                //wybor zwyciezcy turnieju
                var best = Thieves[participants.ElementAt(0)];
                for (int j = 0; j < participants.Count; j++)
                {
                    if (best.Fitness < Thieves[participants.ElementAt(j)].Fitness)
                    {
                        best = Thieves[participants.ElementAt(j)];
                    }
                }
                newGen.Add(new Thief(best));
            }

            Thieves = newGen;
        }

        private void SelectionRoulette()
        {
            var probabilities = new List<double>();
            var newGen = new List<Thief>();
            var min = Thieves.Min(x => x.Fitness);
            
            var offset = 0.0;
            //generate starting probability values
            foreach (var t in Thieves)
            {
                probabilities.Add(t.Fitness);
            }
            //remove negative values
            if (min < 0)
            {
                probabilities = Normalize(probabilities, min, offset);
            }

            var sum = probabilities.Sum(x => x);
            var probsum = 0.0;
            //calculate proper probability for this type of selection
            for (int i = 0; i < probabilities.Count; i++)
            {
                probabilities[i] = (probabilities[i] / sum) + probsum;
                probsum = probabilities[i];
            }
            var rnd = new Random();
            //Selection process
            for (int i = 0; i < _populationSize; i++)
            {
                var last = 0.0;
                var j = 0;
                double winner = probabilities[probabilities.Count - 1] * rnd.NextDouble();
                foreach (var p in probabilities)
                {
                    
                    if (last < winner && winner < p)
                    {
                        newGen.Add(new Thief(Thieves[j]));
                        j = 0;
                        break;
                    }

                    j++;
                }
            }

            Thieves = newGen;

        }

        private List<double> Normalize(List<double> list, double min, double offset)
        {
            var l = new List<double>();
            foreach (var val in list)
            {
                l.Add(val - min + offset);
            }

            return l;
        }

        private void Mutation()
        {
            var rnd = new Random();
            for (int i = 0; i < _populationSize; i++)
            {
                if (rnd.Next(0, 100) < _pm)
                {
                    PathOperations.Mutate(Thieves[i]);
                }
            }
        }

        private void Crossover()
        {
            var rnd = new Random();
            for (int i = 0; i < (_populationSize / 2); i++)
            {
                if (rnd.Next(0, 100) < _px)
                {
                    (Thieves[i].Path, Thieves[i + 1].Path) = PathOperations.Crossover(Thieves[i], Thieves[i + 1]);
                }
            }
        }

        private void SortItems()
        {
            var iSer = new ItemService(Problem);
            if (_greedyAlgorhitmType == 0)
            {
                iSer.SortItemsBySmallestWeight();
            }
            else if(_greedyAlgorhitmType == 1)
            {
                iSer.SortItemsByBestValueWeightRatio();
            }
            else if (_greedyAlgorhitmType == 2)
            {
                iSer.SortItemsByBiggestValue();
            }
        }

        private void Initialize()
        {
            Sw = new Stopwatch();
            _fitc = new FitnessCalculator();
            InitializeProblem();
            InitializeThieves();
            InitializeLogger();
        }

        public void InitializeThieves()
        {
            Thieves = ThiefService.GenerateRandomThieves(Problem, _populationSize);
        }

        public void InitializeProblem()
        {
            DataReader.GenerateFilePaths();
            var factory = new ProblemService();
            Problem = factory.Generate(DataReader.FilePaths[_fileIndex]);
            SortItems();
        }

        private void InitializeLogger()
        {
            Log = new Logger(_populationSize, _generations, _px, _pm, _selectionType, _tour, _greedyAlgorhitmType);
            Log.Stats.Add("generation;best;average;worst");
        }

        private void LogData()
        {
            Log.AddToLogger(Thieves.Max(x => x.Fitness), Thieves.Average(x => x.Fitness), Thieves.Min(x => x.Fitness), _printLoggerData);
        }
    }
}