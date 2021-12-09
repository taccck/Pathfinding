using System.Collections.Generic;
using UnityEngine;

public class LevelPathfinding : MonoBehaviour
{
    public static LevelPathfinding current;

    [SerializeField] private bool updateNodes = true; //contains only the obstacle layer
    [SerializeField] private LayerMask levelMaks; //contains all layers that can collide with the player
    [SerializeField] private LayerMask obstacleMask; //contains only the obstacle layer

    public Node[] Grid { get; private set; }

    public bool FatCheck(Vector3 startPos, Vector3 endPos, float r) =>
        ObstacleRaycast(startPos, endPos, r, levelMaks).hit;

    public (Node, Node) GetStartAndEndNode(Vector3 startPos, Vector3 endPos, float r)
    {
        Node start = GetNodeBetween(startPos, endPos, r);
        Node end = GetNodeBetween(endPos, startPos, r);
        return (start, end);
    }

    public void ResetNodes(IEnumerable<Node> openSet, IEnumerable<Node> closedSet)
    {
        foreach (Node n in openSet)
        {
            n.gCost = Mathf.Infinity;
            n.hCost = Mathf.Infinity;
        }

        foreach (Node n in closedSet)
        {
            n.gCost = Mathf.Infinity;
            n.hCost = Mathf.Infinity;
        }
    }

    private void Awake()
    {
        if (current != null)
            Debug.LogError("More than one level pathfinding in scene!");
        current = this;
    }

    private void Start()
    {
        GetAllNodes();
        FindNeighbours();
    }

    private void FindNeighbours()
    {
        if (!updateNodes && SaveNodes.FileExists)
        {
            NodeData loadData = SaveNodes.Load();
            //might need to reconstruct the grid set
            for (int i = 0; i < Grid.Length; i++)
                Grid[i].neighbours = loadData.data[i];
            return;
        }

        foreach (Node n in Grid)
            n.SetNeighbours(Grid, levelMaks); //set all neighbours

        int[][] gridData = new int[Grid.Length][];
        for (int i = 0; i < Grid.Length; i++)
            gridData[i] = Grid[i].neighbours;
        NodeData saveData = new NodeData {data = gridData};
        SaveNodes.Save(saveData);
    }

    private void GetAllNodes()
    {
        List<Node> nodes = new List<Node>();
        foreach (PathObstacle po in GetComponentsInChildren<PathObstacle>())
        {
            nodes.AddRange(po.nodes);
        }

        Grid = nodes.ToArray();
    }

    private Node GetNodeBetween(Vector3 startPos, Vector3 endPos, float r)
    {
        (RaycastHit rayHit, bool hit) = ObstacleRaycast(startPos, endPos, r, obstacleMask);
        if (!hit) return null;
        return GetClosestNode(rayHit.transform.GetComponent<PathObstacle>().nodes, startPos, r, rayHit.transform.name);
    }

    private Node GetClosestNode(Node[] nodes, Vector3 position, float r, string obstacleName)
    {
        Node currNode = null;
        float closestNode = Mathf.Infinity;
        foreach (Node n in nodes) //set current node to the closest node from the object blocking the raycast
        {
            float currDistance = Vector3.Distance(position, n.position);
            (_, bool hit) = ObstacleRaycast(position, n.GetPositionWithOffset(r), r, levelMaks);
            if (currDistance >= closestNode || hit)
                continue;

            currNode = n;
            closestNode = currDistance;
        }

        if (currNode == null)
        {
            Debug.LogError($"{obstacleName} did not have nodes, or its nodes are blocked");
            return null;
        }

        return currNode;
    }

    private (RaycastHit rayHit, bool hit) ObstacleRaycast(Vector3 start, Vector3 end, float r, LayerMask mask)
    {
        bool hit = Physics.SphereCast(start, r, end - start, out RaycastHit raycastHit,
            Vector3.Distance(start, end) + .1f, mask);
        return (raycastHit, hit);
    }
}