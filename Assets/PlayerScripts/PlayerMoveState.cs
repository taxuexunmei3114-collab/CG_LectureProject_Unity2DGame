using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
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
        float targetVelocityX = inputcontrol.xInput * player.moveSpeed;
        player.setVelocity(targetVelocityX, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.x) <=0.01f)//靠速度判断
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
