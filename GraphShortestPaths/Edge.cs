using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphShortestPaths
{
    public class IncidentEdge
    {
        public int Weight { get; set; }
        public Vertex IncidentTo { get; } // входит (конец)

        public IncidentEdge(Vertex incidentTo, int weight)
        {
            IncidentTo = incidentTo;
            Weight = weight;
        }
    }
}
