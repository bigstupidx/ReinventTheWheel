using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    
    private AudioSource audio;

    // Set this to the UI-Wheel-Hover in inspector
    public AudioClip boulderRoll;
    private Rigidbody2D rgb;


    private bool rollSet;
    public double minVelocity;

    void Start() {
        // Gets the Audio Source component
        audio = gameObject.GetComponent<AudioSource>();
        // Gets the Rigibody2D component in order to prevent the sound 
        // to be spammed when the boulder is rolling
        rgb = gameObject.GetComponent<Rigidbody2D>();

        // Used to set the audio clip to the rolling sound once
        rollSet = false;
    }

    void OnCollisionEnter2D(Collision2D col) {
        // Calls the SetSound() function in order to set the correct sound clip
        // or else the sound clip that is used to chisel will be played when the 
        // boulder is rolling
        if (!rollSet)
            SetSound();
        // Test to see if it hits the floor and also if the velocity is greater
        // than a number (I used 4.5 to test) and if it is, play the bouler
        // hitting the floor sound
        if (col.gameObject.name == "Floor" && rgb.velocity.x > minVelocity) {
            audio.Play();
        }
    }

    void SetSound() {
        // This is used to set the sound effect when the boulder is rolling
        // Need to set the boulderRoll to a sound clip in the inspector
        rollSet = true;
        if (boulderRoll != null)
            audio.clip = boulderRoll;
    }
}
