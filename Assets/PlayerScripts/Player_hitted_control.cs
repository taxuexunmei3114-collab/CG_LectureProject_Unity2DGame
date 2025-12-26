using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_hitted_control : MonoBehaviour
{
    public Player player;

    public void hurt(float damage=1)
    {
        player.getHitted(damage);
    }
}
