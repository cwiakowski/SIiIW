using System;
using System.Collections.Generic;
using System.Linq;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Operations
{
    public class PathOperations
    {
        private static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static void MutatePath(List<int> list)
        {
            var rnd = new Random();
            int a = rnd.Next(0, list.Count);
            int b;
            do
            {
                b = rnd.Next(0, list.Count-a);
            } while (b == a);

            //Swap(list, a, b);
            list.Reverse(a, b);
        }

        public static void Mutate(Thief thief)
        {
            MutatePath(thief.Path);
        }

        public static (List<int>, List<int>) Crossover(List<int> path1, List<int> path2)
        {
            var rnd = new Random();
            //first index of copied items
            var start = rnd.Next(0, path1.Count-1);
            //last index of copied items
            var stop = rnd.Next(start+1, path1.Count);

            var list1 = new List<int>();
            var list2 = new List<int>();
            for (int i = 0; i < path1.Count; i++)
            {
                //filling lists
                if (start <= i && i <= stop)
                {
                    list1.Add(path2[i]);
                    list2.Add(path1[i]);
                }
                else
                {
                    list2.Add(-1);
                    list1.Add(-1);
                }
            }

            int current = (stop+1)%list1.Count;
            int parentIndex = current;
            while (list2[current] == -1)
            {                
                var number1 = path2[parentIndex];
                if (list2.Contains(number1))
                {
                    parentIndex = (parentIndex + 1) % list2.Count;
                }
                else
                {
                    list2[current] = path2[parentIndex];
                    parentIndex = (parentIndex + 1) % list2.Count;
                    current = (current + 1) % list2.Count;
                }
            }

            current = (stop + 1) % list1.Count;
            parentIndex = current;
            while (list1[current] == -1)
            {
                var number1 = path1[parentIndex];
                if (list1.Contains(number1))
                {
                    parentIndex = (parentIndex + 1) % list1.Count;
                }
                else
                {
                    list1[current] = path1[parentIndex];
                    parentIndex = (parentIndex + 1) % list1.Count;
                    current = (current + 1) % list1.Count;
                }
            }
            return (list1, list2);
        }

        public static (List<int>, List<int>) Crossover(Thief thief1, Thief thief2)
        {
            return Crossover(thief1.Path, thief2.Path);
        }
    }
}