using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform boulderPosition;
    public HoleMaker holeMakerScript;
    public float orthographicSize;
	// Use this for initialization
	void Start () {
        StartCoroutine(CameraZoomOut());
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(boulderPosition.position.x, boulderPosition.position.y, -10);
	}

    IEnumerator CameraZoomOut()
    {
        //Wait until the timer is at 0
        float time = holeMakerScript.timer;
        while (time> 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        while(Camera.main.orthographicSize < orthographicSize)
        {
            Camera.main.orthographicSize += Time.deltaTime;
            yield return null;
        }
    }


}
