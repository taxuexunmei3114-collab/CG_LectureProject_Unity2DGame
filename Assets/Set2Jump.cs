using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set2Jump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player e = collision.GetComponent<Player>();
        if (e != null)
        {
            e.Set2JumpEnable(true);
        }
    }
}
