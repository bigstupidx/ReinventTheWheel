using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTracker : MonoBehaviour {
    
    // Reference to player object, AKA boulder
    public GameObject player;

    // Transform of boulder
    private Transform boulderPosition;

    // Last position of boulder
    public float lastPosition;

    // Total points the player has, AKA distance travelled
    public float points = 0;

    // Testing Variables
    //private bool inMotion;
    //public float currentPosition;
    //public float finalPosition;

	// Use this for initialization
	void Start () {
        boulderPosition = player.GetComponent<Transform>();
        lastPosition = boulderPosition.transform.position.x;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        points += boulderPosition.transform.position.x - lastPosition;
        lastPosition = boulderPosition.transform.position.x;
	}
}
