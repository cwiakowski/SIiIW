using System;
using System.Collections;
using System.Collections.Generic;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services
{
    public class PathOperations
    {
        private static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static void MutatePath(IList<int> list)
        {
            var rnd = new Random();
            int a = rnd.Next(0, list.Count);
            int b;
            do
            {
                b = rnd.Next(0, list.Count);
            } while (b == a);

            Swap(list, a, b);
        }
    }
}