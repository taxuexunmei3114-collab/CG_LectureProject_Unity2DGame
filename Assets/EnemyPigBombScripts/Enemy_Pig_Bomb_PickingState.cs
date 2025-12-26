using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_Bomb_PickingState : EnemyState
{
    private Enemy_Pig_Bomb bombEnemy;
    private bool isAnimationFinished = false;

    public Enemy_Pig_Bomb_PickingState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
        : base(stateMachine, enemy, animationName, type)
    {
        this.bombEnemy = enemy as Enemy_Pig_Bomb;
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, rb.velocity.y);
        isAnimationFinished = false;
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, rb.velocity.y);
        // 检查投掷动画是否完成
        if (!isAnimationFinished && !animController.IsPlaying(animationName))
        {
            isAnimationFinished = true;
            if (bombEnemy._isPlayerDetected)
            {
                // 如果还能看到玩家，继续追击
                stateMachine.ChangeState(bombEnemy.chaseState);
            }
            else
            {
                // 否则回到巡逻状态
                stateMachine.ChangeState(bombEnemy.patrolState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        isAnimationFinished = false;
    }
}
