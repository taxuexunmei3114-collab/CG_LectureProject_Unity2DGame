using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBgmControl : MonoBehaviour
{
    public static ToggleBgmControl Instance;
    private bool isEnterBossRoom;
    public Collider2D bossRoomTrigger;

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

        isEnterBossRoom = false;

    }
    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        
        bool isPlayerInBossRoom = bossRoomTrigger.OverlapPoint(player.transform.position);

        
        if (isPlayerInBossRoom && !isEnterBossRoom)
        {
            isEnterBossRoom = true;
            BgmControl.Instance.PlayBossBGM();
        }

        
        if (!isPlayerInBossRoom && isEnterBossRoom)
        {
            isEnterBossRoom = false;
            // 离开Boss房间时，切换回关卡音乐
            BgmControl.Instance.PlayLevelBGM();
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{

    //    // 是否第一次进入
    //    if (!isEnterBossRoom)
    //    {
    //        // 2. 标记：已经进入Boss房，不会再触发
    //        isEnterBossRoom = true;

    //        // 3. 播放Boss战BGM2
    //        BgmControl.Instance.ChangeBGMSmooth(BgmControl.Instance.bgm2);

            
    //    }
    //}

    public void Reset()
    {
        isEnterBossRoom = false;
    }
}
