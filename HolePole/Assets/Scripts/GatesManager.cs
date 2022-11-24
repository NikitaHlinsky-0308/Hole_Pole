using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesManager : MonoBehaviour
{

    public static GatesManager instance;
    
    
    // closest gate
    public GateChanger gate;

    // closest enemy
    [SerializeField] private EnemyBehaviour enemy;
    
    private float _closestEnemyDist = 0;
    private EnemyBehaviour _closestEnemy;
    
    // test
    [SerializeField] private Transform player; 
    private GateChanger _closestGate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        StartCoroutine(Delay());
    }

    

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.startingDest = true;
        
        
    }
    
    
}
