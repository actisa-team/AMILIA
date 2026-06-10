using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;


namespace nameDijkstra
{
    /// <summary>
    /// Processes the node iteraction to find the path with the lowest value.
    /// The value is deterninated by a delegated function that provided the start and the end node must give the value between then.
    /// The nodes are indexed. The start and end nodes should be represented by indexes from a list of all possible nodes.
    /// The main objective of the this class is to be as simple and clean as possible but in this solution some extra coding was done to help clarity.
    /// </summary>
    /// <remarks>
    /// Author: André Pires Ferreira Vianna
    /// </remarks>
    public class oDijkstra
    {
        private int _startNode = 0;
        private List<Destination> _nodes;
        private Func<int, int, decimal> _values;

        /// <summary>
        /// Number of existing of nodes.
        /// </summary>
        public int NumberOfNodes { get; private set; }

        /// <summary>
        /// Indicates if the solver runned all the way to the end or stopped in an intermediated iteraction.
        /// </summary>
        public bool IsSolved { get { return !_nodes.Any(n => !n.Done); } }

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        /// <param name="numberOfNodes">Maximum number of nodes in the set (Must be positive).</param>
        /// <param name="values">Delegated function that gives the 'weight' between two nodes</param>
        public oDijkstra(int numberOfNodes, Func<int, int, decimal> values)
        {
            if (numberOfNodes < 0) throw new ArgumentException("The number of nodes must be positive.");
            if (values == null) throw new ArgumentNullException("values");

            NumberOfNodes = numberOfNodes;
            _values = values;
        }

        /// <summary>
        /// Resets the solver engine.
        /// </summary>
        public void Reset()
        {
            if (_nodes == null) _nodes = new List<Destination>();
            _nodes.Clear();
            for (var node = 0; node < NumberOfNodes; node++)
            {
                _nodes.Add(new Destination(node, node == _startNode));
                var value = _values(_startNode, node);
                if (value != -1) GetNode(node).AddStep(_startNode, value);
            }
        }

        /// <summary>
        /// Just to make the code cleaner.
        /// Does not need exception treatment since it should only used internally.
        /// </summary>
        /// <param name="n">The node index.</param>
        /// <returns>The node object.</returns>
        private Destination GetNode(int n)
        {
            return _nodes.SingleOrDefault(i => i.Node == n);
        }

        /// <summary>
        /// Main loop of the Djikstra engine.
        /// That loop does not use arrays or fixed sized lists to reduce memory consuption allowing larger node sets.
        /// </summary>
        private void Iteraction()
        {
            var source = (from r in _nodes
                          where !r.Done
                             && r.Path.Any()
                          orderby r.Path.Sum(p => p.Value)
                          select r).First();

            var connectedNodes = (from r in _nodes
                                  where r.Node != source.Node
                                     && r.Node != _startNode
                                     && _values(source.Node, r.Node) != -1
                                     && (!r.Path.Any() || r.Path.Sum(v => v.Value) > (source.Path.Sum(p => p.Value) + _values(source.Node, r.Node)))
                                  select new { node = r, newValue = _values(source.Node, r.Node) }).ToList();

            connectedNodes.ForEach(i =>
            {
                i.node.SetPath(source.Path);
                i.node.AddStep(source.Node, i.newValue);
            });

            source.Done = true;
        }

        /// <summary>
        /// Solves the Djikstra engine. 
        /// </summary>
        /// <param name="startNode">The node where we want to start the 'walk' through the path.</param>
        /// <param name="endNode">The node where we want to finish the 'walk' through the path.</param>
        /// <returns>A list of node indexes that form the path from the resquested start node to the requested end node.</returns>
        public IEnumerable<int> Solve(int startNode, int endNode)
        {
            if (startNode < 0 || startNode >= NumberOfNodes) throw new ArgumentException("The start node must be between zero and the number of nodes");
            if (endNode < 0 || endNode >= NumberOfNodes) throw new ArgumentException("The end node must be between zero and the number of nodes");

            _startNode = startNode;
            Reset();


            for (int i = 1; i < NumberOfNodes; i++)
            {
                Iteraction();
            }


            GetNode(endNode).AddStep(endNode, 0);


            return GetNode(endNode).Path.Select(p => p.Node);
        }

        /// <summary>
        /// Internal class just to make the code cleaner handling the destination info.
        /// </summary>
        private class Destination
        {
            public Destination(int n, bool d)
            {
                Node = n;
                Path = new List<Origin>();
                Done = d;
            }

            public int Node { get; private set; }
            public List<Origin> Path { get; private set; }
            public bool Done { get; internal set; }

            public void AddStep(int d, decimal v)
            {
                Path.Add(new Origin(d, v));
            }

            public void SetPath(List<Origin> p)
            {
                Path = new List<Origin>(p);
            }
        }

        /// <summary>
        /// Internal class just to make the code cleaner handling the origin info.
        /// </summary>
        private class Origin
        {
            public Origin(int n, decimal v)
            {
                Node = n;
                Value = v;
            }

            public int Node { get; private set; }
            public decimal Value { get; internal set; }
        }
    }
}
