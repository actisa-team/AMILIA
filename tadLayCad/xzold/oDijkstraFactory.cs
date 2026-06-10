using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace nameDijkstra
{
    public class oDijkstraFactory
    {

        private IList<NodeData> _nodeTable;
        private IList<LinkData> _linkTable;



        public oDijkstraFactory(IList<NodeData> iListNode, IList<LinkData> iListLink)
        {

            _nodeTable = iListNode;

            _linkTable = iListLink;

            oDijkstra _distanceSolver;

            //Resolver por distancia
            _distanceSolver = new oDijkstra(_nodeTable.Count, GetDistance);



            var solution = _distanceSolver.Solve(0, 1);

            var path = solution.Aggregate("", (s, n) => { return (s == "" ? "" : s + "->") + n; });
            var segments = solution.Zip(solution.Skip(1), (a, b) => Tuple.Create<int, int>(a, b));
            var distance = segments.Select(s => GetDistance(s.Item1, s.Item2)).Sum();


            MessageBox.Show(path.ToString() + " ; Dist " + distance.ToString());



        }

        public decimal GetDistance(int start, int end)
        {
            return (from n in _linkTable
                    where n.StartId == start
                    && n.EndId == end
                    select n.Distance).DefaultIfEmpty(-1).Single();
        }

    }


    public class NodePos
    {

        public NodePos(double iX, double iY)
        {

            mX = iX;
            mY = iY;

        }

        public double mX { get; set; }
        public double mY { get; set; }

    }
    public class NodeData
    {
        public NodeData(int n, double x, double y)
        {
            NodeId = n;
            Place = new NodePos(x, y);
        }

        public int NodeId { get; private set; }
        public NodePos Place { get; private set; }
    }
    public class LinkData
    {
        public LinkData(int iP1Id, int iP2Id, decimal iDist)
        {
            StartId = iP1Id;
            EndId = iP2Id;
            Distance = iDist;
        }

        public int StartId { get; private set; }
        public int EndId { get; private set; }
        public decimal Distance { get; private set; }
    }
}
