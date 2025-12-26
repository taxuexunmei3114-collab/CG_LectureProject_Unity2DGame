using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHittedState : PlayerState
{
    public PlayerHittedState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0,rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}