using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAnimationType
{
    Loop,      // 循环播放：Idle, Move
    OneShot    // 播放一次：Attack, Hit, Dead
}

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy_Pig enemy;

    public string animationName;// 动画名称
    public EnemyAnimationType type;

    protected Rigidbody2D rb;
    public AnimationController animController;

    public static float health;

    public EnemyState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animationName, EnemyAnimationType type)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.animationName = animationName;
        this.type = type;
        health = enemy.current_health;
    }

    public virtual void Enter()
    {
        rb = enemy.rb;
        animController = enemy.animationcontroller; // 假设Enemy_Pig有一个animationcontroller字段

        // 如果是一次性动画，检查是否已经在播放
        if (type == EnemyAnimationType.OneShot)
        {
            if (!animController.IsPlaying(animationName))
            {
                animController.Play(animationName);
            }
        }
        else
        {
            // 循环动画：直接播放，无需检查
            animController.Play(animationName);
        }
    }

    public virtual void Update()
    {
        health = enemy.current_health;
        if (enemy.isDead)
        {
            return;
        }
    }

    public virtual void Exit()
    {
        //Debug.Log($"[EnemyState] Exiting {GetType().Name} (animation '{animationName}')");
    }
}