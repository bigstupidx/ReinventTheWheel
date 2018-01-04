using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform boulderPosition;
    public HoleMaker holeMakerScript;
    public float orthographicSize;
    private float timer;

	// Use this for initialization
	void Start ()
    {
        timer = holeMakerScript.timeToChisel;
    }

    // This will change from the tutorial state into the actual game play
    // Using the PlayerPref, the state will be 0 if its the tutorial or 1
    // if it is not the tutorial
    public void ChangeState() {
        StartCoroutine(CameraZoomOut());
    }

    // Update is called once per frame
    void Update ()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) != 0)  
            transform.position = new Vector3(boulderPosition.position.x, boulderPosition.position.y, -10);      
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
        if (holeMakerScript.timeToChisel > 5)
        {
            //_currentState = 1;
            while (Camera.main.orthographicSize < orthographicSize)
            {
                Camera.main.orthographicSize += Time.deltaTime;
                yield return null;
            }
        }
    }

}
