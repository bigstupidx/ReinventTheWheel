using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemanBystanderScream : MonoBehaviour {
    public GameObject boulder;
    public AudioSource screamAudioSource;
    public AudioClip[] screamClips;
    private bool executedOnce;
	// Use this for initialization
	void Start () {
        executedOnce = false;
        boulder = GameObject.FindGameObjectWithTag("Boulder");
        screamAudioSource = GameObject.Find("BystanderScreamAudioObject").GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(boulder.transform.position.x - transform.position.x) <= 1f && !executedOnce)
        {
            executedOnce = true;
            screamAudioSource.clip = screamClips[UnityEngine.Random.Range(0, screamClips.Length)];
            screamAudioSource.Play();
            Destroy(gameObject);
        }
	}
}
