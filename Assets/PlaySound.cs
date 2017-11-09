using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    private AudioSource audio;

    [SerializeField]
    // Set to 0.3f for testing
    private float delayTime;

    void Start() {
        // Gets the Audio Source component
        audio = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        // Test to see if it hits the floor
        if (col.name == "Floor") {
            // Plays the audio sound and then delays before destroying the 
            // SoundEffect gameObject that stores the audio source
            audio.Play();
            StartCoroutine(Delay());
        }
    }

    // Delays before destroying the SoundEffect Object
    IEnumerator Delay() {
        yield return new WaitForSeconds(delayTime);
        // Destroys the gameObject to avoid the sound
        // from being played on every cycle
        Destroy(gameObject);
    }
}
