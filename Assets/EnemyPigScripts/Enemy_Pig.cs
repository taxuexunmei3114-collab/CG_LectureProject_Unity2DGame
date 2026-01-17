using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Pig : MonoBehaviour
{
    public float current_health = 2f;//血量系统
    public bool isDead = false;//是否处于死亡状态
    //受击无敌时间
    public bool isInvincible = false;
    public float invincibilityTime = 1f;
    //碰撞检测
    public bool isGround;
    public bool isWall;
    public bool isCliff;
    //方向
    public float facingdir = 1;
    public bool facingRight = true;

    // 射线检测变量
    protected RaycastHit2D _playerRaycastResult;
    public bool _isPlayerDetected;
    public RaycastHit2D PlayerRaycastResult => _playerRaycastResult; // 只读属性
    //组件
    public Animator anim;
    public Rigidbody2D rb;
    public AnimationController animationcontroller; // 添加动画控制器引用
    public GameObject DiamondPrefab;

    // 在Awake方法之前的变量声明区域添加
    public Transform currentPatrolTarget;// 当前巡逻目标点
    public bool hasPatrolRange = false;// 是否设置了巡逻范围

    [Header("Patrol Info")]
    [SerializeField] public Transform patrolPointA; // 巡逻点A
    [SerializeField] public Transform patrolPointB; // 巡逻点B

    [Header("MoveInfo")]
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] public float chaseSpeedMultiplier = 3f; // 追击速度倍率（可配置）

    [Header("playerDetection")]
    [SerializeField] protected float apDistance = 3F;
    [SerializeField] protected LayerMask whatisPlayer;
    [SerializeField] public float attackTriggerDistance = 0.5f; // 攻击触发距离（可配置）

    [Header("Collison info")]
    [SerializeField] protected Transform CliffCheck;
    [SerializeField] protected float cliffDis = 0.35f;
    [SerializeField] public bool isCliffCheckEnable=true;
    [Space]
    [SerializeField] protected float wallDis = 0.63f;
    [SerializeField] protected Transform WallCheck;
    [Space]
    [SerializeField] protected float groundDis = 0.63f;
    [SerializeField] protected Transform GroundCheck;
    [Space]
    [SerializeField] protected Transform DetectCheck;
    [Space]
    [SerializeField] protected LayerMask whatIsGround;

    #region State Components
    public EnemyStateMachine stateMachine { get; protected set; }
    public Enemy_Pig_IdleState idleState { get; protected set; }
    public Enemy_Pig_PatrolState patrolState { get; protected set; }
    public Enemy_Pig_ChaseState chaseState { get; protected set; }
    public Enemy_Pig_AttackState attackState { get; protected set; }
    public Enemy_Pig_HittedState hittedState { get; protected set; }
    public Enemy_Pig_DeadState deadState { get; protected set; }

    #endregion

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        // 初始化所有状态
        idleState = new Enemy_Pig_IdleState(stateMachine, this, "Pig_idle", EnemyAnimationType.Loop);
        patrolState = new Enemy_Pig_PatrolState(stateMachine, this, "Pig_move", EnemyAnimationType.Loop);
        chaseState = new Enemy_Pig_ChaseState(stateMachine, this, "Pig_move", EnemyAnimationType.Loop);
        attackState = new Enemy_Pig_AttackState(stateMachine, this, "Pig_attack", EnemyAnimationType.OneShot);
        hittedState = new Enemy_Pig_HittedState(stateMachine, this, "Pig_hitted", EnemyAnimationType.OneShot);
        deadState = new Enemy_Pig_DeadState(stateMachine, this, "Pig_dead", EnemyAnimationType.OneShot);

    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        anim = GetComponent<Animator>();
        animationcontroller = GetComponent<AnimationController>();
        // 检查是否设置了巡逻范围
        if (patrolPointA != null && patrolPointB != null)
        {
            hasPatrolRange = true;
            // 调整巡逻点的Y坐标与敌人一致
            currentPatrolTarget = patrolPointA; // 默认从A点开始
        }
        // 初始化状态机
        stateMachine.Initialized(idleState);
    }

    protected virtual void Update()
    {
        HealthControl();
        if (!isDead)
        {
            CollisionDetection();
            stateMachine.currentState.Update();
            FlipController();
        }
    }

    // 用于检测玩家的方法
    public virtual bool IsPlayerDetected()
    {
        return _isPlayerDetected;
    }

    // 获取与玩家的距离
    public float GetDistanceToPlayer()
    {
        if (_playerRaycastResult.collider != null)
        {
            return _playerRaycastResult.distance;
        }
        return float.MaxValue;
    }

    protected virtual void CollisionDetection()
    {
        bool isGrounded1, isGrounded2, isGrounded3;
        isGrounded1 = Physics2D.Raycast(GroundCheck.position, Vector2.down, groundDis, whatIsGround);
        isGrounded2 = Physics2D.Raycast(new Vector2(GroundCheck.position.x - 0.3f, GroundCheck.position.y), Vector2.down, groundDis, whatIsGround);
        isGrounded3 = Physics2D.Raycast(new Vector2(GroundCheck.position.x + 0.3f, GroundCheck.position.y), Vector2.down, groundDis, whatIsGround);
        isGround = isGrounded1 || isGrounded2 || isGrounded3;
        isCliff = Physics2D.Raycast(CliffCheck.position, Vector2.down, cliffDis, whatIsGround);
        isCliff = !isCliff;
        isWall = Physics2D.Raycast(WallCheck.position, Vector2.right * facingdir, wallDis, whatIsGround);

        _playerRaycastResult = Physics2D.Raycast(DetectCheck.position, Vector2.right * facingdir, apDistance, whatisPlayer);
        _isPlayerDetected = _playerRaycastResult.collider != null;
    }

    public void Flip()
    {
        facingdir = facingdir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController()
    {
        if (facingRight && rb.velocity.x < -0.01f)
        {
            Flip();
        }
        else if (!facingRight && rb.velocity.x > 0.01f)
        {
            Flip();
        }
    }

    // 从外部调用的受伤方法
    public virtual void getHurt(float damage_health_amount = 1f)
    {
        if (isInvincible || isDead) return;

        // 直接切换到受击状态
        stateMachine.ChangeState(hittedState);
        current_health-=damage_health_amount;
        StartCoroutine(StartInvincibility());
    }
    private IEnumerator StartInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }


    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(CliffCheck.position, new Vector3(CliffCheck.position.x, CliffCheck.position.y - cliffDis));
        Gizmos.color = Color.white;
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallDis * facingdir, WallCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(DetectCheck.position, new Vector3(DetectCheck.position.x + apDistance * facingdir, DetectCheck.position.y));
        Gizmos.color = Color.black;
        Gizmos.DrawLine(GroundCheck.position, new Vector3(GroundCheck.position.x, GroundCheck.position.y-groundDis));
    }
    public virtual void OnHittedAnimationEnd()
    {
        if(isDead) stateMachine.ChangeState(deadState);
        else stateMachine.ChangeState(idleState);

    }
    public virtual void OnDeadAnimationEnd()
    {
        OnDestroy();
        ChanceDrop();
    }
    protected virtual void OnDestroy()
    {
        gameObject.SetActive(false);
    }
    protected virtual void HealthControl()
    {
        if (current_health<=0)
        {
            isDead = true;
        }
    }

    public bool CanMoveInDirection(bool rightDirection)
    {
        if (hasPatrolRange)
        { 
            float currentX = transform.position.x;
            float minX = Mathf.Min(patrolPointA.position.x, patrolPointB.position.x);
            float maxX = Mathf.Max(patrolPointA.position.x, patrolPointB.position.x);

            if (rightDirection) // 想往右移动
            {
                return currentX < maxX;
            }
            else // 想往左移动
            {
                return currentX > minX;
            }
        }
        else
        {
            // 能移动 = 不是悬崖
            return !isCliff;
        }
    }

    public bool IsGrounded()
    {
        return isGround; // 你已有这个字段
    }

    //25%概率掉落恢复血量的钻石
    public virtual void ChanceDrop()
    {
        if(DiamondPrefab != null && Random.Range(0f, 100f) <= 25f)
        {

            GameObject gameObject = Instantiate(DiamondPrefab, rb.position, Quaternion.identity);
        }
    }
}