using System.Collections;
using UnityEngine;


/**
 * Author: Ruben Sanchez
 * Defines the carving behavior:
 * - Makes a copy of the sprite on awake so that the changes made do not persist, sets the spriteRenderer's sprite to the clone sprite
 * - Translates a clickpoint to the surface of the boulder collider, creating the pointOfImpact
 * - Iterates through the pixels of the clone sprite, checks if each pixel is within a predefined polygon surrounding the pointOfImpact, makes them transparent if so
 * - Updates the PolyCollider to cut out the now transparent pixels
 **/

public class HoleMaker : MonoBehaviour
{
    public GameObject boulder;
    public GameObject chisel;
    //to be turned off when paused and when the timer is done
    public static bool activated;
    //used to allow boulder to fly off the cliff
    public static bool hasPixels;
    //size used to clear the chunks off the boulder
    public float chipSize;
    //the time the player will have to chisel the boulder 
    public float timeToChisel;
    //max amount of time where the click/touch hold will increase the chip size
    public float maxChargeTime;
    //multiplier to increase the chip size;
    public float increaseChipSizeBy;
    public SpriteRenderer spriteRen;
    public Sprite[] sprites;
    //used to check if the player is currently holding to charge a chisel
    public bool charging;
    public float distanceOfChiselFromBoulder;


    //the surface edge of the boulder that is in line with the boulder center and the click point
    private Vector2 _pointOfImpact;
    //the clone of the sprite that is created on awake in order to not modify the original sprite
    private Texture2D _textureClone;
    //amount of time that the player holds a click/touch
    private float _timeHeldDown;
    //the increased chip size after the player holds a click/touch
    private float _increasedChip;
    //degrees that the boulder is rotated by at the beginning of the game
    private float _theta;
    //point offset from the boulder, used to rayCast towards the boulder to find the surface
    private Vector2 _pointOutsideOfBoulder;
    private Animator _chiselAnim;

    // Sound Effect Stuff
    public AudioClip[] chiselSoundClips;
    public AudioSource chiselAudioSource;
    public GameObject chipsParticleSystem;

    void Awake()
    {
       
    }
	// Use this for initialization
	void Start ()
    {
        _chiselAnim = boulder.GetComponent<Animator>();

        hasPixels = true;

        spriteRen.sprite = sprites[Random.Range(0, sprites.Length)];
        boulder.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    

        //make a copy of the sprite
        MakeCopy();
        //Update the collider in case copying the sprite changed its pivot slightly
        UpdateCollider();

        _timeHeldDown = 0;
        activated = true;
        charging = false;
        _increasedChip = chipSize;

        ///////////////////////////////////////////////////////////////////////
        ////////////// Sound Effect Initializer ///////////////////////////////
        // Gets the audio source in order to play a chisel sound when the
        // player is clicking to chisel
        // Sets the audio clip to the chisel sound that was set in the inspector
        // and is going to be used when the player is chiseling the rock
        if (chisel != null)
            chiselAudioSource.clip = chiselSoundClips[Random.Range(0,chiselSoundClips.Length)];
        else
            Debug.Log("Set the chisel sound in the inspector");
        ////////////// End Sound Effect Initializer ///////////////////////////
        ///////////////////////////////////////////////////////////////////////
    }


    // Update is called once per frame
    void Update ()
    {
        //if the tutorial has already been played, allow normal controls
        if ((PlayerPrefs.GetInt("Tutorial", 0) != 0)) {

            //add to the charge timer if the player is holding down a click/touch
            if (Input.GetMouseButton(0) && activated && _timeHeldDown < maxChargeTime)
            {
                _timeHeldDown += Time.deltaTime;

                if (_timeHeldDown >= .2f)
                    charging = true;
            }

            //strike on click/touch, add charge if player held down click/touch
            if ((Input.GetMouseButtonUp(0) && charging) || ((Input.GetMouseButtonDown(0)) && !charging) && activated)
            {
                Instantiate(chipsParticleSystem, gameObject.transform.position, Quaternion.identity);
                chiselAudioSource.clip = chiselSoundClips[Random.Range(0, chiselSoundClips.Length)];
                chiselAudioSource.Play();
                if (charging)
                {
                    _increasedChip = chipSize + ((_timeHeldDown / maxChargeTime) * increaseChipSizeBy);
                }
                    

                else
                    _increasedChip = chipSize;

                GetPointOfImpact();
                MakeAHole();
                UpdateCollider();

                
                charging = false;
                
                if(chisel.activeInHierarchy)
                {
                    ChiselController.anim.SetTrigger("Strike");
                }
               
                _timeHeldDown = 0;
            }
        }
    }

