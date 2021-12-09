using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    public static Vector3[] FindPath(Node start, Node end, float r, Vector3 startPos, Vector3 endPos)
    {
        //returns a walkable path between start and end
        start.gCost = 0;
        Node currNode = start;
        Vector3 currPos = currNode.position;
        SortedNodeSet openSet = new SortedNodeSet(); //all nodes to consider as current node
        List<Node> closedSet = new List<Node>(); //all nodes that have been the current node
        while (currNode != end)
        {
            foreach (int i in currNode.neighbours)
            {
                Node n = LevelPathfinding.current.Grid[i];
                if (closedSet.Contains(n)) continue;
                
                if (LevelPathfinding.current.FatCheck(n.GetPositionWithOffset(r), currNode.GetPositionWithOffset(r), r))
                    continue;
                
                float hCost = Vector3.Distance(n.position, end.position);
                float gCost = currNode.gCost + Vector3.Distance(currPos, n.position);
                if (hCost + gCost >= n.FCost) continue;
                
                n.hCost = hCost;
                n.gCost = gCost;
                n.parent = currNode;
                openSet.Add(n);
            }

            if (openSet.Count == 0)
            {
                Debug.LogWarning("End node has no connection to start node,\n" +
                                 "or entity is too fat for any path there");
                return null;
            }

            currNode = openSet[0];

            currPos = currNode.position;
            closedSet.Add(currNode);
            openSet.Remove(currNode);
        }

        LevelPathfinding.current.ResetNodes(openSet, closedSet);

        return RetraceSteps();

        Vector3[] RetraceSteps()
        {
            List<Vector3> path = new List<Vector3> {endPos};
            while (currNode != start)
            {
                Vector3 nodePos = currNode.GetPositionWithOffset(r);
                path.Add(nodePos);
                currNode = currNode.parent;
            }

            path.Add(start.GetPositionWithOffset(r));
            path.Add(startPos);

            path.Reverse();
            return path.ToArray();
        }
    }
}