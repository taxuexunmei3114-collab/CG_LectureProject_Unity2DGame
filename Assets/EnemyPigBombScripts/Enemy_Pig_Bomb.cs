using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_Bomb : Enemy_Pig
{
    [Header("Bomb Info")]
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    public float throwCooldown = 2f;


    [Header("Throw Settings")]
    public float minThrowForce = 3f;   // 玩家很近时的最小力度
    public float maxThrowForce = 8f;   // 玩家很远时的最大力度
    public float maxThrowRange = 10f;  // 超过这个距离也按最大力扔

    public float lastThrowTime = 0f;

    // 炸弹猪特有的状态（需要在Awake中初始化）
    public Enemy_Pig_Bomb_ThrowState throwState { get; private set; }
    public Enemy_Pig_Bomb_PickingState  pickingState { get; private set; }

    protected override void Awake()
    {
        // 重新初始化状态机，包含炸弹猪特有的状态
        stateMachine = new EnemyStateMachine();

        // 初始化所有状态（包括父类的和自己的）
        idleState = new Enemy_Pig_IdleState(stateMachine, this, "Pig_bomb_idle", EnemyAnimationType.Loop);
        patrolState = new Enemy_Pig_PatrolState(stateMachine, this, "Pig_bomb_run", EnemyAnimationType.Loop);
        chaseState = new Enemy_Pig_ChaseState(stateMachine, this, "Pig_bomb_run", EnemyAnimationType.Loop);
        attackState = new Enemy_Pig_AttackState(stateMachine, this, "Pig_attack", EnemyAnimationType.OneShot);
        hittedState = new Enemy_Pig_HittedState(stateMachine, this, "Pig_bomb_hitted", EnemyAnimationType.OneShot);
        deadState = new Enemy_Pig_DeadState(stateMachine, this, "Pig_bomb_dead", EnemyAnimationType.OneShot);
        // 初始化炸弹猪特有的状态
        throwState = new Enemy_Pig_Bomb_ThrowState(stateMachine, this, "Pig_bomb_throw", EnemyAnimationType.OneShot);
        pickingState = new Enemy_Pig_Bomb_PickingState(stateMachine, this, "Pig_bomb_picking", EnemyAnimationType.OneShot);
    }

    // 炸弹猪特有的方法
    public void ThrowBomb()
    {
        if (bombPrefab == null || bombSpawnPoint == null)
            return;

        // 计算水平距离（忽略Y轴，更适合2D平台）
        float distance = GetDistanceToPlayer();


        // 根据距离计算投掷力度：越远越用力，但不超过 maxThrowForce
        float throwForce = Mathf.Lerp(minThrowForce, maxThrowForce,
                                     Mathf.InverseLerp(0f, maxThrowRange, distance));

        // 实例化炸弹
        GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(facingdir * throwForce, throwForce * 0.5f);
        }
    }

    // 重写方法以添加炸弹猪特定行为
    public override bool IsPlayerDetected()
    {
        // 调用父类检测逻辑
        CollisionDetection(); 
        return _isPlayerDetected;
    }

    // 判断是否应该投掷炸弹
    public bool ShouldThrowBomb()
    {
        return Time.time - lastThrowTime >= throwCooldown &&
               _isPlayerDetected &&
               GetDistanceToPlayer() <= attackTriggerDistance; 
    }
}