using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour 
{
    // 第一个可玩关卡的场景名称
    public string firstLevelSceneName = "SampleScene"; 
    
    // GitHub 仓库链接
    public string githubUrl = "https://github.com/taxuexunmei3114-collab/CG_LectureProject_Unity2DGame";

    public void Start()
    {
        // 获取开始按钮并添加点击事件监听器
        Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
        Debug.Log("按钮被点击了！"); // 查看Console是否有输出
    

        // 如果有退出按钮，也可以添加
        Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);
        
        // 获取链接按钮并添加点击事件监听器
        // 假设链接按钮的名称为 "LinkButton"，如果不存在则不会添加监听器
        GameObject linkButtonObj = GameObject.Find("LinkButton");
        if (linkButtonObj != null)
        {
            Button linkButton = linkButtonObj.GetComponent<Button>();
            if (linkButton != null)
            {
                linkButton.onClick.AddListener(OpenGitHubLink);
                Debug.Log("链接按钮事件监听器已添加");
            }
            else
            {
                Debug.LogWarning("找到名为 'LinkButton' 的游戏对象，但没有 Button 组件");
            }
        }
        else
        {
            Debug.LogWarning("未找到名为 'LinkButton' 的游戏对象，请确保按钮名称正确");
        }
    }

    public void StartGame()
    {
        // 切换到关卡音乐
        if (BgmControl.Instance != null)
        {
            BgmControl.Instance.PlayLevelBGM();
        }
        
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
    
    public void OpenGitHubLink()
    {
        // 打开GitHub网页
        if (!string.IsNullOrEmpty(githubUrl))
        {
            Debug.Log("打开GitHub链接: " + githubUrl);
            Application.OpenURL(githubUrl);
        }
        else
        {
            Debug.LogWarning("GitHub链接为空，无法打开");
        }
    }
}
