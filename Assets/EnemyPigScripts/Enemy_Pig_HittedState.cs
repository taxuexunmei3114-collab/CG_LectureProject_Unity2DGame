using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig_HittedState : EnemyState
{

    public Enemy_Pig_HittedState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
        : base(stateMachine, enemy, animationName, type)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, 0);
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, 0);
        // hitted状态由动画帧事件触发changestate，在Enemy_pig基类的nHittedAnimationEnd（）

    }
}