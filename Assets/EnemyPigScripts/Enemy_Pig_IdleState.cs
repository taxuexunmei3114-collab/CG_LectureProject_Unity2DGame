using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_IdleState : EnemyState
{


    public Enemy_Pig_IdleState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
        : base(stateMachine, enemy, animationName, type)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, rb.velocity.y);
        // 检测玩家
        if (enemy.IsPlayerDetected())
        {

            // 计算与玩家的距离
            float distanceToPlayer = enemy.GetDistanceToPlayer();

            if (distanceToPlayer <= enemy.attackTriggerDistance)
            {
                // 距离足够近，准备攻击
                // 检查是否为炸弹猪并判断是否应该投掷炸弹
                if (enemy is Enemy_Pig_Bomb bombEnemy && bombEnemy.ShouldThrowBomb())
                {
                    stateMachine.ChangeState(bombEnemy.throwState);
                    return;
                }

                stateMachine.ChangeState(enemy.attackState);
                return;
            }

            // 获取玩家相对于敌人的方向
            bool playerOnRight = enemy.PlayerRaycastResult.point.x > enemy.transform.position.x;
            // 检查是否可以朝玩家方向移动
            if (enemy.CanMoveInDirection(playerOnRight))
            {
                stateMachine.ChangeState(enemy.chaseState);
            }

        }
        else
        {
            stateMachine.ChangeState(enemy.patrolState);
        }
    }
}