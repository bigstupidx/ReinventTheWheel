using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemanBystanderScream : MonoBehaviour {
    public GameObject boulder;
    public AudioSource screamAudioSource;
    public AudioSource squishAudioSource;
    public AudioClip[] screamClips;
    public AudioClip[] squishClips;

    private bool executedOnce;
	// Use this for initialization
	void Start () {
        executedOnce = false;
        boulder = GameObject.FindGameObjectWithTag("Boulder");
        screamAudioSource = GameObject.Find("BystanderScreamAudioObject").GetComponent<AudioSource>();
        squishAudioSource = GameObject.Find("BystanderSquishAudioObject").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(boulder.transform.position.x - transform.position.x) <= 1f && !executedOnce)
        {
            executedOnce = true;
            screamAudioSource.clip = screamClips[UnityEngine.Random.Range(0, screamClips.Length)];
            screamAudioSource.Play();
            squishAudioSource.clip = squishClips[Random.Range(0, squishClips.Length)];
            squishAudioSource.Play();
            GetComponent<Animator>().SetTrigger("Hit");
        }
	}
}
