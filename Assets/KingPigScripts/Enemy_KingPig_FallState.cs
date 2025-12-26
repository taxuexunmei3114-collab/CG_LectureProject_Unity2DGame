// Enemy_KingPig_FallState.cs
using UnityEngine;

public class Enemy_KingPig_FallState : EnemyState
{
    private Enemy_KingPig kingPig;
    public Enemy_KingPig_FallState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animName, EnemyAnimationType type)
        : base(stateMachine, enemy, animName, type)
    {
        kingPig = enemy as Enemy_KingPig;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // 检测是否落地
        if (kingPig.IsGrounded())
        {
            stateMachine.ChangeState(kingPig.groundState);
        }

        // 保持垂直速度（让物理生效）
        // 不要设置 rb.velocity.x，允许惯性或风力等（可选）
    }
}