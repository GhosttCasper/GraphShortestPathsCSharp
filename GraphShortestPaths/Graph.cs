using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Кратчайшие пути между всеми парами вершин в графе (граф выбрать самому).
 */

namespace GraphShortestPaths
{
    public class Graph
    {
        public List<Vertex> VerticesList = new List<Vertex>();
        public List<Edge> EdgesList = new List<Edge>();
        private int Size;
        public int Time;

        public Graph(int size, string[] strs)
        {
            Size = size;
            for (int i = 1; i <= Size; i++)
                VerticesList.Add(new Vertex(i));

            try
            {
                int curIndex = 0;
                foreach (var str in strs)
                {
                    Vertex curVertexFrom = VerticesList[curIndex];
                    var array = str.Split();
                    int length = array.Length;
                    if (!string.IsNullOrEmpty(str))
                        for (int i = 0; i < length; i++)
                        {
                            int intVar = int.Parse(array[i++]);
                            if (intVar > Size)
                                throw new Exception("The vertex is missing");
                            Vertex curVertexTo = VerticesList[intVar - 1];

                            intVar = int.Parse(array[i]);
                            IncidentEdge curEdge = new IncidentEdge(curVertexTo, intVar);
                            curVertexFrom.AdjacencyList.Add(curEdge);
                        }

                    curIndex += 1;
                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is FormatException)
                    Console.WriteLine("String is empty (Graph)"); //throw new Exception("String is empty (Graph)"); 
                else
                    throw new Exception(ex.Message);
            }
        }

        public Graph()
        {
        }

