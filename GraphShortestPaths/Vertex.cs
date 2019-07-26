using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphShortestPaths
{
    public class Vertex
    {
        public int Index { get; }
        public int Distance { get; set; }
        public Vertex Parent { get; set; }

        public bool Discovered { get; set; }
        public int DiscoveryTime { get; set; }
        public int FinishingTime { get; set; }

        public List<IncidentEdge> AdjacencyList { get; }

        public Vertex(int index)
        {
            Index = index;
            Parent = null;
            Distance = int.MaxValue;
            AdjacencyList = new List<IncidentEdge>();
        }
    }
}
