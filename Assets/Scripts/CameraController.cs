using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform boulderPosition;
    public Transform bottomFloor;
    public HoleMaker holeMakerScript;
    //x coordinate to move to camera up once the ground is flat
    public float xPositionToMoveUp;
    public float amountToMoveUpBy;
    public float orthographicSize;

    private float timer;
    private bool _movingUp;

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
        {
            if (boulderPosition.position.x >= xPositionToMoveUp && !_movingUp)
            {
                _movingUp = true;
                print("started Co");
                StartCoroutine(MoveToTargetHeight());
            }

            //follow the boulder directly in the center
            if (boulderPosition.position.x < xPositionToMoveUp && !_movingUp)  
            transform.position = new Vector3(boulderPosition.position.x, boulderPosition.position.y, -10);

            //follow while keeping the vertical offset
            else if (_movingUp)
                transform.position = new Vector3(boulderPosition.position.x, transform.position.y, -10);
        }
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

    IEnumerator MoveToTargetHeight()
    {
        //if below the target height, move up
        if (transform.position.y < (bottomFloor.position.y + amountToMoveUpBy))
        {
            while (transform.position.y < (bottomFloor.position.y + amountToMoveUpBy))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (Time.deltaTime * 1.5f), -10);

                yield return null;
            }
        }

        //if above the target height due to a boulder bounce, move down
        else
        {
            while (transform.position.y > (bottomFloor.position.y + amountToMoveUpBy))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * 2), -10);

                yield return null;
            }
        }
    }

}
