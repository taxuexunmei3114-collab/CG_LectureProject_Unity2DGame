using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_DeadState : EnemyState
{
    private bool deathProcessed = false;

    public Enemy_Pig_DeadState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
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
        if (!deathProcessed && !animController.IsPlaying(animationName))
        {
            deathProcessed = true;
        }
    }
}