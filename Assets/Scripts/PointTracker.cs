using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Individual High Score markers, have a caveman sprite that will be switched
//to the player drawn image when the player passes the highscore marker
[System.Serializable]
public class HighScoreMarker
{
    public GameObject markerObject;
    //marker sprite before the player passes the score
    public Sprite caveman;
    //image after the user passes the score and runs over the caveman, can probably be switched to a UI image if needed
    public Sprite userImage;
}


public class PointTracker : MonoBehaviour
{
    // Reference to player object, AKA boulder
    public GameObject player;
    //marker for the highscore
    public HighScoreMarker[] highScoreMarkers;
    public AudioSource audioS;
    public AudioClip[] newHighScoreScreams;
    //height at which the ground ends up leveling out and the score marker is placed, the x value is from the actual score
    public float highScoreMarkerYValue;

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
	void Start ()
    {
        boulderPosition = player.GetComponent<Transform>();
        lastPosition = boulderPosition.transform.position.x;

        //used to test the markers
        PlayerPrefs.SetInt("HighScore0", 10);
        PlayerPrefs.SetInt("HighScore1", 20);
        PlayerPrefs.SetInt("HighScore2", 30);
        PlayerPrefs.SetInt("HighScore3", 40);
        PlayerPrefs.SetInt("HighScore4", 50);
        PlayerPrefs.SetInt("HighScore5", 60);
        PlayerPrefs.SetInt("HighScore6", 70);
        PlayerPrefs.SetInt("HighScore7", 80);
        PlayerPrefs.SetInt("HighScore8", 90);
        PlayerPrefs.SetInt("HighScore9", 100);



        //iterates throught the current highscores, activates and places the markers for the high scores that have been set
        for (int i = 0; i < highScoreMarkers.Length; i++)
        {
            if(PlayerPrefs.GetInt(highScoreMarkers[i].markerObject.name, 0) > 0)
            {
                highScoreMarkers[i].markerObject.SetActive(true);
                highScoreMarkers[i].markerObject.transform.position = new Vector3(PlayerPrefs.GetInt(highScoreMarkers[i].markerObject.name, 0), highScoreMarkerYValue);
            }
        }
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        points += boulderPosition.transform.position.x - lastPosition;
        lastPosition = boulderPosition.transform.position.x;

        //used to test the sound clips and sprite change when it passes the markers, 
        //this code can be pasted into the actual high score implementation
        for (int i = 0; i < highScoreMarkers.Length; i++)
        {
            if (PlayerPrefs.GetInt(highScoreMarkers[i].markerObject.name, 0) > 0 && Mathf.Abs(points - PlayerPrefs.GetInt(highScoreMarkers[i].markerObject.name, 0)) <= .2f)
            {               
                audioS.clip = newHighScoreScreams[Random.Range(0, newHighScoreScreams.Length)];
                audioS.Play();

                highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = highScoreMarkers[i].userImage;
            }
        }



    }
}
