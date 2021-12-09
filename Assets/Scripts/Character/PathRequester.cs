using System.Collections.Generic;
using UnityEngine;

public class PathRequester : MonoBehaviour
{
    private int _pathIndex = 1; //0 is start pos
    private Vector3[] _path;

    private const float _MIN_MOVE_DIST = .1f;

    public Vector3? GetWalkPoint()
    {
        if (_path == null) return null;

        if (CloseEnough(GetDistanceNoHeight(transform.position, _path[_pathIndex])))
            _pathIndex++; //if you are close enough to the current walk point
        if (_pathIndex < _path.Length) return _path[_pathIndex];

        _path = null;
        return null;
    }

    public void SetPath(Vector3 startPos, Vector3 endPos, float r)
    {
        if (CloseEnough(Vector3.Distance(startPos, endPos))) return; //if close and moving is unnecessary

        _pathIndex = 1;

        (Node start, Node end) = LevelPathfinding.current.GetStartAndEndNode(startPos, endPos, r);
        if (start == null || end == null) //no obstacle between
        {
            _path = new[] {startPos, endPos};
            return;
        }

        //art below
        List<Vector3> temp = new List<Vector3>(Pathfinding.FindPath(start, end, r, startPos, endPos));
        if (!Physics.SphereCast(temp[0], r, temp[2] - temp[0], out _,
            Vector3.Distance(temp[0], temp[2]), ~LayerMask.GetMask("Floor"))) 
            temp.RemoveAt(1);
        if (!Physics.SphereCast(temp[temp.Count - 1], r, temp[temp.Count - 3] - temp[temp.Count - 1], out _,
            Vector3.Distance(temp[temp.Count - 1], temp[temp.Count - 3]), ~LayerMask.GetMask("Floor")))
            temp.RemoveAt(temp.Count - 2);
        _path = temp.ToArray();
    }
    
    private bool CloseEnough(float distance) => distance < _MIN_MOVE_DIST;

    private float GetDistanceNoHeight(Vector3 a, Vector3 b)
    {
        (a.y, b.y) = (0, 0);
        return Vector3.Distance(a, b);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (_path != null)
            for (int i = 0; i < _path.Length; i++)
            {
                Gizmos.DrawSphere(_path[i], .2f);

                if (i - 1 >= 0)
                {
                    Gizmos.DrawLine(_path[i], _path[i - 1]);
                }
            }
    }
}