using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    public AudioSource boulderAudioSource;
    public AudioClip[] boulderAudioClips;


    void Start() {
        // Gets the Audio Source component
        //audio = gameObject.GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D col) {
        // Test to see if it hits the floor
        if (col.gameObject.tag == "Floor" && HoleMaker.hasPixels) {
            // Plays the audio sound and then delays before destroying the 
            // SoundEffect gameObject that stores the audio source
            boulderAudioSource.clip = boulderAudioClips[Random.Range(0, boulderAudioClips.Length)];
            boulderAudioSource.Play();
        }
    }
}
