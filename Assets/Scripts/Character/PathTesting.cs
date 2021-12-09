using UnityEngine;

public class PathTesting : MonoBehaviour
{
    [SerializeField] private Transform goal;
    [SerializeField] private float r = 1f;
    [SerializeField] private float speed = 4f;

    private PathRequester _pathing;

    private void FixedUpdate()
    {
        Vector3? moveTo = _pathing.GetWalkPoint();
        if (moveTo == null) return;
        transform.Translate(((Vector3)moveTo - transform.position).normalized * Time.deltaTime * speed);
    }

    private void Awake()
    {
        _pathing = GetComponent<PathRequester>();
    }

    private void OnEnable()
    {
        _pathing.SetPath(transform.position, goal.position, r);
    }
}