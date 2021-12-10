using UnityEngine;

public class PathTesting : MonoBehaviour
{
    [SerializeField] private Transform goal;
    [SerializeField] private float r = 1f;
    [SerializeField] private float speed = 4f;

    private Vector3? _walkPoint;
    private PathRequester _pathing;

    private void FixedUpdate()
    {
        if (_walkPoint == null) return;
        transform.Translate(((Vector3)_walkPoint - transform.position).normalized * Time.deltaTime * speed);
    }

    private void UpdatePath()
    {
        _walkPoint = _pathing.WalkPoint;
    }

    private void Awake()
    {
        _pathing = GetComponent<PathRequester>();
    }

    private void OnEnable()
    {
        _pathing.onPathUpdated += UpdatePath;
        _pathing.SetPath(transform.position, goal.position, r);
    }

    private void OnDisable()
    {
        _pathing.onPathUpdated -= UpdatePath;
    }
}