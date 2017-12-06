using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MammothController : MonoBehaviour
{
    public float mammothSpeed;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip mammothSound, mammothBoulderCollideClip,backgroundMusicAudioClip;
    public Rigidbody2D rb2d;
    public PolygonCollider2D polyCol;

    private GameObject _mammoth;
    
    public  void ReleaseTheMammoth()
    {
        _mammoth = gameObject;
        StartCoroutine(PlayMammothSounds());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boulder")
        {
            
            if (HoleMaker.hasPixels)
            {
                audioSource.Stop();
                PlayMammothBoulderCollide();
                anim.SetBool("isStunned", true);
                Invoke("StopMammothVelocity", 1);
                other.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            
            else
            {
                polyCol.isTrigger = true;
                rb2d.gravityScale = 0;
                other.GetComponent<Rigidbody2D>().gravityScale = .15f;
                other.GetComponent<Rigidbody2D>().drag = 5;

            }


        }

       
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Floor")
        {
            anim.SetBool("isStunned", true);
            rb2d.angularVelocity = -300;
            rb2d.gravityScale = .2f;
            audioSource.Stop();

        }
    }
    public void PlayMammothSound()
    {
        audioSource.PlayOneShot(mammothSound);
    }
    public void PlayMammothBoulderCollide()
    {
        audioSource.PlayOneShot(mammothBoulderCollideClip);
    }


    IEnumerator PlayMammothSounds()
    {
        audioSource.PlayOneShot(mammothSound);
        yield return new WaitForSeconds(1);
            
        rb2d.velocity = Vector2.right * mammothSpeed;
        audioSource.Play();
    }
    private void StopMammothVelocity()
    {
        audioSource.clip = backgroundMusicAudioClip;
        audioSource.Play();
        rb2d.velocity = Vector2.zero;
    }
}
