using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTracker : MonoBehaviour {

    public GameObject player;
    private Transform boulderPosition;
    private bool inMotion;

    // Testing
    public float currentPosition;
    public float finalPosition;

    public float points = 0;

	// Use this for initialization
	void Start () {
        boulderPosition = player.GetComponent<Transform>();
        currentPosition = boulderPosition.transform.position.x;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //currentPosition = boulderPosition.transform.position.x - currentPosition;
        if (!player.GetComponent<Rigidbody2D>().IsSleeping()) {
            Debug.Log(boulderPosition.transform.position.x + " - " +currentPosition);
            points += boulderPosition.transform.position.x - currentPosition;
        }
        //points += boulderPosition.transform.position.x - currentPosition;

        if (player.GetComponent<Rigidbody2D>().IsSleeping()) {
            
            Debug.Log(currentPosition);
        }
	}
}
