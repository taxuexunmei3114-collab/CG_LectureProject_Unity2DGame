using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_hitted_control e = collision.GetComponent<Player_hitted_control>();
        if (e != null)
        {
            e.hurt(1);
        }

    }
}
