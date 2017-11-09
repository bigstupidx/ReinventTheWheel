using System.Collections;
using System.Collections.Generic;
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
    //to be turned off when paused and when the timer is done
    public static bool activated;
    //size used to clear the chunks off the boulder
    public float chipSize;
    //the time the player will have to chisel the boulder 
    public float timer;
    public SpriteRenderer spriteRen;

    //the surface edge of the boulder that is in line with the boulder center and the click point
    private Vector2 _pointOfImpact;
    //the clone of the sprite that is created on awake in order to not modify the original sprite
    private Texture2D _textureClone;
    private Vector2 _directionTowardsBoulder;


    void Awake()
    {
        //make a copy of the sprite
        MakeCopy();
        //Update the collider in case copying the sprite changed its pivot slightly
        UpdateCollider();
    }

	// Use this for initialization
	void Start ()
    {
        activated = true;
        Invoke("SetGravity", timer);
    }


    // Update is called once per frame
    void Update ()
    {
		if(Input.GetMouseButtonDown(0) && activated)
        {
            //Get the click point, and get the point on the boulder collider surface in line with the click point and center of the boulder 
            Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _directionTowardsBoulder = Vector3.Normalize((Vector2)transform.position - clickPoint);
          
            Vector2 pointOutsideOfBoulder = (Vector2)transform.position +  (-_directionTowardsBoulder * 20);
            _pointOfImpact = Vector3.zero;

            RaycastHit2D[] hit = Physics2D.RaycastAll(pointOutsideOfBoulder, _directionTowardsBoulder);
            
            //find the point where it hit the surface of the boulder
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider != null && hit[i].collider.gameObject == this.gameObject)
                {
                    _pointOfImpact = hit[i].point;
                }
                        
            }
           
            //move the point of impact farther away, will be used as a radius for a circle
            _pointOfImpact += ((-(_directionTowardsBoulder * chipSize * .05f)));
            //make the impact coordinates relative to the sprite
            _pointOfImpact = WorldToPixel(_pointOfImpact);

            //carve the hole
            MakeAHole();
            UpdateCollider();
        }
	}

    //After the timer, activate gravity on the boulder to start rolling
    public void SetGravity()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1;
        activated = false;
    }

    public void MakeAHole()
    {
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

                //Debug.DrawLine(transform.position,_pointOfImpact, Color.red, 3);
                float distance = Mathf.Sqrt(Mathf.Pow((x - _pointOfImpact.x), 2) + Mathf.Pow((y - _pointOfImpact.y), 2));

                //if the pixel is within the shape, make it transparent
                if (distance < chipSize * 5.5f)
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
        Destroy(GetComponent<PolygonCollider2D>());
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();

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

    //get the impact point relative to the boulder, and account for local gameObject scale as well as pixelsPerUnit
    //NOTE: Currently, the boulder has to have a z rotation of 0 for this to work as expected
    public Vector2 WorldToPixel(Vector2 pointOfImpact)
    {
        //make the point of impact relative to the boulder
        Vector2 impactRelativeToBoulder = pointOfImpact - (Vector2)transform.position;

        float width = spriteRen.sprite.rect.width;
        float height = spriteRen.sprite.rect.height;
        float angle = transform.rotation.z;
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
        float pixelsPerUnit = spriteRen.sprite.pixelsPerUnit;

        //impactRelativeToBoulder.x = impactRelativeToBoulder.x * Mathf.Cos(angle) - impactRelativeToBoulder.y * Mathf.Sin(angle);
        //impactRelativeToBoulder.y = (impactRelativeToBoulder.x * Mathf.Sin(angle) + impactRelativeToBoulder.y * Mathf.Cos(angle));

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
        spriteRen.sprite = Sprite.Create(_textureClone, spriteRen.sprite.textureRect, Vector2.one * 0.5f, oldSprite.pixelsPerUnit, 0, SpriteMeshType.Tight, oldSprite.border);
        spriteRen.sprite.name = "Clone";
    }


}
