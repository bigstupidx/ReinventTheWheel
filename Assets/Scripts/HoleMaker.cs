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
    //sizes used to clear the shapes in the boulder
    public float circleRadius;
    public float squareSideLength;
    public float triangleSideLength;
    public float cloverRadiusLength;
    public float starSideLength;
    //the time the player will have to chisel the boulder 
    public float timer;
    public SpriteRenderer spriteRen;
    //different shapes to clear from the boulder
    public enum HoleShapes { Circle, Square, Triangle, Clover, Star };
    //the surface edge of the boulder that is in line with the boulder center and the click point
    private Vector2 _pointOfImpact;
    //the clone of the sprite that is created at Start in order to not modify the original sprite
    private Texture2D _textureClone;
    //variable to check which shaped hole to clear next
    private HoleShapes _currentHole;
   
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
        _currentHole = HoleShapes.Star;
        Invoke("SetGravity", timer);
    }

    //After the timer, activate gravity on the boulder to start rolling
    public void SetGravity()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            //Get the click point, and get the point on the boulder collider surface in line with the click point and center of the boulder 
            Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionTowardsBoulder = Vector3.Normalize((Vector2)transform.position - clickPoint);
          
            Vector2 pointOutsideOfBoulder = (Vector2)transform.position +  (-directionTowardsBoulder * 20);
            _pointOfImpact = Vector3.zero;

            RaycastHit2D[] hit = Physics2D.RaycastAll(pointOutsideOfBoulder, directionTowardsBoulder);
            
            //find the point where it hit the surface of the boulder
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider != null && hit[i].collider.gameObject == this.gameObject)
                {
                    _pointOfImpact = hit[i].point;
                }
                        
            }
            Debug.DrawLine(clickPoint, _pointOfImpact, Color.red, 2);

            //make the impact coordinates relative to the sprite
            _pointOfImpact = WorldToPixel(_pointOfImpact);

            //carve the hole
            MakeAHole();
            UpdateCollider();
            //choose the next shape to carve with
            UpdateHoleShape();
        }
	}

    public void MakeAHole()
    {
        //iterate through the pixels and see if the pixel coordinates are within the shape that we are carving out, 
        //make them transparent if they meet the conditions
        for (int x = 0; x < _textureClone.width; x++)
        {
            for (int y = 0; y < _textureClone.height; y++)
            {
                //get the color of the pixel at the current coordinates
                Color color = _textureClone.GetPixel(x, y);

                //if the pixel is already transparent, go to the next iteration
                if (color.a == 0)
                    continue;

                //each shape will have a different condition that needs to be met
                bool clearConditionMet = false;

                //cut a circular hole with _pointOfImpact as the center and circleRadius as the radius 
                if (_currentHole == HoleShapes.Circle)
                {
                    float distance = Mathf.Sqrt(Mathf.Pow((x - _pointOfImpact.x), 2) + Mathf.Pow((y - _pointOfImpact.y), 2));

                    if (distance < circleRadius)
                        clearConditionMet = true;
                }

                //cut a square hole with _pointOfImpact as the center and squareSideLength as the distance of the edges from the center
                else if (_currentHole == HoleShapes.Square)
                {
                    if (Mathf.Abs(_pointOfImpact.x - x) < squareSideLength && Mathf.Abs(_pointOfImpact.y - y) < squareSideLength)
                        clearConditionMet = true;
                }

                //cut a triangular hole with _pointOfImpact as the middle of the baseline and triangleSideLength as the side lengths, 
                //If above the center, have a vertex pointing down, else have a vertex pointing up
                else if (_currentHole == HoleShapes.Triangle)
                {
                    //if the pointOfImpact is below the center of the boulder, have one of the vertexes pointing up
                    if(_pointOfImpact.y <  WorldToPixel((Vector2)transform.position).y)
                    {
                        //check to see if the coordinate is within the left side of the /_\ triangle
                        if (x <= _pointOfImpact.x && x >= (_pointOfImpact.x - triangleSideLength) && y >= _pointOfImpact.y && y <= _pointOfImpact.y + ((x - _pointOfImpact.x) + triangleSideLength))
                            clearConditionMet = true;

                        else if (x >= _pointOfImpact.x && x <= (_pointOfImpact.x + triangleSideLength) && y >= _pointOfImpact.y && y <= _pointOfImpact.y + (-(x - _pointOfImpact.x) + triangleSideLength))
                            clearConditionMet = true;
                    }

                    //else have one of the vertexes pointing down
                    else
                    {
                        //check to see if the coordinate is within the left side of the \--/ triangle
                        if (x <= _pointOfImpact.x && x >= (_pointOfImpact.x - triangleSideLength) && y <= _pointOfImpact.y && y >= _pointOfImpact.y + (-(x - _pointOfImpact.x) - triangleSideLength))
                            clearConditionMet = true;

                        else if (x >= _pointOfImpact.x && x <= (_pointOfImpact.x + triangleSideLength) && y <= _pointOfImpact.y && y >= _pointOfImpact.y + ((x - _pointOfImpact.x) - triangleSideLength))
                            clearConditionMet = true;
                    }
                 
                }

                //cut a clover hole with 3 circles and and a radius size of cloverRadiusLength
                if (_currentHole == HoleShapes.Clover)
                {
                    //Three overlapping triangles, clear pixel if it's within any of the three
                    float distance1 = Mathf.Sqrt(Mathf.Pow((x - _pointOfImpact.x), 2) + Mathf.Pow((y - _pointOfImpact.y), 2));

                    float distance2 = Mathf.Sqrt(Mathf.Pow((x - (_pointOfImpact.x + cloverRadiusLength)), 2) + Mathf.Pow((y - _pointOfImpact.y), 2));

                    float distance3 = Mathf.Sqrt(Mathf.Pow((x - (_pointOfImpact.x + (cloverRadiusLength * .5f))), 2) + Mathf.Pow((y - (_pointOfImpact.y + cloverRadiusLength)), 2));

                    if (distance1 < cloverRadiusLength || distance2 < cloverRadiusLength || distance3 < cloverRadiusLength)
                        clearConditionMet = true;
                }

                if (_currentHole == HoleShapes.Star)
                {
                    //check to see if the coordinate is within the left side of the /_\ triangle
                    if (x <= _pointOfImpact.x && x >= (_pointOfImpact.x - starSideLength) && y >= _pointOfImpact.y && y <= _pointOfImpact.y + ((x - _pointOfImpact.x) + starSideLength))
                        clearConditionMet = true;

                     if (x >= _pointOfImpact.x && x <= (_pointOfImpact.x + starSideLength) && y >= _pointOfImpact.y && y <= _pointOfImpact.y + (-(x - _pointOfImpact.x) + starSideLength))
                        clearConditionMet = true;

                    //check to see if the coordinate is within the left side of the \--/ triangle
                     if (x <= _pointOfImpact.x && x >= (_pointOfImpact.x - starSideLength) && y <= (_pointOfImpact.y + (.7f * starSideLength)) && y >= (_pointOfImpact.y + (.7f * starSideLength)) + (-(x - _pointOfImpact.x) - starSideLength))
                        clearConditionMet = true;

                     if (x >= _pointOfImpact.x && x <= (_pointOfImpact.x + starSideLength) && y <= (_pointOfImpact.y + (.7f * starSideLength)) && y >= (_pointOfImpact.y + (.7f * starSideLength)) + ((x - _pointOfImpact.x) - starSideLength))
                        clearConditionMet = true;
                }

                //if the pixel is within the shape, make it transparent
                if (clearConditionMet)
                {
                    color.a = 0;
                    //This line of code and if statement, turn Green pixels into Red pixels.
                    _textureClone.SetPixel(x, y, color);
                }
            }
        }

        //apply the changes to the sprite
        _textureClone.Apply();
    }

    //remove and add a polyCOllider, it will now cut out the newly transparent pixels
    //if theres a chunk floating in the air, then that will be a separate path of the collider; remove it
    //we will use the coordinates of these separate paths to also clear the pixels(not implemented yet)
    public void UpdateCollider()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        
        //To be implemented: cross product can be used to find which path holds the most area, that will be whats left of the boulder
        //It's possible to make those separated chunks different gameObjects and have them fall off

        //if(collider.pathCount > 1)
        //{
   

        //    for (int j = 0; j < collider.pathCount; j++)
        //    {
        //        if(j != indexOfBiggestPath)
        //        {
        //            Vector2[] points = collider.GetPath(j);

        //            Vector2[] zeroPoints = new Vector2[points.Length];

        //            for (int k = 0; k < points.Length; k++)
        //            {
        //                zeroPoints[k] = Vector2.zero;
        //            }

                   
        //             collider.SetPath(j, zeroPoints);
                    
        //        }
                
        //    }
        //}
    }

    //change the shape of the next hole to carve out
    public void UpdateHoleShape()
    {
        if (_currentHole == HoleShapes.Circle)
            _currentHole = HoleShapes.Square;

        else if (_currentHole == HoleShapes.Square)
            _currentHole = HoleShapes.Triangle;

        else if (_currentHole == HoleShapes.Triangle)
            _currentHole = HoleShapes.Clover;

        else if (_currentHole == HoleShapes.Clover)
            _currentHole = HoleShapes.Star;

        else if (_currentHole == HoleShapes.Star)
            _currentHole = HoleShapes.Circle;
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