        public bool Relax(Vertex incidentFrom, Vertex incidentTo, int weight)
        {
            if (incidentTo.Distance > incidentFrom.Distance + weight && incidentFrom.Distance != int.MaxValue)
            {
                incidentTo.Distance = incidentFrom.Distance + weight;
                incidentTo.Parent = incidentFrom;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Алгоритм Беллмана-Форда. Сложность 0(V*Е)
        /// </summary>
        public bool BellmanFordAlgorithm(Vertex source)
        {
            source.Distance = 0;

            bool stop = false;
            for (int i = 1; i < Size && !stop; i++)
            {
                stop = true;
                foreach (var curVertex in VerticesList)
                {
                    foreach (var incidentEdge in curVertex.AdjacencyList)
                    {
                        if (Relax(curVertex, incidentEdge.IncidentTo, incidentEdge.Weight))
                            stop = false;
                    }
                }
            }

            foreach (var curVertex in VerticesList)
            {
                foreach (var incidentEdge in curVertex.AdjacencyList)
                {
                    if (incidentEdge.IncidentTo.Distance > curVertex.Distance + incidentEdge.Weight)
                        return false;
                }
            }

            return true;
        }

        public string PrintPredecessorSubgraph(Vertex source)
        {
            string str = source.Index + " ";
            Vertex curParent = source;
            for (int i = 0; i < Size; i++)
            {
                foreach (var vertex in VerticesList)
                {
                    if (vertex.Parent == curParent)
                    {
                        str += vertex.Index + " ";
                        curParent = vertex;
                        break;
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// Поиск в глубину. Сложность 0(V + Е).
        /// </summary>
        public List<Vertex> DepthFirstSearch()
        {
            List<Vertex> vertices = new List<Vertex>();

            Time = 0;
            foreach (var curVertex in VerticesList)
            {
                if (curVertex.Discovered == false)
                    vertices = DFSVisit(curVertex, vertices);
            }

            return vertices;
        }

        private List<Vertex> DFSVisit(Vertex vertex, List<Vertex> vertices)
        {
            Time++;
            vertex.DiscoveryTime = Time;
            vertex.Discovered = true;
            vertices.Add(vertex);
            foreach (var curEdge in vertex.AdjacencyList)
            {
                if (curEdge.IncidentTo.Discovered == false)
                {
                    curEdge.IncidentTo.Parent = vertex;
                    DFSVisit(curEdge.IncidentTo, vertices);
                }
            }

            Time++;
            vertex.FinishingTime = Time;

            return vertices;
        }

        public List<Vertex> TopologicalSort() // only for directed acyclic graph
        {
            List<Vertex> vertices = DepthFirstSearch();
            return vertices;
        }

        /// <summary>
        /// В ориентированных ациклических графах. Сложность 0(V+Е)
        /// </summary>
        public void DagShortestPaths()// only for directed acyclic graph
        {
            List<Vertex> vertices = TopologicalSort();
            Vertex source = VerticesList[1];
            source.Distance = 0;

            foreach (var curVertex in vertices)
            {
                foreach (var incidentEdge in curVertex.AdjacencyList)
                {
                    Relax(curVertex, incidentEdge.IncidentTo, incidentEdge.Weight);
                }
            }
        }

        public void InitializeSingleSource(Vertex source)
        {
            foreach (var vertex in VerticesList)
            {
                vertex.Parent = null;
                vertex.Distance = int.MaxValue;
            }
            source.Distance = 0;
        }

        /// <summary>
        /// Алгоритм Дейкстры. Сложность 0(V^2)
        /// Работает, если в графе веса ребер, исходящих из некоторого истока s, могут быть отрицательными,
        /// веса всех других ребер неотрицательные, а циклы с отрицательными весами отсутствуют
        /// </summary>
        public void Dijkstra(Vertex source)
        {
            InitializeSingleSource(source);

            List<Vertex> discoveredVertices = new List<Vertex>();
            List<Vertex> verticesToAddInTree = new List<Vertex>(VerticesList);

            while (verticesToAddInTree.Count != 0)
            {
                Vertex curVertex = ExtractMin(verticesToAddInTree);
                discoveredVertices.Add(curVertex);

                foreach (var incidentEdge in curVertex.AdjacencyList)
                {
                    Relax(curVertex, incidentEdge.IncidentTo, incidentEdge.Weight);
                }
            }
        }

        private Vertex ExtractMin(List<Vertex> vertices)
        {
            Vertex minVertex = vertices[0];
            int min = minVertex.Distance;

            foreach (var vertex in vertices)
            {
                if (vertex.Distance < min)
                {
                    min = vertex.Distance;
                    minVertex = vertex;
                }
            }

            vertices.Remove(minVertex);
            return minVertex;
        }

        /// <summary>
        /// Алгоритм Джонсона. Сложность 0(V^2*lgV + VE), если Фибоначчиевая  куча, или 0(VE*lgV).
        /// Вычисление кратчайших путей между всеми парами вершин
        /// </summary>
        public void JohnsonAlgorithm()
        {
            Size += 1;
            VerticesList.Add(new Vertex(Size));
            Vertex curVertexFrom = VerticesList[Size - 1];
            for (int i = 0; i < Size - 1; i++)
            {
                Vertex curVertexTo = VerticesList[i];
                IncidentEdge curEdge = new IncidentEdge(curVertexTo, 0);
                curVertexFrom.AdjacencyList.Add(curEdge);
            }
            if (BellmanFordAlgorithm(curVertexFrom) == false)
                Console.WriteLine("Входной граф содержит цикл с отрицательным весом");
            else
            {
                foreach (var vertex in VerticesList)
                {
                    foreach (var incidentEdge in vertex.AdjacencyList)
                    {
                        incidentEdge.Weight = incidentEdge.Weight + vertex.Distance - incidentEdge.IncidentTo.Distance;
                    }
                }
                VerticesList.RemoveAt(Size - 1);
                Size -= 1;

                int[] Distances = new int[Size];
                for (int i = 0; i < Size; i++)
                {
                    Distances[i] = VerticesList[i].Distance;
                }
                int[,] matrix = new int[Size, Size];

                for (int i = 0; i < Size; i++)
                {
                    Dijkstra(VerticesList[i]);
                    for (int j = 0; j < Size; j++)
                    {
                        matrix[i, j] = VerticesList[j].Distance + Distances[j] - Distances[i];
                    }
                }

                StringBuilder output = new StringBuilder();
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        output.Append(matrix[i, j] + " ");
                    }
                    output.Append("\n");
                }
                Console.WriteLine(output);
            }
        }
        
        public string PrintPath(Vertex source, Vertex end, string str)
        {
            if (source == end)
            {
                str += source.Index + " ";
                return str;
            }

            if (end.Parent == null)
            {
                Console.WriteLine("Path from source to end is missing");
                return str;
            }

            PrintPath(source, end.Parent, str);
            str += end.Index + " ";
            return str;
        }
        
        //public string OutputGraph()
        //{
        //    string adjacencyMatrixStr = "";
        //    foreach (var row in AdjacencyMatrix)
        //    {
        //        foreach (var edge in row)
        //        {
        //            adjacencyMatrixStr += edge.Weight.ToString();
        //            adjacencyMatrixStr += " ";
        //        }
        //        adjacencyMatrixStr += "\n";
        //    }
        //    return adjacencyMatrixStr;
        //}

        public bool IsEmpty()
        {
            return VerticesList.Count == 0;
        }
    }
}

