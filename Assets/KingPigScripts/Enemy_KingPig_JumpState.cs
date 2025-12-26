// Enemy_KingPig_JumpState.cs
using UnityEngine;

public class Enemy_KingPig_JumpState : EnemyState
{
    private Enemy_KingPig kingPig;
    private Vector3 teleportTarget;
    private bool hasTeleported = false; // 新增标志
    private float timer;

    public Enemy_KingPig_JumpState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animName, EnemyAnimationType type)
        : base(stateMachine, enemy, animName, type)
    {
        kingPig = enemy as Enemy_KingPig;
    }

    public void SetTeleportTarget(Vector3 target)
    {
        teleportTarget = target;
        hasTeleported = false; // 重置
    }

    public override void Enter()
    {
        base.Enter();
        hasTeleported = false;
        kingPig.isTeleporting = true;
        timer = 0f;


    }

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        // 第一次 Update 时才瞬移（确保跳动画至少开始播放）
        if (timer >= 0.3f && !hasTeleported && teleportTarget != Vector3.zero)
        {
            kingPig.transform.position = teleportTarget;
            hasTeleported = true;
            kingPig.rb.velocity= new Vector2(0,0);
            // 注意：不要在这里 ChangeState，因为要先进入 Fall
        }

        // 立即进入下落（因为已经在空中）
        // 但为了避免同一帧切换，可加极短延迟或直接切
        if (hasTeleported)
        {
            stateMachine.ChangeState(kingPig.fallState);
        }
    }
}