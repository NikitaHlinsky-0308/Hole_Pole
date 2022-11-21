using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerController;
    
    public LayerMask whatIsGround, whatIsPlayer;
    private float _punchStrength;

    public int health;
    //private int addittionHealth = 0;
    private bool healthAlreadySet;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;

    [SerializeField] private Animator anim;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, enemyAttacked, isGrounded, isBorting;
    public Vector3 playerMoveDir;
    public float playerStrength;
    private bool _isInAttackState;

    //public AnimationCurve AttackedCurve;
    public bool playerAttack { get; set; }

    [Space(20)]
    [Header("Punch strength by level")]
    [SerializeField] private float[] playerPower = new float[3];
    
    // IProgressable impl
    
    public int CurrentLvl { get; set; }


    [ContextMenu(nameof(TestMethod))]
    public void TestMethod()
    {
        IncreaseLvl();
        UpdateLevelStats(CurrentLvl);
    }
    public void IncreaseLvl()
    {
        if (CurrentLvl < 3 && CurrentLvl >= 1)
        {
            CurrentLvl += 1;
        }
        
    }

    public void DecreaseLvl()
    {
        if (CurrentLvl <= 3 && CurrentLvl > 1)
        {
            CurrentLvl -= 1;
        }
    }
    
    [SerializeField] private GameObject gateChanger;
    private GateChanger _gateChanger;
    public bool startingDest = false;
    
    
    private void Awake()
        {
            player = GameObject.FindWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            //EnemyManager.instance.enemies.Add(this);
            //health = GetComponent<Life>();
            //health = new Life(() => { /*Death*/});
        }
    private void Start()
    {
        _gateChanger = gateChanger.GetComponent<GateChanger>();
        CurrentLvl = 1;
        UpdateLevelStats(CurrentLvl);
    }

    private void Update()
    {
        if (isBorting)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            transform.Translate( playerMoveDir * playerStrength * Time.deltaTime, Space.World);
            
            //  raycast for ground check 
            if (Physics.Raycast(ray, 3.0f, whatIsGround))
            {
                // if we hit nothing, do gravity and die s
                Debug.Log("Did Hit Ground");
                isGrounded = true;
            }
            else
            {
                Debug.Log("Doesn't hit the ground");
                // apply gravity here
                // create instance for gravityScaler
                transform.Translate( new Vector3(0, -9.81f, 0) * Time.deltaTime, Space.World);
                isGrounded = false;
            }
        }
        if (agent.enabled)
        {


            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if ( startingDest) StartingDestination();
            if (!playerInSightRange && !playerInAttackRange && !startingDest) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (IsEnemyClose())
            {
                anim.SetTrigger("Attack");
            }
            
            //if (playerInAttackRange && playerInSightRange) AttackPlayer();
            
            
            
            
            // if (playerAttack)
            // {
            //     // use Attaked and save moveDir once 
            //     // call attacked once per time
            //     if (!enemyAttacked && playerController.moveDirection.magnitude > 1f)
            //     {
            //         //Attacked(playerController.moveDirection, 2);
            //         Attacked(player.transform.eulerAngles.y, _punchStrength);
            //         enemyAttacked = true;
            //         Invoke(nameof(ResetAttacked), 1.2f);
            //     }
            //
            // }

            // if (isInHole)
            // {
            //
            // }


            //Debug.Log(player.transform.eulerAngles.y);
            //WaveHealthIncrease();
        }
    }

    [ContextMenu(nameof(StartingDestination))]
    public void StartingDestination()
    {
        anim.SetBool("Running", true);
        agent.SetDestination(GatesManager.instance.gate.transform.position);
    }
    
    private void Patroling()
    {
        Debug.Log("start patroling");
        anim.SetBool("Running", true);
        
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        anim.SetBool("Running", true);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //playerAttack = true;
        
        
        //Make sure enemy doesn't move
        //Attacked(playerController.moveDirection, 2);

        // agent.SetDestination(transform.position);
        //
        // transform.LookAt(player);
        //
        // if (!alreadyAttacked)
        // {
        //     ///Attack code here
        //     // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        //     // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        //     // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
        //     ///End of attack code
        //
        //     
        //     
        //     Debug.Log("attack");
        //     alreadyAttacked = true;
        //     Invoke(nameof(ResetAttack), timeBetweenAttacks);
        // }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ResetAttacked()
    {
        enemyAttacked = false;
    }

    public void Attacked(float characterRot, float speed)
    {
        if (!enemyAttacked && playerController.moveDirection.magnitude > 1f)
        {
            agent.updateRotation = false;
    
            agent.velocity = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * characterRot),
                0,
                Mathf.Cos(Mathf.Deg2Rad * characterRot)) * speed;
    
            StartCoroutine(borting());
            //Attacked(player.transform.eulerAngles.y, 9);
            enemyAttacked = true;
            Invoke(nameof(ResetAttacked), 1.2f);
        }
    }
    
    // public void Attacked(Vector3 moveDir, float speed)
    // {
    //     if (!enemyAttacked && playerController.moveDirection.magnitude > 1f)
    //     {
    //         //float pastY = transform.position.y;
    //         agent.enabled = false;
    //         //agent.updateRotation = false;
    //         
    //         transform.Translate(moveDir * 50f * Time.deltaTime, Space.World);
    //
    //         isBorting = true;
    //         StartCoroutine(borting());
    //         //Attacked(player.transform.eulerAngles.y, 9);
    //         enemyAttacked = true;
    //         Invoke(nameof(ResetAttacked), 1.2f);
    //     }
    // }

    private IEnumerator borting()
    {
        // Old
        // yield return new WaitForSeconds(0.3f);
        //agent.updateRotation = true;
        // playerAttack = false; 
        
        // New 
        
        yield return new WaitForSeconds(1.5f);
        isBorting = false; 
        playerAttack = false; 
        
        if (isGrounded)
        {
            agent.enabled = true;
        }
        else
        {
            StartCoroutine(DestroyEnemy(1.1f));
        }
    }
    
    private IEnumerator AlreadyAttaked(float time)
    {
        _isInAttackState = true;
        yield return new WaitForSeconds(time);
        _isInAttackState = false;
    }

    private IEnumerator DestroyEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //EnemyManager.instance.enemies.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GatesPlus"))
        {
             _gateChanger = other.GetComponentInParent<GateChanger>();
             _gateChanger.SetOppositeGate();
             IncreaseLvl();
             UpdateLevelStats(CurrentLvl);
             //Debug.Log(CurrentLvl);
        }
        if (other.gameObject.CompareTag("GatesMinus"))
        {
            _gateChanger = other.GetComponentInParent<GateChanger>();
            _gateChanger.SetOppositeGate();
             DecreaseLvl();
             UpdateLevelStats(CurrentLvl);
             //Debug.Log(CurrentLvl);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(playerController.AttackedReaction(
                transform.eulerAngles.y, 
                _punchStrength));
        }
        
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     agent.enabled = false;
        // }
        
        
    }
    
    private bool IsEnemyClose()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, 1.6f, whatIsPlayer) && !_isInAttackState)
        {
            StartCoroutine(AlreadyAttaked(2));
            return true;
        }

        return false;
    }
    
    public void UpdateLevelStats(int currentLvl)
    {
        startingDest = false;
        switch (currentLvl)
        {
            case 1:
                // improve stats
                _punchStrength = playerPower[0];
                break;
            
            case 2:
                _punchStrength = playerPower[1];
                break;
            
            case 3:
                _punchStrength = playerPower[2];
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }

    public int Health
    {
        get
        {
            return health;
        }
        set 
        {
            health = value;
        }
    }

}
