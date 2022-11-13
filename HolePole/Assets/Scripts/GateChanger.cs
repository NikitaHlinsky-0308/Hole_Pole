using System;
using System.Collections;
using UnityEngine;

public class GateChanger : MonoBehaviour
{

    [SerializeField] private GameObject plusGatePref;
    [SerializeField] private GameObject minusGatePref;
    [SerializeField] private Collider plusCollider;
    [SerializeField] private Collider minusCollider;
    
    [SerializeField] private bool activeIsPlus;
    private bool _isOnDelay = false;


    private void Start()
    {
        if (!activeIsPlus)
        {
            plusGatePref.SetActive(true);
            minusGatePref.SetActive(false);
        }
        else
        {
            plusGatePref.SetActive(false);
            minusGatePref.SetActive(true);
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         
    //         plusCollider.enabled = false;
    //         minusCollider.enabled = false;
    //         
    //         // if (plusCollider.enabled && minusCollider.enabled)
    //         // {
    //         //     plusCollider.enabled = false;
    //         //     minusCollider.enabled = false;
    //         // }
    //         // if (!plusCollider.enabled && !minusCollider.enabled)
    //         // {
    //         //     plusCollider.enabled = true;
    //         //     minusCollider.enabled = true;
    //         // }
    //         
    //     }
    // }

    private IEnumerator ChangeDelay(float time)
    {
        _isOnDelay = true;
        minusCollider.enabled = false;
        plusCollider.enabled = false;

        
        if (plusGatePref.gameObject.activeInHierarchy)
        {
            plusGatePref.SetActive(false);
            minusGatePref.SetActive(true);
        }
        else //if (minusGatePref.gameObject.activeInHierarchy)
        {
            minusGatePref.SetActive(false);
            plusGatePref.SetActive(true);
        }
        
        yield return new WaitForSeconds(time);
        
        _isOnDelay = false;
        plusCollider.enabled = true;
        minusCollider.enabled = true;
        
    }

    [ContextMenu(nameof(SetOppositeGate))]
    public void SetOppositeGate()
    {
        
        if (!_isOnDelay)
        {
            StartCoroutine(ChangeDelay(2f));
        }
    }
}
