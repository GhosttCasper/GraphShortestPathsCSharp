using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphShortestPaths
{
    public class Vertex
    {
        public int Index;
        public int Distance;
        public Vertex Parent;

        public bool Discovered;
        public int DiscoveryTime;
        public int FinishingTime;

        //public int Color;
        public int Key; // минимальный вес среди всех ребер, соединяющих v с вершиной в дереве.

        public List<IncidentEdge> AdjacencyList;

        public Vertex(int index)
        {
            Index = index;
            Parent = null;
            Distance = int.MaxValue;
            //Color = index - 1;
            Key = int.MaxValue;
            AdjacencyList = new List<IncidentEdge>();
        }
    }
}
