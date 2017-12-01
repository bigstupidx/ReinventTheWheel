using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MammothController : MonoBehaviour
{
    public float mammothSpeed;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip mammothSound, mammothBoulderCollideClip,backgroundMusicAudioClip;
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
            PlayMammothBoulderCollide();
            anim.SetBool("isStunned", true);
            other.GetComponent<Rigidbody2D>().gravityScale = 1;
            Invoke("StopMammothVelocity", 1);
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
            
        _rb2d.velocity = Vector2.right * mammothSpeed;
        audioSource.Play();
    }
    private void StopMammothVelocity()
    {
        audioSource.clip = backgroundMusicAudioClip;
        audioSource.Play();
        _rb2d.velocity = Vector2.zero;
    }
}
