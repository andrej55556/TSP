using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ham_cycle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("1 - Чтение из файла");
                Console.WriteLine("2 - Генератор чисел");
                Console.WriteLine("3 - Набор тестов");
                key = Console.ReadKey();
                Console.WriteLine();
                switch(key.Key)
                {
                    case ConsoleKey.D1:
                        {
                            Enumeration graph1 = new Enumeration();
                            Console.Write("path : ");
                            string path = Console.ReadLine();
                            float[,] arr = graph1.ReadDistanceMatrix(path);
                            Graph graph2 = new Graph(arr);
                            Christofides graph3 = new Christofides(arr);
                            graph1.Solution();
                            graph2.Solution();
                            graph3.Solution();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            Random rnd = new Random();
                            //int size = rnd.Next(1, 13);
                            int size = 2;

                            Graph graph2 = new Graph(size);
                            graph2.GenerateRandomVerices();
                            float[,] arr = graph2.CalculateDistances();
                            Enumeration graph1 = new Enumeration(arr);
                            Christofides graph3 = new Christofides(arr);
                            float len_true = graph1.Solution();
                            float len2 = graph2.Solution();
                            float len3 = graph3.Solution();
                            Difference(len_true, len2, len3);
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            Enumeration graph1 = new Enumeration();
                            Console.Write("path : ");
                            string path = Console.ReadLine();
                            float[,] arr = graph1.ReadDistanceMatrix(path);
                            Graph graph2 = new Graph(arr);
                            Christofides graph3 = new Christofides(arr);
                            graph1.Solution();
                            graph2.Solution();
                            graph3.Solution();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Такого пункта не существует");
                            break;
                        }
                }
            } while (key.Key != ConsoleKey.Escape);
        }

        private static void Difference(float len_true, float len2, float len3)
        {
            float d1 = Math.Abs(len2 - len_true)/ len_true * 100;
            float d2 = Math.Abs(len3 - len_true)/ len_true * 100;
            Console.WriteLine("Отклонение для жадного алгоритма : " + d1);
            Console.WriteLine("Отклонение для алгоритма Кристофидеса : " + d2);
        }
    }
    }
