using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ham_cycle
{
    class Christofides
    {
        private float[,] m;
        public float[,] M
        {
            get { return m; }
            set { m = value; }
        }
        private int n;
        public int N
        {
            get { return n; }
            set { n = value; }
        }
        public Christofides()
        {
            this.n = 0;
            this.m = new float[n, n];
        }
        public Christofides(int n)
        {
            this.n = n;
            this.m = new float[n, n];
        }
        public Christofides(int N, float[,] M) : this(N)
        {
            this.m = M;
        }
        public Christofides(float[,] M)
        {
            this.m = M;
            this.n = M.GetLength(0);
        }
        public void Print()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Console.Write(m[i, j].ToString() + ' ');
                Console.WriteLine();
            }
        }
        private int MinimumKey(float[] key, bool[] mstSet)
        {
            float minKey = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < n; v++)
            {
                if (!mstSet[v] && key[v] < minKey)
                {
                    minKey = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }
        // Алгоритм Прима. Метод для нахождения минимального остовного дерева (MST)
        public float[,] MST()
        {
            int[] parent = new int[n]; // Массив для хранения родительских вершин
            float[] key = new float[n]; // Массив для хранения ключей (минимальных весов)
            bool[] mstSet = new bool[n]; // Массив для отметки выбранных вершин

            // Инициализация ключей и отметка всех вершин как не выбранных
            for (int i = 0; i < n; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            /*key[0] = 0; // Установка ключа начальной вершины в 0
            parent[0] = -1; // Начальная вершина является корневой*/
            Random r = new Random();
            int ri = r.Next(0, n);
            key[ri] = 0; // Установка ключа начальной вершины в случайную вершину
            parent[ri] = -1; // Начальная вершина является корневой

            // Поиск минимального остовного дерева
            for (int count = 0; count < n - 1; count++)
            {
                int u = MinimumKey(key, mstSet); // Выбор вершины с минимальным ключом
                mstSet[u] = true; // Отметка выбранной вершины

                // Обновление ключей смежных вершин, если они еще не выбраны и имеют больший вес
                for (int v = 0; v < n; v++)
                {
                    if (m[u, v] != 0 && !mstSet[v] && m[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = m[u, v];
                    }
                }
            }

            float[,] mstMatrix = new float[n, n]; // MST в виде матрицы смежности
            /*for (int i = 1; i < n; i++)
            {
                mstMatrix[i, parent[i]] = m[i, parent[i]];
                mstMatrix[parent[i], i] = m[i, parent[i]];
            }*/
            for (int i = 0; i < n; i++)
            {
                if (parent[i] != -1)
                {
                    mstMatrix[i, parent[i]] = m[i, parent[i]];
                    mstMatrix[parent[i], i] = m[i, parent[i]];
                }
            }

            return mstMatrix;
        }
        //Гамильтонов цикл
        public int[] RootTraversal(float[,] mst, int root)
        {
            bool[] visited = new bool[n]; // Массив для отметки посещенных вершин
            int[] traversal = new int[n]; // Массив для хранения обхода вершин
            int index = 0;
            DepthFirstTraversal(mst, visited, root, traversal, ref index);

            int[] Hamiltoniancycle = new int[n + 1];

            for (int i = 0; i < n; i++)
            {
                Hamiltoniancycle[i] = traversal[i];
            }
            Hamiltoniancycle[n] = root;

            return Hamiltoniancycle;
        }
        private void DepthFirstTraversal(float[,] mst, bool[] visited, int vertex, int[] traversal, ref int index)
        {
            visited[vertex] = true;
            traversal[index++] = vertex;

            for (int i = 0; i < n; i++)
            {
                if (mst[vertex, i] != 0 && !visited[i])
                {
                    DepthFirstTraversal(mst, visited, i, traversal, ref index);
                }
            }
        }
        //Гамильтонов цикл без аргументов
        public int[] RootTraversal()
        {
            Random r = new Random();

            return RootTraversal(this.MST(), r.Next(0, n));
        }

        public void PrintCycle(int[] arr)
        {
            for (int i = 0; i < n + 1; i++)
                if (i < n)
                    Console.Write(arr[i] + 1 + "-");
                else Console.WriteLine(arr[i] + 1);
        }
        //длина Гамильтонова цикла
        public float CycleLength(int[] cycle)
        {
            float sum = 0;

            for (int i = 1; i < n + 1; i++)
            {
                sum += m[cycle[i - 1], cycle[i]];
            }

            return sum;
        }
        //длина Гамильтонова цикла без аргументов
        public float Hamiltoniancyclelength()
        {
            return CycleLength(RootTraversal());
        }
        //Алгоритм Крситофидеса
        public float Solution()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[,] MST = this.MST();

            Random r = new Random();
            int[] HC = RootTraversal(MST, r.Next(0, n));

            float HCL = CycleLength(HC);

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            //Console.WriteLine("Исходный граф");
            //this.Print();

            //Console.WriteLine("Кратчайший гамильтонов цикл : " + String.Join(" -> ", HC));
            //Console.WriteLine($"Длина цикла : {HCL}");
            Console.WriteLine("Время выполнения программы : " + ts.TotalMilliseconds);

            return HCL;
        }

        public void RandomGenerate()
        {
            Random r = new Random();
            float a = (float)r.NextDouble() * (100 - 20) + 20;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (i != j && j < i)
                    {
                        m[i, j] = a;
                        m[j, i] = a;
                    }
                }
        }
    }
}
