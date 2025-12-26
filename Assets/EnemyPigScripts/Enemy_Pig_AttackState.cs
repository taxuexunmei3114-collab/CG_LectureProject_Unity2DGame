using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_AttackState : EnemyState
{
    private bool isAnimationFinished = false;

    public Enemy_Pig_AttackState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
        : base(stateMachine, enemy, animationName, type)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, 0);
        isAnimationFinished = false;
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, 0);
        // 检查攻击动画是否完成
        if (!isAnimationFinished && !animController.IsPlaying(animationName))
        {
            isAnimationFinished = true;
            // 攻击完成后返回巡逻或追击状态
            if (enemy.IsPlayerDetected())
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
            else
            {
                stateMachine.ChangeState(enemy.patrolState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}