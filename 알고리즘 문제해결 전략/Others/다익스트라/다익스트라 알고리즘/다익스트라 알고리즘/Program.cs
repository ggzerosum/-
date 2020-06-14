using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 다익스트라_알고리즘
{
    class Program
    {
        static void Main(string[] args)
        {
            // 시작점을 기준으로 다른 모든 노드에대한 최단 경로를 파악하는 것이 다익스트라 알고리즘
            int veticesCount = 5;
            DijkstraWithArray dijkstraWithArray = new DijkstraWithArray(5);

            dijkstraWithArray.SetEdge(0, 1, 4);
            dijkstraWithArray.SetEdge(0, 3, 2);

            dijkstraWithArray.SetEdge(3, 1, 1);
            dijkstraWithArray.SetEdge(3, 2, 1);
            dijkstraWithArray.SetEdge(2, 3, 2);

            dijkstraWithArray.SetEdge(1, 4, 3);

            dijkstraWithArray.SetEdge(4, 3, 3);
            dijkstraWithArray.SetEdge(4, 2, 6);

            int start = 0;
            int destination = 4;

            int[] shortestPathes = new int[veticesCount];
            shortestPathes[start] = -1; // 시작점은 직전 최단 경로가 존재하지 않으므로 따로 처리를 해주어야한다.

            dijkstraWithArray.ComputePath(start, destination, shortestPathes);

            dijkstraWithArray.Print();

            PrintShortestPathFromStartPoint(destination, shortestPathes);
        }

        private static void PrintShortestPathFromStartPoint(int dest, int[] shortestPathes)
        {
            Console.Write("최단 경로 : ");
            int current = dest;
            while (true) // -1이 입력된 인덱스는 시작점을 가르키도록 정의해두었다.
            {
                Console.Write($"{current}");
                int previousShortestVertex = shortestPathes[current];
                if (previousShortestVertex == -1) // 시작점일 경우 중단
                {
                    break;
                }

                Console.Write($" - ");
                current = previousShortestVertex;
            }

            Console.WriteLine();
        }
    }

    class DijkstraWithArray
    {
        internal int[,] edges;

        internal int[] vertices;

        private int[] visited;
        private int Infinite = int.MaxValue;

        public DijkstraWithArray(int vertexCount)
        {
            vertices = new int[vertexCount];
            visited = new int[vertexCount];

            edges = new int[vertexCount, vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    edges[i, j] = -1; // 음수는 연결이 안되어있음을 의미
                }
            }
        }

        public void SetEdge(int @from, int @to, int cost)
        {
            edges[from, to] = cost;
        }

        // 배열을 이용하는 다익스트라 알고리즘
        public void ComputePath(int start, int destination, int[] shortestPathes/*index 정점의 직전 최단 경로 정점이 무엇인 지 저장하기위한 배열*/)
        {
            SetInfiniteToAllVerticesExceptStartVertex(start);

            while (HasUnVisitedVertex()) // 중복계산을 막기위해서. 만일, 배열이 아니라 계산할 경로를 리스트에 넣어서 계산이 완료될 때마다 하나씩 빼는 방식이라면 리스트가 Empty일 때까지 계산하면 된다.
            {
                int vertex = FindMinimumVertexNotVisited();
                MarkAsVisited(vertex);

                // 현재 검사할 방문되지않은 가장 작은 Vertex가 Destination이라면 계산을 중단하여 최적화할 수 있다.
                if (vertex == destination)
                {
                    break;
                }

                for (int i = 0; i < vertices.Length; i++)
                {
                    int from = vertex;
                    int to = i;

                    if (IsConnected(from, to))
                    {
                        int dist = this.vertices[from] + this.edges[from, to];
                        if (dist < this.vertices[to]) // 같은 경우는 처리하면 안된다. 중복으로 계산되기 때문
                        {
                            this.vertices[to] = dist;
                            shortestPathes[to] = from; // 직전의 경로를 입력, 이를 통해 원하는 도착점 A에서 경로를 역산해나가면 시작점을 반드시 찾을 수 있다.
                        }
                    }
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < this.vertices.Length; i++)
            {
                Console.WriteLine($"{i}정점까지의 최소 비용 : {this.vertices[i]}");
            }
        }


        private void SetInfiniteToAllVerticesExceptStartVertex(int start)
        {
            // Start 이외에는 모두 Infinite로 초기화
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Infinite;
            }

            vertices[start] = 0;
        }

        private int FindMinimumVertexNotVisited()
        {
            int minValue = Infinite;
            int minIndex = -1;
            for (int i = 0; i < this.vertices.Length; i++)
            {
                if (!IsVisited(i))
                {
                    if (this.vertices[i] < minValue)
                    {
                        minValue = this.vertices[i];
                        minIndex = i;
                    }
                }
            }

            return minIndex;
        }

        private bool IsConnected(int from, int to)
        {
            return this.edges[from, to] != -1;
        }
        private bool HasUnVisitedVertex()
        {
            for (int i = 0; i < this.visited.Length; i++)
            {
                if (this.visited[i] == 0)
                {
                    return true;
                }
            }

            return false;
        }
        private bool IsVisited(int vertex)
        {
            return this.visited[vertex] == 1;
        }
        private void MarkAsVisited(int vertex)
        {
            this.visited[vertex] = 1;
        }
    }
}