using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    private AudioSource audio;

    void Start() {
        // Gets the Audio Source component
        audio = gameObject.GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D col) {
        // Test to see if it hits the floor
        if (col.gameObject.name == "Floor") {
            // Plays the audio sound and then delays before destroying the 
            // SoundEffect gameObject that stores the audio source
            audio.Play();
        }
    }
}
