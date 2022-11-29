using UnityEngine;

public class Lerping : MonoBehaviour
{
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 0, 5f), Time.deltaTime);
    }
}