    //Gets the new point to chisel, rotates around the boulder after every chisel,
    //then finds the surface point along that line
    //then offsets a little bit away from the surface, that new point is will be the center of the circle to carve
    public void GetPointOfImpact()
    {
        Vector2 _directionTowardsBoulder = Vector2.zero;
        Vector2 clickPoint = Vector2.zero;

        if ((PlayerPrefs.GetInt("Tutorial", 0) != 0)) {
            clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _directionTowardsBoulder = Vector3.Normalize((Vector2)boulder.transform.position - clickPoint);

            //_pointOutsideOfBoulder = (Vector2)boulder.transform.position + (-_directionTowardsBoulder * 50);

        } else {
            Vector3 pos = new Vector3(Random.RandomRange(-50, 50), Random.RandomRange(-50, 50), 0);

            //Debug.Log(pos);
            Vector2 newChiselPosition = pos;
            _directionTowardsBoulder = Vector3.Normalize((Vector2)boulder.transform.position - newChiselPosition);
            newChiselPosition = (Vector2)boulder.transform.position + (-_directionTowardsBoulder * distanceOfChiselFromBoulder);
            chisel.transform.position = Vector3.MoveTowards(chisel.transform.position, newChiselPosition, 1);
            clickPoint = Camera.main.ScreenToWorldPoint(chisel.transform.position);

            Instantiate(chipsParticleSystem, gameObject.transform.position, Quaternion.identity);
            chiselAudioSource.clip = chiselSoundClips[Random.Range(0, chiselSoundClips.Length)];
            chiselAudioSource.Play();
        }

        _pointOutsideOfBoulder = (Vector2)boulder.transform.position + (-_directionTowardsBoulder * 50);


        _theta = boulder.transform.rotation.eulerAngles.z;

        _pointOfImpact = Vector3.zero;

        RaycastHit2D[] hit = Physics2D.RaycastAll(_pointOutsideOfBoulder, _directionTowardsBoulder);
       
        //find the point where it hit the surface of the boulder
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null && hit[i].collider.gameObject == boulder)
            {
                _pointOfImpact = hit[i].point;
            }

        }

        //move the point of impact farther away, will be used as a radius for a circle
        _pointOfImpact += ((-(_directionTowardsBoulder * _increasedChip * (charging ? .03f : .05f))));

        Debug.DrawLine((Vector2)boulder.transform.position, _pointOfImpact, Color.red, 1);


        //make the impact coordinates relative to the sprite
        _pointOfImpact = WorldToPixel(_pointOfImpact);
    }

    public void MakeAHole()
    {
        //will be set to true if/when the loop finds pixels, otherwise the mammoth will fly off to valhalla
        hasPixels = false;
        //iterate through the pixels and see if the pixel coordinates are within the shape that we are carving out, 
        //make them transparent if they meet the condition

        for (int x = 0; x < _textureClone.width; x++)
        {
            for (int y = 0; y < _textureClone.height; y++)
            {
                //get the color of the pixel at the current coordinates
                Color color = _textureClone.GetPixel(x, y);

                //if the pixel is already transparent, go to the next iteration
                if (color.a == 0)
                    continue;

                else 
                    hasPixels = true;

                //Debug.DrawLine(transform.position,_pointOfImpact, Color.red, 3);
                float distance = Mathf.Sqrt(Mathf.Pow((x - _pointOfImpact.x), 2) + Mathf.Pow((y - _pointOfImpact.y), 2));

                //if the pixel is within the shape, make it transparent
                if (distance < _increasedChip * 5.5f)
                {
                    color.a = 0;
                    _textureClone.SetPixel(x, y, color);
                }

            }
        }

        //apply the changes to the sprite
        _textureClone.Apply();
    }

    //remove and add a polyCollider, it will now cut out the newly transparent pixels
    //if theres a chunk floating in the air, then that will be a separate path of the collider; remove it
    //checks to see if a pixel is no longer within the collider, removes it if it's outside
    public void UpdateCollider()
    {
        Destroy(boulder.GetComponent<PolygonCollider2D>());
        PolygonCollider2D collider = boulder.AddComponent<PolygonCollider2D>();

        //if there's more than one path, keep the biggest path, remove all the rest
        if (collider.pathCount > 1)
        {
            //index of the array of different paths of the collider
            int indexOfBiggestPath = 0;
            //the path with the biggest area will be the remaining boulder
            float biggestArea = 0;

            for (int i = 0; i < collider.pathCount; i++)
            {
                if (GetPolygonArea(collider.GetPath(i)) > biggestArea)
                {
                    biggestArea = GetPolygonArea(collider.GetPath(i));
                    indexOfBiggestPath = i;
                }
            }

            //set all the points of the separate paths to Vector2.Zero
            for (int j = 0; j < collider.pathCount; j++)
            {
                if (j != indexOfBiggestPath)
                {
                    Vector2[] points = collider.GetPath(j);

                    Vector2[] zeroPoints = new Vector2[points.Length];

                    for (int k = 0; k < points.Length; k++)
                    {
                        zeroPoints[k] = Vector2.zero;
                    }

                    collider.SetPath(j, zeroPoints);
                }
            }

            //iterate through the pixels and see if the pixel coordinates are within the main collider
            //make them transparent if they are not
            for (int x = 0; x < _textureClone.width; x++)
            {
                for (int y = 0; y < _textureClone.height; y++)
                {
                    //get the color of the pixel at the current coordinates
                    Color color = _textureClone.GetPixel(x, y);

                    Vector2 pixelWorldSpace = new Vector2(((x - spriteRen.sprite.rect.width / 2) / spriteRen.sprite.pixelsPerUnit) + transform.position.x, ((y - spriteRen.sprite.rect.height / 2) / spriteRen.sprite.pixelsPerUnit) + transform.position.y);
                    //check if the current pixel is within the collider
                    if (!collider.OverlapPoint(pixelWorldSpace))
                    {
                        color.a = 0;
                        _textureClone.SetPixel(x, y, color);
                    }
                }
            }
        }
    }

    //get the area of a polygon
    public float GetPolygonArea(Vector2[] points)
    {

        float area = 0;

        for (int i = 0; i < points.Length - 1; i++)
        {
            area += (points[i].x * points[i + 1].y) - (points[i].y * points[i + 1].x);
        }

        area += (points[points.Length - 1].x * points[0].y) - (points[points.Length - 1].y * points[0].x);


        area = Mathf.Abs(area / 2);

        return area;
    }

    //if the boulder touches the ground while it's fully transparent, don't let it go anywhere
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && !hasPixels)
            boulder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    //get the impact point relative to the boulder, and account for local gameObject scale as well as pixelsPerUnit
    //NOTE: Currently, the boulder has to have a z rotation of 0 for this to work as expected
    public Vector2 WorldToPixel(Vector2 pointOfImpact)
    {
        //make the point of impact relative to the boulder
        Vector2 impactRelativeToBoulder = pointOfImpact - (Vector2)boulder.transform.position;
        
        //account for rotation of the boulder
        if(_theta != 0)
        {
            impactRelativeToBoulder = Quaternion.AngleAxis(-_theta, Vector3.forward) * impactRelativeToBoulder;
        }


        float width = spriteRen.sprite.rect.width;
        float height = spriteRen.sprite.rect.height;
        float xScale = boulder.transform.localScale.x;
        float yScale = boulder.transform.localScale.y;
        float pixelsPerUnit = spriteRen.sprite.pixelsPerUnit;

        //convert the coordinates using pixels, if the pixel coordinates match up with the sprite coordinates, those pixels will be cleared
        impactRelativeToBoulder.x *= (pixelsPerUnit / xScale);
        impactRelativeToBoulder.y *= (pixelsPerUnit / yScale);
        impactRelativeToBoulder.x += (width / 2f);
        impactRelativeToBoulder.y += (height / 2f);


        return impactRelativeToBoulder;
    }

    //make a copy of the sprite to carve it out, otherwise the changes made will persist 
    private void MakeCopy()
    {      
        Texture2D texture = spriteRen.sprite.texture;
        _textureClone = Instantiate(texture);
        Sprite oldSprite = spriteRen.sprite;
        spriteRen.sprite = Sprite.Create(_textureClone, spriteRen.sprite.textureRect, Vector2.one * .5f, oldSprite.pixelsPerUnit, 0, SpriteMeshType.Tight, oldSprite.border);
        spriteRen.sprite.name = oldSprite.name + " Clone";
       
    }

}
