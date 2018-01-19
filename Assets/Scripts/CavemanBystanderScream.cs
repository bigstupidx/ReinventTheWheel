using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemanBystanderScream : MonoBehaviour
{
    public GameObject boulder;
    public AudioSource screamAudioSource;
    public AudioSource squishAudioSource;
    public AudioClip[] screamClips;
    public AudioClip[] squishClips;

    private bool _executedOnce;

	// Use this for initialization
	void Start ()
    {
        _executedOnce = false;
        boulder = GameObject.FindGameObjectWithTag("Boulder");
        screamAudioSource = GameObject.Find("BystanderScreamAudioObject").GetComponent<AudioSource>();
        squishAudioSource = GameObject.Find("BystanderSquishAudioObject").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Mathf.Abs(boulder.transform.position.x - transform.position.x) <= 1f && !_executedOnce)
        {
            _executedOnce = true;
            screamAudioSource.PlayOneShot(screamClips[UnityEngine.Random.Range(0, screamClips.Length)]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Boulder") && !_executedOnce)
        {
            _executedOnce = true;
            screamAudioSource.PlayOneShot(screamClips[UnityEngine.Random.Range(0, screamClips.Length)]);
            squishAudioSource.PlayOneShot(squishClips[Random.Range(0, squishClips.Length)]);
            GetComponent<Animator>().SetTrigger("Hit");
        }

    }
}
