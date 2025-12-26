using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.XR;

public class PlayerDeadState : PlayerState
{
    private bool deathCoroutineStarted = false;
    public PlayerDeadState(PlayerStateMachine stateMachine, Player player, string animationName, AnimationType type, PlayerInputHandler c) : base(stateMachine, player, animationName, type, c)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, rb.velocity.y);
        // 确保只触发一次死亡逻辑
        if (!deathCoroutineStarted && !animController.IsPlaying(animationName))
        {
            deathCoroutineStarted = true;
        }
    }
}
