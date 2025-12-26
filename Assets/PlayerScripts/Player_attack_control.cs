using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy_hitted_control e = collision.GetComponent<Enemy_hitted_control>();
        if (e != null)
        {
            e.hurt();
        }
    }
}
