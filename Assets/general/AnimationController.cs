using UnityEngine;

/// <summary>
/// 简单的动画控制器，替代bool参数控制方式
/// 直接通过代码播放动画，提供更精确的控制
/// </summary>
///
public class AnimationController: MonoBehaviour
{
    public Animator animator;

    public void Play(string animationName)
    {
        animator.Play(animationName);
        //Debug.Log($"AnimationController: Playing '{animationName}'");
    }

    /// <summary>
    /// 平滑切换到指定动画
    /// </summary>
    /// <param name="animationName">动画状态名称</param>
    /// <param name="duration">过渡时间（秒）</param>
    public void CrossFade(string animationName, float duration = 0.1f)
    {
        animator.CrossFade(animationName, duration);
        //Debug.Log($"AnimationController: Crossfading to '{animationName}' (duration: {duration}s)");
    }

    /// <summary>
    /// 设置浮点参数（用于yVelocity等特殊参数）
    /// </summary>
    /// <param name="parameterName">参数名称</param>
    /// <param name="value">参数值</param>
    public void SetFloat(string parameterName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(parameterName, value);
        }
    }

    /// <summary>
    /// 设置布尔参数（保持向后兼容）
    /// </summary>
    /// <param name="parameterName">参数名称</param>
    /// <param name="value">参数值</param>
    public void SetBool(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }

    /// <summary>
    /// 触发触发器参数（保持向后兼容）
    /// </summary>
    /// <param name="parameterName">参数名称</param>
    public void SetTrigger(string parameterName)
    {
        if (animator != null)
        {
            animator.SetTrigger(parameterName);
        }
    }

    /// <summary>
    /// 检查指定动画是否正在播放
    /// </summary>
    /// <param name="animationName">动画名称</param>
    /// <returns>是否正在播放</returns>
    public bool IsPlaying(string animationName)
    {
        if (animator == null) return false;

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1f;
    }

    /// <summary>
    /// 获取当前动画状态信息（调试用）
    /// </summary>
    public string GetCurrentAnimationInfo()
    {
        if (animator == null) return "No Animator";

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return $"Current: {stateInfo.shortNameHash}, Progress: {stateInfo.normalizedTime:F2}";
    }

    private void Awake()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            // 如果当前对象没有，尝试获取子对象的
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
           Debug.LogWarning("AnimationController: No Animator component found!");
        }
    }
}