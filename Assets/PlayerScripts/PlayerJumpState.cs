using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        // 添加空中移动控制 - 使用可配置的跳跃移动倍数
        float airMoveSpeed = player.moveSpeed * player.jumpMoveMultiplier;
        player.setVelocity(inputcontrol.xInput * airMoveSpeed, rb.velocity.y);
        if (inputcontrol.IsAttackPressed())
        {
            stateMachine.ChangeState(player.attackState);
        }
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
