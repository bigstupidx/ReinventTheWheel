using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform boulderPosition;
    public HoleMaker holeMakerScript;
    public float orthographicSize;
    private AudioSource audio;

    public List<GameObject> state;
    private int _currentState;
    private float timer;
    public bool chiseling;

	// Use this for initialization
	void Start () {
        //PlayerPrefs.SetInt("Tutorial", 0);
        audio = gameObject.GetComponent<AudioSource>();

        GetState();
        ChangeState();

        timer = holeMakerScript.timeToChisel;
        chiseling = false;
    }

    // This will change from the tutorial state into the actual game play
    // Using the PlayerPref, the state will be 0 if its the tutorial or 1
    // if it is not the tutorial
    void ChangeState() {
        StartCoroutine(CameraZoomOut());
    }

    // Will apply the changes of the state of the game (either tutorial state
    // or non-tutorial state)
    void GetState() {
        _currentState = PlayerPrefs.GetInt("Tutorial", 0);
        if (_currentState != 0)
            audio.Play();

        state[_currentState].SetActive(true);

        boulderPosition = state[_currentState].transform.GetChild(0);
        holeMakerScript = state[_currentState].transform.GetChild(0).GetComponent<HoleMaker>();
    }

    // Update is called once per frame
    void Update () {
        if (_currentState == 1) {
            transform.position = new Vector3(boulderPosition.position.x, boulderPosition.position.y, -10);
        } else {
            if (!chiseling) {
                Debug.Log(_currentState);
                if (_currentState == 0) {
                    StartCoroutine(Chisel());
                }
                //StartCoroutine(Chisel());
            }
        }
		
	}

    IEnumerator Chisel() {
        chiseling = true;
        yield return new WaitForSeconds(0.5f);

        holeMakerScript.GetPointOfImpact();
        holeMakerScript.MakeAHole();
        holeMakerScript.UpdateCollider();
        chiseling = false;
    }

    IEnumerator CameraZoomOut()
    {
        //Wait until the timer is at 0
        float time = holeMakerScript.timeToChisel;
        while (time> 0)
        {
            //Debug.Log(time);
            time -= Time.deltaTime;
            yield return null;
        }

        // Will only zoom out if the player is not on the tutorial state
        if (holeMakerScript.timeToChisel > 5) {
            while (Camera.main.orthographicSize < orthographicSize) {
                Camera.main.orthographicSize += Time.deltaTime;
                yield return null;
            }
        }

        // Checks to see if the tutorial is finished before changing states
        if (_currentState == 0) {
            holeMakerScript.chisel.SetActive(false);
            StartCoroutine(DelayBeforeChangingState());
        }
    }

    // Waits until the mammoth hits the rock before changing states
    IEnumerator DelayBeforeChangingState() {
        yield return new WaitForSeconds(2.5f);
        state[_currentState].SetActive(false);

        PlayerPrefs.SetInt("Tutorial", 1);
        GetState();
        ChangeState();
    }

}
