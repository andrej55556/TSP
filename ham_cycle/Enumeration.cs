using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ham_cycle
{
    internal class Enumeration
    {
        float[,] distanceMatrix;
        float bestDistance;
        int[] bestRoute;

        public Enumeration()
        { }

        public Enumeration(float[,] matrix)
        {
            distanceMatrix = matrix;
            bestRoute = new int[matrix.GetLength(0)];
            for (int i = 0; i < bestRoute.Length; i++)
                bestRoute[i] = i;
        }

        public float[,] ReadDistanceMatrix(string filePath) // считывание с файла кол-во городов и матрицы расстояний
        {
            string[] lines = File.ReadAllLines(filePath);
            int n = int.Parse(lines[0]);    // кол-во городов

            distanceMatrix = new float[n, n];

            for (int i = 1; i <= n; i++)
            {
                string[] distances = lines[i].Split(' ');
                for (int j = 0; j < n; j++)
                {
                    distanceMatrix[i - 1, j] = float.Parse(distances[j]);
                    Console.Write(distanceMatrix[i - 1, j] + " ");
                }
                Console.WriteLine();
            }
            bestRoute = new int[distanceMatrix.GetLength(0)];
            for (int i = 0; i < bestRoute.Length; i++)
                bestRoute[i] = i;

            return distanceMatrix;
        }

        private float CalculateDistance(int[] route)
        {
            float distance = 0;
            for (int i = 0; i < route.Length - 1; i++)
            {
                distance += distanceMatrix[route[i], route[i + 1]];
            }

            distance += distanceMatrix[route[route.Length - 1], route[0]];

            return distance;
        }

        private void Swap(int[] route, int i, int j)
        {
            int temp = route[i];
            route[i] = route[j];
            route[j] = temp;
        }

        private void Permute(int[] route, int left, int right)
        {
            if (left == right)
            {
                float distance = CalculateDistance(route);
                if (distance < bestDistance)
                {
                    Array.Copy(route, bestRoute, route.Length);
                    bestDistance = distance;
                }
            }
            else
            {
                for (int i = left; i <= right; i++)
                {
                    Swap(route, left, i);
                    Permute(route, left + 1, right);
                    Swap(route, left, i);
                }
            }
        }

        public int[] GetRoute()
        {
            int[] arr = new int[distanceMatrix.GetLength(0)];
            for(int i = 0; i < distanceMatrix.GetLength(0); i++)
            {
                arr[i] = i;
            }
            return arr;
        }
        public float Solution()
        {
            int[] route = GetRoute();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Permute(route, 0, route.Length - 1); ;
            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;
            float len = CalculateDistance(route);
            Console.WriteLine("Кратчайший гамильтонов цикл : " + String.Join(" -> ", bestRoute));
            Console.WriteLine($"Длина цикла : {len}");
            Console.WriteLine("Время выполнения программы : " + ts.TotalMilliseconds);

            return len;
        }
    }
}
