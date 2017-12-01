using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MammothController : MonoBehaviour
{
    public float mammothSpeed;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip mammothSound, mammothStampede;
    private GameObject _mammoth;
    private Rigidbody2D _rb2d;
    public  void ReleaseTheMammoth()
    {
        _mammoth = gameObject;
        _rb2d = _mammoth.GetComponent<Rigidbody2D>();       
        StartCoroutine(PlayMammothSounds());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boulder")
        {
            audioSource.Stop();
            anim.SetBool("isStunned", true);
            other.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
    public void PlayMammothSound()
    {
        audioSource.PlayOneShot(mammothSound);
    }
    public void PlayMammothStampede()
    {
        audioSource.PlayOneShot(mammothStampede);
    }
    IEnumerator PlayMammothSounds()
    {
        audioSource.PlayOneShot(mammothSound);
        yield return new WaitForSeconds(1);
            
        _rb2d.velocity = Vector2.right * mammothSpeed;
        audioSource.Play();
    }
}
