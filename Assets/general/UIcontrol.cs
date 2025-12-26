using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIcontrolr : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button godModeButton;

    [Header("UI Text & Image")]
    [SerializeField] private Text godModeText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Image gameOverImage;

    private bool isGodMode = false;

    private void Start()
    {
        // 初始隐藏游戏结束相关UI
        HideGameOverUI();
        // 设置按钮导航为None，防止键盘输入触发按钮
        if (restartButton != null)
        {
            Navigation nav = restartButton.navigation;
            nav.mode = Navigation.Mode.None;
            restartButton.navigation = nav;

            restartButton.onClick.AddListener(() => {
                Debug.Log("Restart button clicked");
                RestartGame();
            });
        }

        if (godModeButton != null)
        {
            Navigation nav = godModeButton.navigation;
            nav.mode = Navigation.Mode.None;
            godModeButton.navigation = nav;

            godModeButton.onClick.AddListener(() => {
                Debug.Log("God Mode button clicked");
                ToggleGodMode();
            });
        }

        UpdateGodModeText();
    }

    private void RestartGame()
    {
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        HideGameOverUI();
    }

    private void ToggleGodMode()
    {
        isGodMode = !isGodMode;

        // 设置玩家无敌模式
        if (Player.Instance != null)
        {
            Player.Instance.setGodMode(isGodMode);
        }

        UpdateGodModeText();
    }

    private void UpdateGodModeText()
    {
        if (godModeText != null)
        {
            godModeText.text = isGodMode ? "无敌: 开启(注意：炸弹猪是无敌的)" : "无敌: 关闭(注意：炸弹猪是无敌的)";
        }
    }

    // 显示游戏结束UI
    public void GameOver()
    {
        if (gameOverText != null)
        { 
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "游戏失败\n(请重新开始)";
        }

        if (gameOverImage != null)
            gameOverImage.gameObject.SetActive(true);
    }

    public void GameVictory()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "游戏通关\n(请重新开始)";
        }
        if (gameOverImage != null)
            gameOverImage.gameObject.SetActive(true);
    }

    // 隐藏游戏结束UI
    private void HideGameOverUI()
    {
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (gameOverImage != null)
            gameOverImage.gameObject.SetActive(false);
    }

}
