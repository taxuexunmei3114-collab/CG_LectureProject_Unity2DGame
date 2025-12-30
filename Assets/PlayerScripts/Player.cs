using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    #region Static Reference
    public static Player Instance { get; private set; }
    #endregion

    #region components
    public Animator anim;
    public Rigidbody2D rb;
    public AnimationController animationcontroller;
    public PlayerInputHandler playerInputHandler;
    public UIcontrol ui;
    #endregion

    #region State
    public PlayerStateMachine stateMachine { get; private set; }
    public playerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerHittedState hittedState { get; private set; }
    public PlayerDeadState deadState {  get; private set; }

    public PlayerDoorInState doorInState { get; private set; }

    public PlayerDoorOutState doorOutState { get; private set; }

    public HealthControl healthControl;

    public bool isInvincible = false;
    public float invincibilityTime = 1.3f;
    public bool isDead = false;
    public bool isGodMode = false;


    private float health;
    private float maxHealth;

    #endregion

    #region State info
    [Header("MoveInfo")]
    public float moveSpeed;
    public float jumpForce;
    [Space]
    [Header("Air Movement")]
    [Range(0.3f, 1.0f)]
    public float jumpMoveMultiplier = 0.8f; // 跳跃上升时的移动速度倍数
    [Range(0.3f, 1.0f)]
    public float fallMoveMultiplier = 0.8f;  // 下落时的移动速度倍数

    protected float facingdir = 1;//朝向
    protected bool facingRight = true;//是否朝向右侧

    [Header("Collison info")]
    [SerializeField] protected Transform GroundCheck;
    [SerializeField] protected float groundDis;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected bool isGrounded;
    [Space]
    [SerializeField] protected float wallDis;
    [SerializeField] protected Transform WallCheck;


    protected bool isGround;
    protected bool isWall;
    public bool is2JumpEnable = false;
    public bool is2jump = false;

    [Header("Attack")]
    [SerializeField] public bool isAttack;
    [SerializeField] public float deathDelay = 2f; // 死亡动画播完后，等待多久销毁
    [Space]
    [Header("Hit System")]
    [Range(0.1f, 2.0f)]
    public float hitDuration = 0.5f;           // 受击持续时间
    [Range(0f, 20f)]
    public float hitForceX = 5f;              // 受击击退X轴力度
    [Range(0f, 20f)]
    public float hitForceY = 3f;              // 受击击退Y轴力度

    #endregion

    private void Awake()
    {

        stateMachine = new PlayerStateMachine();

        idleState = new playerIdleState(stateMachine, this, "Player_idle",AnimationType.Loop,playerInputHandler);
        moveState = new PlayerMoveState(stateMachine, this, "Player_move", AnimationType.Loop, playerInputHandler);
        jumpState = new PlayerJumpState(stateMachine, this, "Player_jump", AnimationType.Loop, playerInputHandler);
        airState = new PlayerAirState(stateMachine, this, "Player_fall", AnimationType.Loop, playerInputHandler);
        attackState = new PlayerAttackState(stateMachine, this, "Player_attack", AnimationType.OneShot, playerInputHandler);
        hittedState = new PlayerHittedState(stateMachine, this, "Player_hitted", AnimationType.OneShot, playerInputHandler);
        deadState = new PlayerDeadState(stateMachine, this, "Player_dead", AnimationType.OneShot, playerInputHandler);
        doorInState = new PlayerDoorInState(stateMachine, this, "Player_doorIn", AnimationType.OneShot, playerInputHandler);
        doorOutState = new PlayerDoorOutState(stateMachine, this, "Player_doorOut", AnimationType.OneShot, playerInputHandler);
    }
    protected void Start()
    {
        // 设置静态实例引用
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        anim = GetComponentInChildren<Animator>();
        stateMachine.Initialized(idleState);//初始设置为idle
        health = healthControl.health;
        maxHealth = healthControl.maxHealth;
    }
    protected void Update()
    {
        healthControl.health = health;
        stateMachine.currentState.Update();
        FlipController();
    }

    public void setVelocity(float xVelocity, float yVelocity)
    {
        Vector2 newVelocity = new Vector2(xVelocity, yVelocity);
        rb.velocity = newVelocity;
    }

    public bool isGroundDetected()
    {
        isGrounded = Physics2D.Raycast(GroundCheck.position, Vector2.up, groundDis, whatIsGround);//改为了从下往上的射线检测
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(GroundCheck.position, new Vector3(GroundCheck.position.x, GroundCheck.position.y + groundDis));
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallDis * facingdir, WallCheck.position.y));
    }

    protected void Flip()
    {
        facingdir = facingdir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController()
    {
        if (facingRight && Input.GetAxisRaw("Horizontal")==-1)
        {
            Flip();
        }
        else if (!facingRight && Input.GetAxisRaw("Horizontal")==1)
        {
            Flip();
        }
    }
    public void setGodMode(bool isenable)
    {
        if (isenable) 
            isGodMode=true;
        else isGodMode = false;
    }

    public void getHitted(float damage_health=1f)
    {
        if (isGodMode) return;
        if (isInvincible || isDead) return;
        stateMachine.ChangeState(hittedState);
        health -= damage_health;
        StartCoroutine(StartInvincibility());

    }

    public void Heal(float increase_health=1f)
    {
        health += increase_health;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    private IEnumerator StartInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
        ui.GameOver();
    }

    public void OnHittedAnimationEnd()
    {
        if (isDead)
        {
            stateMachine.ChangeState(deadState);
            return;
        }

        if (isGroundDetected())
        {
            stateMachine.ChangeState(idleState);
        }
        else
        {
            stateMachine.ChangeState(airState);
        }
    }
    public void OnDeadAnimationEnd()
    {
        OnDestroy();
    }

    public void DoorIn(Transform t1,Transform t2=null)
    {
        if (t2)
        {
            if (stateMachine.currentState != doorInState)
            {
                rb.transform.position = new Vector3(t1.position.x, t1.position.y, t1.position.z);
                stateMachine.ChangeState(doorInState);
            }
            StartCoroutine(DelayedTeleportToExit(t2,0.7f));
        }
        else
        {
            if (stateMachine.currentState != doorInState)
            {
                rb.transform.position = new Vector3(t1.position.x, t1.position.y, t1.position.z);
                stateMachine.ChangeState(doorInState);
            }
            StartCoroutine(DelayOnDestroy(0.7f));
        }
    }
    private IEnumerator DelayedTeleportToExit(Transform exitPoint,float time)
    {
        // 等待一段时间
        yield return new WaitForSeconds(time);
        // 传送到出口位置
        rb.position = exitPoint.position;
    }

    private IEnumerator DelayOnDestroy(float time)
    {
        // 等待一段时间
        yield return new WaitForSeconds(time);
        OnDestroy();
    }

    public void Set2JumpEnable(bool value)
    {
        is2JumpEnable = value;
    }

}