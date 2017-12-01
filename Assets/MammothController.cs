using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MammothController : MonoBehaviour
{
    public float mammothSpeed;
    public Animator anim;
    public  void ReleaseTheMammoth()
    {
        GameObject mammoth = GameObject.FindGameObjectWithTag("Mammoth");
        mammoth.GetComponent<Rigidbody2D>().velocity = Vector2.right * mammothSpeed; 
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boulder")
        {
            anim.SetBool("isStunned", true);
            other.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
