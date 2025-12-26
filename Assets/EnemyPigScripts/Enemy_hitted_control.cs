using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_hitted_control : MonoBehaviour
{
    public Enemy_Pig enemy;

    public void hurt()
    {
        enemy.getHurt(1);
    }
}