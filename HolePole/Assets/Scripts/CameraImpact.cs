using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraImpact : MonoBehaviour
{

    public bool start = false;
    public AnimationCurve shakingCurve;
    public float duration = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //start = false;
            //StartCoroutine(nameof(Shaking));

            StartCoroutine(PlayCameraShakeAnimation(duration, .1f));
            //Debug.Log("E key is pressed");
        } 
    }
    
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = shakingCurve.Evaluate(elapsedTime / duration);
            this.transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null; 
        }

        transform.position = startPosition;
    }
    
    public IEnumerator PlayCameraShakeAnimation(float duration, float magnitude)
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
