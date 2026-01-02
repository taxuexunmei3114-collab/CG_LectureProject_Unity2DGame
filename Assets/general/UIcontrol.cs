using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIcontrol : MonoBehaviour
{
    public static UIcontrol instance;
    [Header("UI Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button godModeButton;
    [SerializeField] private Button mainMenuButton;

    [Header("UI Text & Image")]
    [SerializeField] private Text godModeText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Image gameOverImage;
    [SerializeField] private Text DoubleJumpText;
    [SerializeField] private Image DoubleJumpImage;

    private bool isGodMode = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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
            
        }
        else
        {
            Debug.LogError("mainMenuButton is not assigned in the inspector!");
        }

        UpdateGodModeText();
    }

    private void RestartGame()
    {
        // 重新播放关卡音乐
        if (BgmControl.Instance != null)
        {
            BgmControl.Instance.PlayLevelBGM();
        }
        

        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Awake();
        Start();
        
    }

    private void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu called, attempting to load MainMenu scene");
        
        // 检查场景是否存在
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("MainMenu");
        if (sceneIndex < 0)
        {
            return;
        }
        
        // 加载主菜单场景
        SceneManager.LoadScene("MainMenu");

        // 切换到主菜单音乐
        if (BgmControl.Instance != null)
        {
            Debug.Log("Switching to Main Menu BGM");
            BgmControl.Instance.PlayMainMenuBGM();
        }
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
        // 播放失败音乐
        if (BgmControl.Instance != null)
        {
            BgmControl.Instance.PlayLoseGameBGM();
        }

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
        // 播放胜利音乐
        if (BgmControl.Instance != null)
        {
            BgmControl.Instance.PlayWinGameBGM();
        }

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

    public void GainDoubleJumpAbility()
    {
        if (DoubleJumpText != null)
        {
            DoubleJumpText.gameObject.SetActive(true);
            DoubleJumpText.text = "获得二段跳能力";
        }
        if (DoubleJumpImage != null)
            DoubleJumpImage.gameObject.SetActive(true);

        StartCoroutine(HideDoubleJumpUIAfterDelay(1.5f));


    }

    private IEnumerator HideDoubleJumpUIAfterDelay(float delayTime)
    {
        // 等待指定秒数
        yield return new WaitForSeconds(delayTime);
        // 执行隐藏逻辑
        HideDoubleJumpUi();
    }

    // 隐藏游戏结束UI
    private void HideGameOverUI()
    {
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (gameOverImage != null)
            gameOverImage.gameObject.SetActive(false);
    }

    private void HideDoubleJumpUi()
    {
        if (DoubleJumpText != null)
            DoubleJumpText.gameObject.SetActive(false);

        if (DoubleJumpImage != null)
            DoubleJumpImage.gameObject.SetActive(false);
    }

}
