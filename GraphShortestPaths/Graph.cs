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
                return false;
            }

            return true;
        }

        /// <summary>
        /// Алгоритм Беллмана-Форда. Сложность 0(V*Е)
        /// </summary>
        public bool BellmanFordAlgorithm()
        {
            Vertex source = VerticesList[0];
            source.Distance = 0;

            bool stop = false;
            for (int i = 1; i < Size && !stop; i++)
            {
                stop = true;
                foreach (var curVertex in VerticesList)
                {
                    foreach (var incidentEdge in curVertex.AdjacencyList)
                    {
                        stop = Relax(curVertex, incidentEdge.IncidentTo, incidentEdge.Weight);
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

        public void BuildInducedGraph(List<int> verticesIndexes)
        {
            foreach (var index in verticesIndexes)
            {
                foreach (var incidentEdge in VerticesList[index - 1].AdjacencyList)
                {
                    var adjacencyVertex = incidentEdge.IncidentTo.AdjacencyList;
                    adjacencyVertex.Remove(adjacencyVertex.First(edge => edge.IncidentTo.Index == index));
                }
            }
            foreach (var vertexToDeleteIndex in verticesIndexes)
            {
                VerticesList.RemoveAt(vertexToDeleteIndex - 1);
            }
        }

        /// <summary>
        /// Алгоритм Крускала. Сложность 0(Е lg V).
        /// </summary>
        public int MinimumSpanningTreeKruskal()
        {
            foreach (var curVertex in VerticesList)
            {
                foreach (var incidentEdge in curVertex.AdjacencyList)
                {
                    if (curVertex.Index < incidentEdge.IncidentTo.Index)
                        EdgesList.Add(new Edge(curVertex, incidentEdge.IncidentTo, incidentEdge.Weight));
                }
            }

            EdgesList.Sort((first, second) => first.Weight.CompareTo(second.Weight));

            int totalWeight = 0;



            return totalWeight;
        }

        public List<Edge> GetMinimumSpanningTreeKruskal()
        {
            List<Edge> minimumSpanningTree = new List<Edge>();
            foreach (var edge in EdgesList)
            {
                if (edge.InTree)
                    minimumSpanningTree.Add(edge);
            }

            return minimumSpanningTree;
        }

        public int MinimumSpanningTreePrim() // алгоритм Прима
        {
            Vertex source = VerticesList[0];
            source.Key = 0;

            List<Vertex> verticesToAddInTree = new List<Vertex>(VerticesList);

            int totalWeight = 0;

            while (verticesToAddInTree.Count != 0)
            {
                Vertex curVertex = ExtractMin(verticesToAddInTree);
                totalWeight += curVertex.Key;
                foreach (var incidentEdge in curVertex.AdjacencyList)
                {
                    if (!incidentEdge.IncidentTo.Discovered && incidentEdge.Weight < incidentEdge.IncidentTo.Key) //verticesToAddInTree.Contains(incidentEdge.IncidentTo)
                    {
                        incidentEdge.IncidentTo.Parent = curVertex;
                        incidentEdge.IncidentTo.Key = incidentEdge.Weight;
                    }
                }
            }

            return totalWeight;
        }

        private Vertex ExtractMin(List<Vertex> vertices)
        {
            Vertex minVertex = vertices[0];
            int min = minVertex.Key;

            foreach (var vertex in vertices)
            {
                if (vertex.Key < min)
                {
                    min = vertex.Key;
                    minVertex = vertex;
                }
            }

            minVertex.Discovered = true;
            vertices.Remove(minVertex);
            return minVertex;
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

