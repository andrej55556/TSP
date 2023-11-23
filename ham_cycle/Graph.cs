using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ham_cycle
{
    internal class Graph
    {
        private int n;
        private Point[] vertices;
        private float[,] matrix;
        private List<int> cycle;

        public Graph(int n) 
        { 
            this.n = n;
            vertices = new Point[n];
            matrix = new float[n, n];
            cycle = new List<int>();
        }

        public Graph(float[,] arr)
        {
            matrix = arr;
            n = arr.GetLength(0);
            cycle = new List<int>();
        }

        public void GenerateRandomVerices()
        {
            Random rnd = new Random();
            HashSet<Point> uniqueVertices = new HashSet<Point>();

            while (true)
            {
                for (int i = 0; i < n; i++)
                {
                    int x = rnd.Next(100);
                    int y = rnd.Next(100);
                    Point vertex = new Point(x, y);

                    if (!uniqueVertices.Add(vertex))
                    {
                        // Вершина уже сгенерирована, повторяем генерацию
                        uniqueVertices.Clear();
                        break;
                    }

                    vertices[i] = vertex;
                }

                if (uniqueVertices.Count == n)
                {
                    // Все вершины уникальны, выходим из цикла
                    break;
                }
            }
        }

        public float[,] CalculateDistances()
        {
            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = GetEuclideanDistances(vertices[i], vertices[j]);
                } 
            }
            return matrix;
        }

        private float GetEuclideanDistances(Point p1, Point p2)
        {
            return (float)Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }

        public void PrintMatrix()
        {
            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(String.Format("{0:F1}", matrix[i, j]) + " ");
                }
                Console.WriteLine();
            }
        }

        public void FindHamCycle()
        {
            bool[] visited = new bool[n]; // n + n + n
            int startVertex = 0; // 2
            visited[startVertex] = true; // 2
            cycle.Add(startVertex); // 2

            while(cycle.Count < n) // n + 1 + n + 1
            {
                int currentVertex = cycle[cycle.Count - 1]; // 5
                int nextVertex = -1; // 2
                float minWeight = float.MaxValue; // 3

                for(int neighbor = 0; neighbor < n; neighbor++) // 2 + n + 1 + n + n
                {
                    float weight = matrix[currentVertex, neighbor]; // 3n
                    if(weight < minWeight && !visited[neighbor]) // n + n + 2n
                    {
                        minWeight = weight; // n
                        nextVertex = neighbor; // n
                    }
                }

                cycle.Add(nextVertex); // 2
                visited[nextVertex] = true; // 2
            }
        }

        private float CalculateLengthHamCycle()
        {
            float length_cycle = 0;
            for(int i = 0; i < n - 1; i++)
            {
                length_cycle += matrix[cycle[i], cycle[i + 1]];

            }
            length_cycle += matrix[cycle[cycle.Count - 1], cycle[0]];

            return length_cycle;
        }

        public void PrintVertex()
        {
            for(int i = 0; i < n; i++) //verices.GetLength(0)
            {
                Console.WriteLine((i + 1).ToString() + ":" + vertices[i].x.ToString() + " " + vertices[i].y.ToString());
            }
        }

        public float Solution()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FindHamCycle();
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            float length_cycle = CalculateLengthHamCycle();
            //Console.WriteLine("Кратчайший гамильтонов цикл : " + String.Join(" -> ", cycle));
            //Console.WriteLine($"Длина цикла : {length_cycle}");
            Console.WriteLine("Время выполнения программы : " + ts.TotalMilliseconds);

            return length_cycle;
        }


    }
}
