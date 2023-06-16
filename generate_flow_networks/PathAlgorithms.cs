using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlowNetworks;

/// <summary>
///     Stategy Pattern for Path Algorithms on Networks
/// </summary>
internal interface PathAlgorithm
{
    void FindPathTree(Network network);
}

public class PathAlgorithms
{
    internal static readonly PathAlgorithm LabelSetting = new LabelSettingStrategy();
    internal static readonly PathAlgorithm LabelSettingPrio = new LabelSettingPriorityQueueStrategy();
    internal static readonly PathAlgorithm LabelCorrection = new LabelCorrectingStrategy();
    internal static readonly PathAlgorithm[] All = { LabelSetting, LabelSettingPrio, LabelCorrection };


    private class LabelSettingStrategy : PathAlgorithm
    {
        public void FindPathTree(Network network)
        {
            int checks = 0, pops = 0;
            var queue = new List<Node>();

            queue.Add(network.StartNode);
            var visited = new HashSet<Node>();

            while (queue.Count > 0)
            {
                var u = queue.MinBy(n => n.TotalCost);
                checks += queue.Count;

                queue.Remove(u);
                visited.Add(u);
                pops++;

                foreach (var link in u.Links)
                {
                    var new_cost = u.TotalCost + link.Cost;
                    var v = link.ToNode;
                    if (!visited.Contains(v) && new_cost < v.TotalCost)
                    {
                        v.TotalCost = new_cost;
                        v.ShortestPathLink = link;
                        queue.Add(v);
                    }
                }
            }

            Debug.WriteLine("{0}: {1} checks, {2} pops", this, checks, pops);
        }

        public override string ToString()
        {
            return "Label Setting";
        }
    }

    private class LabelSettingPriorityQueueStrategy : PathAlgorithm
    {
        public void FindPathTree(Network network)
        {
            int checks = 0, pops = 0;
            var queue = new PriorityQueue<Node, double>();
            queue.Enqueue(network.StartNode, 0);

            var visited = new HashSet<Node>();

            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                visited.Add(u);
                pops++;

                foreach (var link in u.Links)
                {
                    var new_cost = u.TotalCost + link.Cost;
                    var v = link.ToNode;
                    if (!visited.Contains(v) && new_cost < v.TotalCost)
                    {
                        v.TotalCost = new_cost;
                        v.ShortestPathLink = link;
                        queue.Enqueue(v, v.TotalCost);
                    }
                }
            }

            Debug.WriteLine("{0}: {1} checks, {2} pops", this, checks, pops);
        }

        public override string ToString()
        {
            return "Label Setting (PriorityQueue)";
        }
    }

    private class LabelCorrectingStrategy : PathAlgorithm
    {
        public void FindPathTree(Network network)
        {
            int checks = 0, pops = 0;
            var queue = new Queue<Node>();
            queue.Enqueue(network.StartNode);

            while (queue.Any())
            {
                var u = queue.Dequeue();
                pops++;

                foreach (var link in u.Links)
                {
                    var new_cost = u.TotalCost + link.Cost;
                    var v = link.ToNode;
                    if (new_cost < v.TotalCost)
                    {
                        v.TotalCost = new_cost;
                        v.ShortestPathLink = link;
                        queue.Enqueue(v);
                    }
                }
            }

            Debug.WriteLine("{0}: {1} checks, {2} pops", this, checks, pops);
        }

        public override string ToString()
        {
            return "Label Correcting";
        }
    }
}