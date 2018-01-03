using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiselController : MonoBehaviour
{
    public GameObject chiselAndHammer;
    public GameObject boulder;
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
	void Update ()
    {
        if ((PlayerPrefs.GetInt("Tutorial", 0) != 0))
        {
            //have chisel move freely around the boulder along a circular path, controlled by the mouse
            Vector2 newChiselPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _directionTowardsBoulder = Vector3.Normalize((Vector2)boulder.transform.position - newChiselPosition);
            newChiselPosition = (Vector2)boulder.transform.position + (-_directionTowardsBoulder * distanceOfChiselFromBoulder);
            transform.position = Vector3.MoveTowards(transform.position, newChiselPosition, 1);
        }

        if (Input.GetMouseButtonDown(0))       
        anim.SetTrigger("Strike");
        
        
        anim.SetBool("Charging", _holeMaker.charging);


        //if (Input.GetMouseButtonUp(0) && _holeMaker.charging)
        //    anim.SetTrigger("Strike");

        //Vector2 directionTowardsBoulder = (Vector2)(Vector3.Normalize(boulder.transform.position - transform.position));
        //float theta = Mathf.Atan(directionTowardsBoulder.y / directionTowardsBoulder.x) * Mathf.Rad2Deg;

        //transform.rotation = Quaternion.Euler(0, 0, theta);

        transform.right = boulder.transform.position - transform.position;

        if(transform.position.x > boulder.transform.position.x)
            transform.localScale = new Vector2(-1, -1);

        else
            transform.localScale = new Vector2(-1, 1);

        //if ((transform.rotation.eulerAngles.z < -90 && transform.rotation.eulerAngles.z > -180) || (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 180))
        //    transform.localScale = new Vector2(-1, -1);

        //else if ((transform.rotation.eulerAngles.z <= 90 && transform.rotation.eulerAngles.z >= 0) || (transform.rotation.eulerAngles.z >= -90 && transform.rotation.eulerAngles.z >= 0))
        //    transform.localScale = new Vector2(-1, 1);

    }
}
