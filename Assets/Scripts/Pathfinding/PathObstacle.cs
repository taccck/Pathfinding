using UnityEngine;

public class PathObstacle : MonoBehaviour
{
    public Node[] nodes;

    [SerializeField] private Vector3[] nodePositions;

    private void Awake()
    {
        nodes = new Node[nodePositions.Length];
        for (int i = 0; i < nodePositions.Length; i++)
        {
            nodes[i] = new Node(transform.position, nodePositions[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 p in nodePositions)
        {
            Gizmos.DrawSphere(p + transform.position, .2f);
        }
    }
}