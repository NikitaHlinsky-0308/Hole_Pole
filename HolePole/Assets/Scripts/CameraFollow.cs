using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    
    [Range(1.0f, 5.0f)]
    [SerializeField] private float smoothness = 1f;

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 nextPosition = Vector3.Lerp(
            transform.position,
            target.position + offset,
            Time.fixedDeltaTime * smoothness
        );

        transform.position = nextPosition;
    }
}
