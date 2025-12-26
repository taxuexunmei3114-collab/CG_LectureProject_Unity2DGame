using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Loop,      // 循环播放：Idle, Run
    OneShot    // 播放一次：Attack, Hit
}

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    public string animationName;// 新的动画名称（用于直接播放）
    public AnimationType type;

    protected Rigidbody2D rb;
    public AnimationController animController;
    public PlayerInputHandler inputcontrol;

    public static float health;

    // 新的构造函数：使用动画名称
    public PlayerState(PlayerStateMachine stateMachine, Player player, string animationName,AnimationType type, PlayerInputHandler inputcontrol)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animationName = animationName;
        this.inputcontrol = inputcontrol;
        this.type=type;
        health = player.healthControl.health;
    }

    public virtual void Enter()
    {
        rb = player.rb;
        animController = player.animationcontroller;
        // 如果是一次性动画，检查是否已经在播放
        if (type == AnimationType.OneShot)
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
        health = player.healthControl.health;
        if(player.isDead)
        {
            return;
        }
        animController.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
 
        //Debug.Log($"[PlayerState] Exiting {GetType().Name} (animation '{animationName}')");

    }

}
