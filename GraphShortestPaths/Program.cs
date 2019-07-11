using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Кратчайшие пути между всеми парами вершин в графе (граф выбрать самому).
 */

namespace GraphShortestPaths
{
    class Program
    {
        private static void Main(string[] args)
        {
            Graph graph = ReadFileWithAdjacencyList("input.txt");
            graph.JohnsonAlgorithm();

            //if (!graph.IsEmpty())
            //    graph = ProcessGraph(graph);
            WriteFile(graph, "output.txt");
        }

        private static Graph ProcessGraph(Graph graph)
        {
            //graph.DagShortestPaths();
            //graph.BellmanFordAlgorithm();
            //graph.Dijkstra();
            string str= graph.PrintPredecessorSubgraph(graph.VerticesList[0]);
            Console.WriteLine(str);
            return graph;
        }

        private static void WriteFile(Graph inducedGraph, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                //string adjacencyMatrixStr = inducedGraph.OutputGraph();
                // writer.WriteLine(adjacencyMatrixStr);
            }
        }

        private static Graph ReadFileWithAdjacencyList(string fileName)
        {
            Graph graph;
            using (StreamReader reader = new StreamReader(fileName))
            {
                int size = ReadNumber(reader);
                string[] numbersStrs = new string[size];
                for (int i = 0; i < size; i++)
                {
                    numbersStrs[i] = reader.ReadLine();
                }
                graph = new Graph(size, numbersStrs);
            }
            return graph;
        }

        private static int ReadNumber(StreamReader reader)
        {
            var numberStr = reader.ReadLine();
            if (numberStr == null)
                throw new Exception("String is empty (ReadNumber)");
            var array = numberStr.Split();
            int number = int.Parse(array[0]);
            return number;
        }

        private static Graph ReadFileWithAdjacencyMatrix(string fileName)
        {
            Graph graph;
            using (StreamReader reader = new StreamReader(fileName))
            {
                int size = ReadNumber(reader);

                string[] numbersStrs = new string[size];
                for (int i = 0; i < size; i++)
                {
                    numbersStrs[i] = reader.ReadLine();
                }
                graph = new Graph(size, numbersStrs);
            }
            return graph;
        }

    }
}
