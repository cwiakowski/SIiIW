﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Utilities
{
    public class ProblemFactory
    {
        public Models.Problem GenerateProblem(string filepath)
        {
            var raw = DataReader.ReadFile(filepath);
            var lines = raw.Split("\n");

            var problem = new Problem()
            {
                Name = lines[0].Substring(lines[0].IndexOf("\t") + 1),
                KnapsackDataType = lines[1].Substring(lines[1].IndexOf("\t") + 1),
                Dimension = int.Parse(lines[2].Substring(lines[2].IndexOf("\t") + 1)),
                NumberOfItems = int.Parse(lines[3].Substring(lines[3].IndexOf("\t") + 1)),
                KnapsackCapacity = int.Parse(lines[4].Substring(lines[4].IndexOf("\t") + 1)),
                SpeedMin = double.Parse(lines[5].Substring(lines[5].IndexOf("\t") + 1)),
                SpeedMax = double.Parse(lines[6].Substring(lines[6].IndexOf("\t") + 1)),
                RentingRatio = double.Parse(lines[7].Substring(lines[7].IndexOf("\t") + 1)),
                EdgeWeightType = lines[8].Substring(lines[8].IndexOf("\t") + 1),
                Cities = new List<City>(),
                Items = new List<Item>()
            };

            var index = lines.ToList().IndexOf(lines.First(s => s.Contains("ITEMS SECTION")));
            for (int i = 10; i < lines.Length -1; i++)
            {
                if (i < index)
                {
                    var line = lines[i].Split("\t");
                    problem.Cities.Add(new City(int.Parse(line[0]) ,double.Parse(line[1]), double.Parse(line[2])));
                }
                else if(index < i)
                {
                    var line = lines[i].Split("\t");
                    problem.Items.Add(new Item(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3])));
                }
            }            
            return problem;
        }
    }
}