using UnityEngine;

public abstract class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isSmooth = true;
    [SerializeField] private float smoothness = 1f;

    protected private void Move( float deltaTime)
    {
        if (isSmooth)
        {
            Vector3 nextPosition = Vector3.Lerp(
                transform.position,
                target.position + offset,
                deltaTime * smoothness
            );

            transform.position = nextPosition;
        } else if (!isSmooth)
        {
            transform.position = target.position + offset;
        }
        
    }
}
