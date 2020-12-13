using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using ABCNet;
using TravellingThiefProblem.Models;
using TravellingThiefProblem.Operations;
using TravellingThiefProblem.Services;
using TravellingThiefProblem.Utilities;

namespace TravellingThiefProblem
{
    class Program
    {


        static void Main(string[] args)
        {
            Jazda2();
            //Test();
            Console.WriteLine("done");
            Console.Read();
        }

        public static void Jazda2()
        {
            var ttp = new TTPSolver(0);
            ttp.StartAnt();
            //ttp.SaveData();
        }

        private static void Test()
        {
            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var ttp = new TTPSolver(i);
                    ttp.Start();
                    ttp.SaveData();
                    Console.WriteLine($"{i} {j} \t{ttp.Sw.Elapsed.ToString()}");
                }
            }
        }
    }
}
