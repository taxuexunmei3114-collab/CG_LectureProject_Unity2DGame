using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorInControl : MonoBehaviour
{
    private Transform entrancePoint; 
    public Transform exitPoint;     

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            entrancePoint=transform;
            if (exitPoint != null)
            {
                player.DoorIn(entrancePoint, exitPoint);
            }
            else
            {
                player.DoorIn(entrancePoint);
            }
        }
    }
}
