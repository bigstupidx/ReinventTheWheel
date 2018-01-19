using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MammothController : MonoBehaviour
{
    public float mammothSpeed;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip mammothBeginRunClip;
    public AudioClip mammothStopScreechClip;
    public AudioClip mammothBoulderCollideClip;
    public AudioClip mammothBoulderCollideDebrisClip;
    public AudioClip backgroundMusicAudioClip;
    public Rigidbody2D rb2d;
    public PolygonCollider2D polyCol;
    //used for the tutorial
    public bool release;

    private GameObject _mammoth;

    void Start()
    {
        //PlayerPrefs.SetInt("Tutorial", 0);
        release = false;
    }

    void Update()
    {
        if(release)
        {
            release = false;
            ReleaseTheMammoth();
        }
    }

    public void ReleaseTheMammoth()
    {
        _mammoth = gameObject;
        StartCoroutine(PlayMammothSounds());
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boulder")
        {
            //If there is any of the boulder left after chiseling   
            if (HoleMaker.hasPixels)
            {
                //audioSource.Stop();
                PlayMammothBoulderCollide();
                anim.SetBool("isStunned", true);
                Invoke("StopMammothVelocity", 1);
                other.GetComponent<Rigidbody2D>().gravityScale = 1;

                if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
                    StartCoroutine(RestartAfterTutorial());
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

    //The mammoth will fly off of the hill if there was no boulder to hit
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
        audioSource.PlayOneShot(mammothBeginRunClip);
    }

    public void PlayMammothBoulderCollide()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(mammothStopScreechClip);
        audioSource.PlayOneShot(mammothBoulderCollideClip);
        audioSource.PlayOneShot(mammothBoulderCollideDebrisClip);
    }

    IEnumerator PlayMammothSounds()
    {
        audioSource.PlayOneShot(mammothBeginRunClip);
        yield return new WaitForSeconds(1);
            
        rb2d.velocity = Vector2.right * mammothSpeed;
        audioSource.Play();
    }

    IEnumerator RestartAfterTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        PlayerPrefs.SetInt("Tutorial", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StopMammothVelocity()
    {
        //audioSource.clip = backgroundMusicAudioClip;
        audioSource.PlayOneShot(backgroundMusicAudioClip);
        rb2d.velocity = Vector2.zero;
    }
}
