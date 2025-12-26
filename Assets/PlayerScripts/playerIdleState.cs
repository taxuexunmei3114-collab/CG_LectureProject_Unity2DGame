using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerIdleState : PlayerGroundState
{
    public playerIdleState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.setVelocity(0, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.setVelocity(0, rb.velocity.y);
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
        if(inputcontrol.IsJumpPressed())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        if (Mathf.Abs(inputcontrol.xInput)!=0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
