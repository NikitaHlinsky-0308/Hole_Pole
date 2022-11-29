using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraImpact : MonoBehaviour
{

    public bool start = false;
    //public AnimationCurve shakingCurve;
    public float duration = 1f;
    public float magnitude = 0.2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //start = false;
            //StartCoroutine(nameof(Shaking));

            StartCoroutine(PlayCameraShakeAnimation(duration, magnitude));
        } 
    }
    
    public void Shaking()
    {
        StartCoroutine(PlayCameraShakeAnimation(duration, magnitude));
    }
    
    private IEnumerator PlayCameraShakeAnimation(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;
 
        while(elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
 
            transform.localPosition = new Vector3(x, y,originalPosition.z);
            elapsedTime += Time.deltaTime;
 
            yield return null;
        }
 
        transform.localPosition = originalPosition;
    }
}
