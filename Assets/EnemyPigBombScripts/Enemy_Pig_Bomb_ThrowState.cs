using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_Bomb_ThrowState : EnemyState
{
    private Enemy_Pig_Bomb bombEnemy;
    private bool isAnimationFinished = false;

    public Enemy_Pig_Bomb_ThrowState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
        : base(stateMachine, enemy, animationName, type)
    {
        this.bombEnemy = enemy as Enemy_Pig_Bomb;
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, rb.velocity.y);
        isAnimationFinished = false;
        bombEnemy.lastThrowTime = Time.time;

        // 执行投掷动作
        if (bombEnemy != null)
        {
            bombEnemy.ThrowBomb();
        }

    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, rb.velocity.y);
        // 检查投掷动画是否完成
        if (!isAnimationFinished && !animController.IsPlaying(animationName))
        {
            isAnimationFinished = true;
            stateMachine.ChangeState(bombEnemy.pickingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isAnimationFinished = false;
    }
}