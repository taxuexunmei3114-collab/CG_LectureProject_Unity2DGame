using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_ChaseState : EnemyState
{
    public Enemy_Pig_ChaseState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
        : base(stateMachine, enemy, animationName, type)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        //先检测是否落地，若未落地先以idle状态自由落体
        if(!enemy.isGround)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }
        // 检测玩家
        if (!enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        // 检查是否为炸弹猪并判断是否应该投掷炸弹
        if (enemy is Enemy_Pig_Bomb bombEnemy)
        {
            if (bombEnemy.IsWaitingToThrow())
            {
                stateMachine.ChangeState(bombEnemy.idleState);
                return;
            }
            if (bombEnemy.ShouldThrowBomb())
            {
                stateMachine.ChangeState(bombEnemy.throwState);
                return;
            }

  
        }
        else
        {
            // 计算与玩家的距离
            float distanceToPlayer = enemy.GetDistanceToPlayer();

            if (distanceToPlayer <= enemy.attackTriggerDistance)
            {
                // 距离足够近，准备攻击
                stateMachine.ChangeState(enemy.attackState);
                return;
            }
        }


        // 获取玩家相对于敌人的方向
        bool playerOnRight = enemy.PlayerRaycastResult.point.x > enemy.transform.position.x;

        // 检查是否可以朝玩家方向移动
        if (enemy.CanMoveInDirection(playerOnRight))
        {
            // 可以移动，继续追击
            float chaseVelocity = enemy.moveSpeed * enemy.chaseSpeedMultiplier * enemy.facingdir;
            rb.velocity = new Vector2(chaseVelocity, 0);
        }
        else
        {
            // 无法朝玩家方向移动，停在边界并切换到Idle状态
            rb.velocity = new Vector2(0, 0);
            stateMachine.ChangeState(enemy.idleState);
            return;
        }


        // 继续追击
        float chaseSpeed = enemy.moveSpeed * enemy.chaseSpeedMultiplier * enemy.facingdir;
        rb.velocity = new Vector2(chaseSpeed, 0);


        // 更新动画参数
        animController.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }
}