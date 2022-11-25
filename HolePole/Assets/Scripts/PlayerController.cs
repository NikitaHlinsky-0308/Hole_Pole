using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour, IPorgressable
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotateSpeed;
    public Vector3 moveDirection;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private FloatingJoystick joystick;

    [SerializeField] private Animator anim;
    [SerializeField] private CameraImpact cameraShake;
    [SerializeField] private VisualEffect runVFX;
    [SerializeField] private VisualEffect splashVFX;
    [SerializeField] private ParticleSystem punchVFX;

    // private float horizontal;
    // private float vertical;

    //private float _punchStrength;
    public float attackRange;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsGround;
    
    private bool _isUnderImpact;
    private float _punchStrength;
    private bool _enemyInAttackRange;
    private bool _isInAttackState;
    
    [Space(20)]
    
    [Header("Punch strength by level")]
    [SerializeField] private int[] playerPower = new int[3];
     
    // IProgressable impl

    public int CurrentLvl { get; set; }

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
    

    // Gate changer impl

    [SerializeField] private GameObject gateChanger;
    private GateChanger _gateChanger;
    private LevelIndicator _lvlIndicator;
    private void Start()
    {
        //_gateChanger = gateChanger.GetComponent<GateChanger>();
        _lvlIndicator = GetComponentInChildren<LevelIndicator>();
        CurrentLvl = 1;
        UpdateLevelStats(CurrentLvl);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        if (!_isUnderImpact)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            // float horizontal = joystick.Horizontal;
            // float vertical = joystick.Vertical;
            
            moveDirection = new Vector3(horizontal, 0.0f, vertical);
            moveDirection.Normalize();
            moveDirection *= speed;

            characterController.Move(moveDirection * Time.fixedDeltaTime);
            //transform.rotation = Quaternion.Euler(0f, Mathf.,0f);
        }

        if (IsEnemyClose())
        {
            anim.SetTrigger("Attack");
            
            // if (!_isInAttackState)
            // {   
            //     
            // }
        }

        if (moveDirection.magnitude >= 1)
        {
            anim.SetBool("Running", true);
            PlayRunVFX();
        } else if (moveDirection.magnitude == 0) anim.SetBool("Running", false);
        

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toRotation,
                rotateSpeed * Time.fixedDeltaTime
            );
        }

        if (!IsGrounded())
        {
            moveDirection.y += (-9.81f);
            characterController.Move(moveDirection * Time.fixedDeltaTime);
            PlaySplashVFX();
        }
        
        
        
    }

    private void PlayRunVFX()
    {
        //runVFX.Stop();
        runVFX.Play();
    }

    private void PlaySplashVFX()
    {
        splashVFX.Play();
    }

    public IEnumerator AttackedReaction(float enemyRot, float impactSpeed = 3, float time = .5f)
    {
        //StartCoroutine(AlreadyAttaked(2f));
        
        //задержка перед ударом врага
        // Debug.Log(_enemyInAttackRange + "Attack in process");
        
        //yield return new WaitForSeconds(0.35f);
        cameraShake.Shaking();
        
        _enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        // условие если после задержки мы ещё возле врага
        Debug.Log(_enemyInAttackRange);
        if (_enemyInAttackRange)
        {


            Vector3 pushDirection = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * enemyRot),
                0,
                Mathf.Cos(Mathf.Deg2Rad * enemyRot)) * speed;

            float startTime = Time.time; // need to remember this to know how long to dash
            while (Time.time < startTime + time)
            {
                _isUnderImpact = true;
                characterController.Move(pushDirection * impactSpeed * Time.deltaTime);
                yield return null;
                _isUnderImpact = false;
            }
        }
    }

    private IEnumerator AlreadyAttaked(float time)
    {
        _isInAttackState = true;
        yield return new WaitForSeconds(time);
        _isInAttackState = false;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 3.0f, whatIsGround)) return true;

        return false;
    }

    private bool IsEnemyClose()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, 1.6f, whatIsEnemy) && !_isInAttackState)
        {
            StartCoroutine(AlreadyAttaked(2));
            return true;
        }

        return false;
    }
    
    public void UpdateLevelStats(int currentLvl)        
    {
        switch (currentLvl)
        {
            case 1:
                // improve stats
                _punchStrength = playerPower[0];
                _lvlIndicator.ChangePrefab(0);
                break;
            
            case 2:
                _punchStrength = playerPower[1];
                _lvlIndicator.ChangePrefab(1);
                break;
            
            case 3:
                _punchStrength = playerPower[2];
                _lvlIndicator.ChangePrefab(2);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Old
            // Debug.Log("Plus Enemy ------------------");
            // EnemyBehaviour enemyBehaviour = other.gameObject.GetComponent<EnemyBehaviour>();
            // // change speed depend on lvl stats
            // enemyBehaviour.Attacked(transform.eulerAngles.y, _punchStrength);

            // New

            
            punchVFX.Play();
            EnemyBehaviour enemyBehaviour = other.gameObject.GetComponent<EnemyBehaviour>();
            enemyBehaviour.isBorting = true;
            enemyBehaviour.agent.enabled = false;
            enemyBehaviour.playerMoveDir = moveDirection;
            enemyBehaviour.playerStrength = _punchStrength;
            enemyBehaviour.StartCoroutine("borting");
            
            
        }

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

        // if (other.gameObject.CompareTag("Water"))
        // {
        //     Debug.Log(other.gameObject.layer.ToString());
        //     PlaySplashVFX();
        // }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}
