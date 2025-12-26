using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float delayExploadTime;
    public GameObject RangeTrigger;

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        Invoke("Explode", delayExploadTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Explode()
    {
        anim.SetTrigger("start");
    }
    private void DestroyThisBomb()
    {
        Destroy(gameObject);
    }

    public void Active()
    {
        RangeTrigger.SetActive(true);
    }
    public void Finish()
    {
        RangeTrigger.SetActive(false);
    }
}
