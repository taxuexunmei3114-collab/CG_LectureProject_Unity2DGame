using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    public Image Bar;
    public float health=5, maxHealth = 8;
    public Player player;

    // Update is called once per frame
    void Update()
    {
        BarFiller();
        testIsDead();
    }

    private void BarFiller()
    {
        Bar.fillAmount = health / maxHealth;
    }

    public void AddHealth(float amount=1)
    {
        health += amount;
    }
    public void RemoveHealth(float amount = 1)
    {
        health -= amount;
    }
    private void testIsDead()
    {
        if (health<=0)
        {
            player.isDead=true;
        }
    }
}
