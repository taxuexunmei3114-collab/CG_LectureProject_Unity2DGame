using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_PatrolState : EnemyState
{
    public Enemy_Pig_PatrolState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
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

        // 检测玩家
        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.chaseState);
            return;
        }
        // 检查是否有巡逻范围限制
        if (enemy.hasPatrolRange)
        {
            // 只检测X轴方向的距离
            float distanceToTargetX = Mathf.Abs(enemy.transform.position.x - enemy.currentPatrolTarget.position.x);

            if (distanceToTargetX < 0.1f) // 到达目标点
            {
                // 切换到另一个巡逻点
                if (enemy.currentPatrolTarget == enemy.patrolPointA)
                {
                    enemy.currentPatrolTarget = enemy.patrolPointB;
                }
                else
                {
                    enemy.currentPatrolTarget = enemy.patrolPointA;
                }

                // 如果需要翻转方向
                if ((enemy.currentPatrolTarget.position.x < enemy.transform.position.x && enemy.facingdir > 0) ||
                    (enemy.currentPatrolTarget.position.x > enemy.transform.position.x && enemy.facingdir < 0))
                {
                    enemy.Flip();
                }
            }

            // 向目标点移动
            Vector2 direction = (enemy.currentPatrolTarget.position - enemy.transform.position).normalized;
            rb.velocity = new Vector2(direction.x * enemy.moveSpeed, rb.velocity.y);
        }
        else
        {
            // 检测只有落地后遇到墙壁或悬崖掉头(可以控制悬崖检测开关)
            if (((enemy.isCliff && enemy.isCliffCheckEnable) || enemy.isWall) && enemy.isGround)
            {
                enemy.Flip();
                return;
            }
            // 移动
            rb.velocity = new Vector2(enemy.moveSpeed * enemy.facingdir, rb.velocity.y);
        }

        // 更新动画参数
        animController.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }
}