using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("跟随目标")]
    public Transform target; // 拖入Player的Transform（玩家对象）
    [Header("相机偏移量")]
    public Vector3 offset;   // 相机与玩家的距离（可在Inspector调整）
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        // 核心：相机位置 = 玩家位置 + 偏移量（保持Z轴不变，避免相机穿模）
        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                transform.position.z // 固定相机Z轴（2D游戏关键，否则会看不到画面）
            );
        }
    }
}
