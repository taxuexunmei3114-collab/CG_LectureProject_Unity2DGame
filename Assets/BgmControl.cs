using UnityEngine;
using System.Collections;

public class BgmControl : MonoBehaviour
{
    // 全局单例：其他脚本可以直接  BgmControl.Instance.调用所有方法
    public static BgmControl Instance;
    // 播放BGM的音频源组件（拖拽赋值即可）
    public AudioSource audioSource;

    public AudioClip bgm1; 
    public AudioClip bgm2;

    private int currentBgmIndex = 1;

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
        }

        // 初始化设置：2D背景音乐、循环播放、唤醒自动播放
        audioSource.spatialBlend = 0; // 纯2D音效，取消3D音效
        audioSource.loop = true;      // BGM默认循环播放
        if (bgm1 != null)
        {
            audioSource.clip = bgm1;
            audioSource.Play();
        }
    }


    public void ToggleBgmSmooth()
    {
        if (currentBgmIndex == 1)
        {
            if (bgm2 != null) ChangeBGMSmooth(bgm2);
            currentBgmIndex = 2;
        }
        else
        {
            if (bgm1 != null) ChangeBGMSmooth(bgm1);
            currentBgmIndex = 1;
        }
    }

    public void ToggleBgmSmoothToStart()
    {
        if (currentBgmIndex == 1)
        {
            if (bgm2 != null) ChangeBGMSmooth(bgm2);
            currentBgmIndex = 2;
            ToggleBgmControl.Instance.Reset();
        }
    }


    // 切换BGM 2种写法
    // 【直接切换BGM】(最常用，一键切换，无过渡)
    // 调用示例：BgmControl.Instance.ChangeBGM(你的新BGM音频文件);
    public void ChangeBGM(AudioClip newBgmClip)
    {
        if (newBgmClip == null) return; // 空音频不执行
        audioSource.clip = newBgmClip;  // 替换新的BGM
        audioSource.Play();             // 播放新BGM
    }

    // 【平滑淡入淡出切换BGM】(推荐！体验最好，无卡顿无突兀)
    // 调用示例：BgmControl.Instance.ChangeBGMSmooth(你的新BGM音频文件, 1f);
    public void ChangeBGMSmooth(AudioClip newBgmClip, float fadeTime = 1.5f)
    {
        if (newBgmClip == null) return;
        StartCoroutine(IE_ChangeBGMSmooth(newBgmClip, fadeTime));
    }

    // 平滑切换的协程（内部执行，不用手动调用）
    private IEnumerator IE_ChangeBGMSmooth(AudioClip newBgmClip, float fadeTime)
    {
        float originVolume = audioSource.volume;
        // 第一步：音量淡出（慢慢变小）
        while (audioSource.volume > 0)
        {
            audioSource.volume -= originVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        // 第二步：替换BGM并播放
        audioSource.clip = newBgmClip;
        audioSource.Play();
        // 第三步：音量淡入（慢慢变大）
        while (audioSource.volume < originVolume)
        {
            audioSource.volume += originVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    // 播放/继续播放BGM
    public void PlayBGM()
    {
        if (!audioSource.isPlaying) audioSource.Play();
    }

    // 暂停BGM
    public void PauseBGM()
    {
        audioSource.Pause();
    }

    // 停止BGM（慎用，停止后需要重新Play，暂停更常用）
    public void StopBGM()
    {
        audioSource.Stop();
    }

    // 设置BGM音量
    public void SetVolume(float vol)
    {
        audioSource.volume = Mathf.Clamp01(vol); // 限制音量0~1，防止出错
    }
}