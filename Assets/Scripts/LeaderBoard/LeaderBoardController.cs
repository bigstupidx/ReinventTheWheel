using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Individual High Score markers, have a caveman sprite that will be switched
//to the player drawn image when the player passes the highscore marker
[System.Serializable]
public class HighScoreMarker
{
    public GameObject markerObject;
    ////marker sprite before the player passes the score
    //public Sprite[] caveman;
    ////image after the user passes the score and runs over the caveman, can probably be switched to a UI image if needed
    //public Sprite userImage;
}

public class LeaderBoardController : MonoBehaviour {
    public GameObject boulder;
    public TextAsset HighScoresTextFile;
    public InputField NameEntry;
    public Text PlayerScoreDisplayText;
    public Text CharactersMaxText;
    public Button NameEntryButton;
    public PointTracker pointTracker;
    public Text[] HighScoreRanks;
    public Text[] HighScoreNames;
    public Text[] HighScoreScores;
    public HighScoreMarker[] highScoreMarkers;
    public float highScoreMarkerYValue;
    public Sprite[] cavemenBystanders;
    public AudioSource audioSource;
    public AudioClip[] newHighScoreScreams;
    private string enteredName;
    private float points;
    private string[] highscoreEntries;
    private List<HighScore> highScoresList;
    private string path = "Assets/Resources/HighScores.txt";
    private StreamReader reader;
    private StreamWriter writer;
    // Use this for initialization
    void Start () 
    {
        int i = 0;
        reader = new StreamReader(path);
        highscoreEntries = reader.ReadToEnd().Split('\n');
        reader.Close();
        highScoresList = new List<HighScore>();
        //highscoreEntries = HighScoresTextFile.text.Split('\n');
        foreach (string str in highscoreEntries)
        {
            string[] entry = str.Split('|');
            if (entry.Length == 1)
                break;
            HighScoreNames[i].text = entry[0];
            HighScoreScores[i].text = entry[1];
            HighScore hs = new HighScore(entry[0], int.Parse(entry[1]));
            highScoresList.Add(hs);
            highScoresList.Sort();

            highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = cavemenBystanders[UnityEngine.Random.Range(0, cavemenBystanders.Length)];
            highScoreMarkers[i].markerObject.SetActive(true);
            highScoreMarkers[i].markerObject.transform.position = new Vector3(highScoresList[i].Score, highScoreMarkerYValue);
            i++;
        }
    }

    public void LateUpdate()
    {
        //used to test the sound clips and sprite change when it passes the markers,
        //this code can be pasted into the actual high score implementation
        for (int i = 0; i < highScoresList.Count; i++)
        {
            if (highScoresList[i].Score > 0 && Mathf.Abs(boulder.transform.position.x - highScoresList[i].Score) <= 1f)
            {
                audioSource.clip = newHighScoreScreams[UnityEngine.Random.Range(0, newHighScoreScreams.Length)];
                audioSource.Play();

                //highScoreNames[i].gameObject.transform.position = new Vector2(highScoresList[i].Score, highScoreMarkerYValue);
                // highScoreNames[i].gameObject.SetActive(true);
                Destroy(highScoreMarkers[i].markerObject);
                // highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = highScoreMarkers[i].userImage;
            }

        }
    }
    public void Activate()
    {
        points = (int)pointTracker.lastPosition;
        PlayerScoreDisplayText.text = "Your Score: " + points;
        if (points < highScoresList[highScoresList.Count - 1].Score)
        {
            NameEntry.interactable = false;
            NameEntry.text = "Try Again";
            NameEntryButton.gameObject.SetActive(false);
            CharactersMaxText.gameObject.SetActive(false);
        }
    }
    public void EntryName()
    {
        enteredName = NameEntry.text;
        EnterIntoHighScore();
        RefreshBoard();
        SaveToTextFile();
    }
    private void EnterIntoHighScore()
    {

        //Read the text from directly from the test.txt file
        /*reader = new StreamReader(path);
        highscoreEntries = reader.ReadToEnd().Split('\n');
        reader.Close();
        highScoresList = new List<HighScore>();
        foreach(string str in highscoreEntries)
        {
            string[] tokens = str.Split('|');
            if (tokens.Length == 1)
                break;
            HighScore hs = new HighScore(tokens[0], int.Parse(tokens[1]));
            highScoresList.Add(hs);
        }*/
        highScoresList.Add(new HighScore(enteredName, (int)points));
        highScoresList.Sort();
        highScoresList.RemoveAt(highScoresList.Count-1);
    }
    private void RefreshBoard()
    {
        int i = 0;
        foreach(HighScore hs in highScoresList)
        {
            HighScoreNames[i].text = hs.Name;
            HighScoreScores[i].text = hs.Score.ToString();
            i++;
        }
    }
    private void SaveToTextFile()
    {
        
        writer = new StreamWriter(path, false);
        
        string fileString = null;
        foreach(HighScore hs in highScoresList)
        {
            fileString += hs.Name + "|" + hs.Score + "\n";
        }
        if(points > highScoresList[highScoresList.Count-1].Score)
        {
            writer.WriteLine(fileString);
            writer.Close();
        }
    }
}
