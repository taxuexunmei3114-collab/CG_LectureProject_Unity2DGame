using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        float airMoveSpeed = player.moveSpeed * player.fallMoveMultiplier;
        player.setVelocity(inputcontrol.xInput * airMoveSpeed, rb.velocity.y);
        if(inputcontrol.IsJumpPressed() && player.is2JumpEnable && !player.is2jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
            player.is2jump = true;
        }
        if (inputcontrol.IsAttackPressed())
        {
            stateMachine.ChangeState(player.attackState);
        }
        if (player.isGroundDetected())
        {
            if (Mathf.Abs(rb.velocity.x) <= 0.01f)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
    }
}
