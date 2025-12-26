// Enemy_KingPig_GroundState.cs
using UnityEngine;

public class Enemy_KingPig_GroundState : EnemyState
{
    private Enemy_KingPig kingPig;
    private bool isAnimationFinished=false;

    public Enemy_KingPig_GroundState(EnemyStateMachine stateMachine, Enemy_Pig enemy, string animName, EnemyAnimationType type)
        : base(stateMachine, enemy, animName, type)
    {
        kingPig = enemy as Enemy_KingPig;
    }

    public override void Enter()
    {
        base.Enter();
        isAnimationFinished = false;
    }

    public override void Update()
    {
        base.Update();
        // 检查动画是否完成
        if (!isAnimationFinished && !animController.IsPlaying(animationName))
        {
            isAnimationFinished = true;
            // 完成后返回巡逻或追击状态
            if (kingPig.IsPlayerDetected())
            {
                stateMachine.ChangeState(kingPig.chaseState);
            }
            else
            {
                stateMachine.ChangeState(kingPig.patrolState);
            }
        }
    }
}