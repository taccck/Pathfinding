using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public float gCost = Mathf.Infinity;
    public float hCost = Mathf.Infinity;
    public Node parent; //the node that comes before it in a path, needed for backtracking 
    public Vector3 position;
    public int[] neighbours; //nodes that can be moved to from this node

    private Ray _toObstacle;

    private readonly Vector3 _relativePos;
    private readonly Vector3 _relativePosNoHeight;

    public float FCost => hCost + gCost;

    public Node(Vector3 obstaclePos, Vector3 position)
    {
        _relativePos = position;
        _relativePosNoHeight = new Vector3(position.x, 0f, position.z);
        this.position = _relativePos + obstaclePos;

        _toObstacle = new Ray(obstaclePos, _relativePosNoHeight);
    }

    public void SetNeighbours(Node[] grid, LayerMask mask)
    {
        //find nodes that you can move to from this node
        //could work large scale by limiting raycast by distance between nodes with a overlap sphere against obstacles 
        List<int> tempNeighbour = new List<int>();
        for (int i = 0; i < grid.Length; i++)
        {
            Node n = grid[i];
            if (n == this) continue;
            if (!Physics.Raycast(position, n.position - position, Vector3.Distance(position, n.position), mask)) 
                tempNeighbour.Add(i);
        }

        neighbours = tempNeighbour.ToArray();
    }

    public Vector3 GetPositionWithOffset(float r)
    {
        //lets game objects move to this node with an offset to not get stuck on obstacles
        float distanceFromOrigin = Vector3.Distance(Vector3.zero, _relativePosNoHeight);
        float offset = Mathf.Sqrt(Mathf.Pow(r, 2) * 2);
        return _toObstacle.GetPoint(distanceFromOrigin + offset);
    }
}