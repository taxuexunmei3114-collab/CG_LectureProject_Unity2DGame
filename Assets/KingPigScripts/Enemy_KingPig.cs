// Enemy_KingPig.cs
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_KingPig : Enemy_Pig
{
    public UIcontrolr ui;
    [Header("King Pig - Teleport via Jump")]
    public Transform[] teleportPoints;
    private float minTeleportInterval = 8f;
    private float maxTeleportInterval = 14f;
    private float jump_vy = 5f;
    private float nextTeleportTime;
    private int currentTeleportIndex = 1;

    // 新增状态（必须）
    public Enemy_KingPig_JumpState jumpState { get; protected set; }
    public Enemy_KingPig_FallState fallState { get; protected set; }
    public Enemy_KingPig_GroundState groundState { get; protected set; }

    public bool isTeleporting;
    public Vector3 teleportTargetPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake(); // 初始化父类状态
        idleState = new Enemy_Pig_IdleState(stateMachine, this, "idle", EnemyAnimationType.Loop);
        patrolState = new Enemy_Pig_PatrolState(stateMachine, this, "run", EnemyAnimationType.Loop);
        chaseState = new Enemy_Pig_ChaseState(stateMachine, this, "run", EnemyAnimationType.Loop);
        attackState = new Enemy_Pig_AttackState(stateMachine, this, "attack", EnemyAnimationType.OneShot);
        hittedState = new Enemy_Pig_HittedState(stateMachine, this, "hitted", EnemyAnimationType.OneShot);
        deadState = new Enemy_Pig_DeadState(stateMachine, this, "dead", EnemyAnimationType.OneShot);
        // 创建 KingPig 专属状态（注意：动画名复用现有）
        jumpState = new Enemy_KingPig_JumpState(stateMachine, this, "jump", EnemyAnimationType.Loop);
        fallState = new Enemy_KingPig_FallState(stateMachine, this, "fall", EnemyAnimationType.Loop);
        groundState = new Enemy_KingPig_GroundState(stateMachine, this, "ground", EnemyAnimationType.OneShot);
    }

    protected override void Start()
    {
        base.Start();
        hasPatrolRange = false; // 不巡逻
        ResetTeleportTimer();
    }

    protected override void Update()
    {
        base.Update();

        // 自动瞬移（避开战斗/受击/死亡,只在巡逻状态瞬移）
        if (!isDead && !isInvincible &&
            stateMachine.currentState == patrolState &&
            !IsPlayerDetected())
        {
            if (Time.time >= nextTeleportTime)
            {
                TryTeleport();
            }
        }

        // 空中/落地状态自动切换
        if (!isDead)
        {
            if (!IsGrounded() &&
                stateMachine.currentState != fallState &&
                stateMachine.currentState != jumpState &&
                stateMachine.currentState != attackState)
            {
                stateMachine.ChangeState(fallState);
            }
            else if (IsGrounded() && stateMachine.currentState == fallState)
            {
                stateMachine.ChangeState(groundState);
            }
        }
    }

    private void TryTeleport()
    {
        if (teleportPoints == null || teleportPoints.Length <= 1) return; // 至少需要2个点（含初始点）

        Vector3 current = transform.position;
        Transform target = null;

        // 第一个点为初始位置，从第二个点（索引1）开始依次选取
        int targetIndex = (currentTeleportIndex+1) % (teleportPoints.Length);

        Transform pt = teleportPoints[targetIndex];
        if (pt != null && Vector3.Distance(pt.position, current) > 0.5f)
        {
            target = pt;
            // 更新下次索引（确保始终从1开始循环）
            currentTeleportIndex = (currentTeleportIndex+1) % (teleportPoints.Length);
        }

        if (target != null)
        {
            isTeleporting = true;
            teleportTargetPosition = new Vector3(target.position.x, target.position.y + 0.8f, transform.position.z);
            rb.velocity = new Vector2(0, jump_vy);
            jumpState.SetTeleportTarget(teleportTargetPosition);
            stateMachine.ChangeState(jumpState);
        }

        ResetTeleportTimer();
    }

    private void ResetTeleportTimer()
    {
        nextTeleportTime = Time.time + Random.Range(minTeleportInterval, maxTeleportInterval);
    }

    // 用于外部调用（如受伤瞬移）
    public void ForceTeleport()
    {
        TryTeleport();
    }

    public override void OnHittedAnimationEnd()
    {
        if (isDead) stateMachine.ChangeState(deadState);
        else TryTeleport();
    }

    public override void OnDeadAnimationEnd()
    {
        base.OnDeadAnimationEnd();
        ui.GameVictory();
    }

    protected override void CollisionDetection()
    {
        isGround = Physics2D.Raycast(GroundCheck.position, Vector2.down, groundDis, whatIsGround);
        isCliff = Physics2D.Raycast(CliffCheck.position, Vector2.down, cliffDis, whatIsGround);
        isCliff = !isCliff;
        isWall = Physics2D.Raycast(WallCheck.position, Vector2.right * facingdir, wallDis, whatIsGround);

        // 前方检测
        var frontDetect = Physics2D.Raycast(DetectCheck.position, Vector2.right * facingdir, apDistance, whatisPlayer);
        // 后方检测
        var backDetect = Physics2D.Raycast(DetectCheck.position, Vector2.right * (-facingdir), apDistance*0.8f, whatisPlayer);

        _isPlayerDetected = frontDetect.collider != null || backDetect.collider != null;

        // 如果检测到玩家且在后方，记录结果并翻转
        if (backDetect.collider != null)
        {
            _playerRaycastResult = backDetect;
            Flip(); // 检测到后方玩家时翻转
        }
        else if (frontDetect.collider != null)
        {
            _playerRaycastResult = frontDetect;
        }
    }
}