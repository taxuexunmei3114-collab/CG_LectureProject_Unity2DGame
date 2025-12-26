using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoorInState : PlayerState
{
    private float delayAfterAnimation = 1f; // 动画结束后等待的时间
    private float timer = 0f;
    private bool isWaitingToExit = false;
    public PlayerDoorInState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isWaitingToExit = false;
        timer = 0f;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, rb.velocity.y);
        // 如果已经在等待退出，就计时
        if (isWaitingToExit)
        {
            timer += Time.deltaTime;
            if (timer >= delayAfterAnimation)
            {
                player.stateMachine.ChangeState(player.doorOutState);
            }
        }
        else
        {
            // 检查动画是否结束
            if (!animController.IsPlaying(animationName))
            {
                // 动画一结束，就开始计时
                isWaitingToExit = true;
                timer = 0f; // 从0开始计时
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
