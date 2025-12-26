using UnityEngine;

/// <summary>
/// 简单的Player输入处理器，统一管理所有输入检测
/// 替代分散在各个状态中的输入检测逻辑
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    // 移动输入
    public float horizontalInput { get; private set; }
    // 瞬时输入（按下的那一帧）
    public bool jumpPressed { get; private set; }
    public bool attackPressed { get; private set; }

    // 检测移动输入（当水平输入的绝对值大于 0.1 时认为有输入）
    public float xInput
    {
        get
        {
            float absoluteValue = Mathf.Abs(horizontalInput);
            if (absoluteValue < 0.1f) return 0f;
            else return horizontalInput;
        }
    }
    // 属性：移动方向（1 表示向右，-1 表示向左，0 表示没有移动）
    public int moveDirection
    {
        get
        {
            if (horizontalInput > 0.1f)
            {
                return 1;
            }
            else if (horizontalInput < -0.1f)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    private void Update()
    {
        UpdateInputDetection();
    }

    private void UpdateInputDetection()
    {
        // 水平移动输入（持续检测）
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 跳跃输入（按下检测）
        jumpPressed = Input.GetKeyDown(KeyCode.Space);

        // 攻击输入（按下检测）
        attackPressed = Input.GetKeyDown(KeyCode.J);
    }

    public bool IsJumpPressed()
    {
        return jumpPressed;
    }

    public bool IsAttackPressed()
    {
        return attackPressed;
    }

    public void SetInputEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public string GetInputInfo()
    {
        return $"Horizontal: {horizontalInput:F2}, Jump: {jumpPressed}, Attack: {attackPressed}";
    }
}