using UnityEngine;
using System.Collections;

public class BgmControl : MonoBehaviour
{
    // 全局单例：其他脚本可以直接  BgmControl.Instance.调用所有方法
    public static BgmControl Instance;
    // 播放BGM的音频源组件（拖拽赋值即可）
    public AudioSource audioSource;

    // 五种场景音乐
    public AudioClip mainMenuBGM;     // 主界面音乐
    public AudioClip levelBGM;        // 关卡音乐（普通关卡）
    public AudioClip bossBGM;         // Boss战音乐
    public AudioClip winGameBGM;      // 胜利音乐
    public AudioClip loseGameBGM;     // 失败音乐

    private AudioClip currentBGM;     // 当前播放的音乐

    private void Awake()
    {
        // 核心：跨场景不销毁 + 单例防重复BGM重叠播放
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 初始化设置：2D背景音乐、循环播放、唤醒自动播放
        audioSource.spatialBlend = 0; 
        audioSource.loop = true;      // BGM默认循环播放
        
        // 默认播放主菜单音乐
        PlayMainMenuBGM();
    }


    // 播放主界面音乐
    public void PlayMainMenuBGM()
    {
        if (mainMenuBGM == null) return;
        currentBGM = mainMenuBGM;
           
        ChangeBGM(mainMenuBGM);
    }

    // 播放关卡音乐（普通关卡）
    public void PlayLevelBGM()
    {
        if (levelBGM == null) 
        {
            Debug.LogWarning("BgmControl: levelBGM is null! Cannot play level music.");
            return;
        }
        currentBGM = levelBGM;
        
        ChangeBGM(levelBGM);
    }

    // 播放Boss战音乐
    public void PlayBossBGM()
    {
        if (bossBGM == null) return;
        currentBGM = bossBGM;
        
        ChangeBGM(bossBGM);
    }

    // 播放胜利音乐
    public void PlayWinGameBGM()
    {
        if (winGameBGM == null) return;
        currentBGM = winGameBGM;
        
       
        ChangeBGM(winGameBGM);
    }

    // 播放失败音乐
    public void PlayLoseGameBGM()
    {
        if (loseGameBGM == null) return;
        currentBGM = loseGameBGM;
        
        ChangeBGM(loseGameBGM);
    }

    // 获取当前播放的音乐
    public AudioClip GetCurrentBGM()
    {
        return currentBGM;
    }


    // 切换BGM 
    public void ChangeBGM(AudioClip newBgmClip)
    {
        if (newBgmClip == null) return; // 空音频不执行
        audioSource.clip = newBgmClip;  // 替换新的BGM
        audioSource.Play();             // 播放新BGM
    }

    // 播放/继续播放BGM
    public void PlayBGM()
    {
       audioSource.Play();
    }

    // 暂停BGM
    public void PauseBGM()
    {
        audioSource.Pause();
    }

    // 设置BGM音量
    public void SetVolume(float vol)
    {
        audioSource.volume = Mathf.Clamp01(vol); // 限制音量0~1，防止出错
    }
}
