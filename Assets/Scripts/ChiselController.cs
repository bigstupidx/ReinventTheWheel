using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiselController : MonoBehaviour
{
    public GameObject chiselAndHammer;
    public GameObject boulder;
    public Transform tutorialFinger;
    public static Animator anim;
    public float distanceOfChiselFromBoulder;

    private HoleMaker _holeMaker;
    

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        _holeMaker = boulder.GetComponent<HoleMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newChiselPosition;

        //have chisel move freely around the boulder along a circular path, controlled by the mouse
        if ((PlayerPrefs.GetInt("Tutorial", 0) != 0))       
             newChiselPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //have the chisel follow the tutorial finger
        else     
             newChiselPosition = tutorialFinger.position;

        

        Vector2 _directionTowardsBoulder = Vector3.Normalize((Vector2)boulder.transform.position - newChiselPosition);
        newChiselPosition = (Vector2)boulder.transform.position + (-_directionTowardsBoulder * distanceOfChiselFromBoulder);
        transform.position = Vector3.MoveTowards(transform.position, newChiselPosition, 1);
        transform.right = boulder.transform.position - transform.position;
        anim.SetBool("Charging", _holeMaker.charging);

        //flip the chisel so it's always right side up
        if (transform.position.x > boulder.transform.position.x)
            transform.localScale = new Vector2(-1, -1);

        else
            transform.localScale = new Vector2(-1, 1);

    }
}
