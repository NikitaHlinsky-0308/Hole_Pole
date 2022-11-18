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
    
    // call setOppositeGate on triggerEnter

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("gate works");

        if (other.CompareTag("Player"))
        {
            SetOppositeGate();
        }
    }
}
