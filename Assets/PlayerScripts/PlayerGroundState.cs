using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.is2jump = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (inputcontrol.IsAttackPressed())
        {
            stateMachine.ChangeState(player.attackState);
        }
        if (!player.isGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        if (inputcontrol.IsJumpPressed() && player.isGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
