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
    [SerializeField] private Button mainMenuButton;

    [Header("UI Text & Image")]
    [SerializeField] private Text godModeText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Image gameOverImage;

    private bool isGodMode = false;

    private void Start()
    {
        // 初始隐藏游戏结束相关UI
        HideGameOverUI();
        
        // 调试信息：检查按钮是否被分配
        Debug.Log("UIcontrol Start called");
        Debug.Log("mainMenuButton is " + (mainMenuButton != null ? "assigned" : "NULL"));
        
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

        if (mainMenuButton != null)
        {
            Navigation nav = mainMenuButton.navigation;
            nav.mode = Navigation.Mode.None;
            mainMenuButton.navigation = nav;

            mainMenuButton.onClick.AddListener(() => {
                Debug.Log("Main Menu button clicked - attempting to load MainMenu scene");
                GoToMainMenu();
            });
            
            // 调试：检查按钮状态
            Debug.Log("mainMenuButton gameObject active: " + mainMenuButton.gameObject.activeSelf);
            Debug.Log("mainMenuButton interactable: " + mainMenuButton.interactable);
        }
        else
        {
            Debug.LogError("mainMenuButton is not assigned in the inspector!");
        }

        UpdateGodModeText();
    }

    private void RestartGame()
    {
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //BgmControl.Instance.ChangeBGMSmooth(BgmControl.Instance.bgm1);
        HideGameOverUI();
    }

    private void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu called, attempting to load MainMenu scene");
        
        // 检查场景是否存在
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("MainMenu");
        if (sceneIndex < 0)
        {
            Debug.LogError("MainMenu scene not found in build settings! Available scenes:");
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                Debug.Log("Scene " + i + ": " + SceneUtility.GetScenePathByBuildIndex(i));
            }
            return;
        }
        
        // 加载主菜单场景
        SceneManager.LoadScene("MainMenu");
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
            godModeText.text = isGodMode ? "无敌: 开启(wasd移动 J攻击 空格跳跃 长按s落下)" : "无敌: 关闭(wasd移动 J攻击 空格跳跃 长按s落下)";
        }
    }

    // 显示游戏结束UI
    public void GameOver()
    {
        if (gameOverText != null)
        { 
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "游戏失败";
        }

        if (gameOverImage != null)
            gameOverImage.gameObject.SetActive(true);

        // 显示主菜单按钮
        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(true);
    }

    public void GameVictory()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "游戏通关";
        }
        if (gameOverImage != null)
            gameOverImage.gameObject.SetActive(true);

        // 显示主菜单按钮
        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(true);
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
