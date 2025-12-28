using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour 
{
    // 第一个可玩关卡的场景名称
    public string firstLevelSceneName = "SampleScene"; 

    public void Start()
    {
        // 获取开始按钮并添加点击事件监听器
        Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
        Debug.Log("按钮被点击了！"); // 查看Console是否有输出
    

        // 如果有退出按钮，也可以添加
        Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        // 切换到第一个关卡场景
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void QuitGame()
    {
        // 退出游戏（仅在构建的可执行文件中有效）
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
