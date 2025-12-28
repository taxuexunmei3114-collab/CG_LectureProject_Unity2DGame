using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiamondControl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player e = collision.GetComponent<Player>();
        if (e != null)
        {
            e.Heal(1);
            OnDestroy();
        }
    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }

}
