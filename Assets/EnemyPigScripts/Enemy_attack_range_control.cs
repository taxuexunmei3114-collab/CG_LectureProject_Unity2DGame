using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_attack_range_control : MonoBehaviour
{
    public GameObject RangeTrigger;

    public void Attack()
    {
        RangeTrigger.SetActive(true);
    }
    public void AttackFinish()
    {
        RangeTrigger.SetActive(false);
    }
}
