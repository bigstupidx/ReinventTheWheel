using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public InputField NameEntry;
    public Text PlayerScoreDisplayText;
    public Button NameEntryButton;
    public PointTracker pointTracker;
    public Text[] HighScoreRanks;
    public Text[] HighScoreNames;
    public Text[] HighScoreScores;
    public List<GameObject> markers;
    //public List<HighScoreMarker> highScoreMarkers;
    public float highScoreMarkerYValue;
    public GameObject[] cavemenBystanders;
    public AudioSource screamsAudioSource;
    public AudioClip[] newHighScoreScreams;
    public UnityAdvertisementsController adsController;
    public AppodealAdvertisementsController appodealController;
    private string enteredName;
    private float points;
    //private string[] highscoreEntries;
    private List<HighScore> highScoresList;
    private string path = "Assets/HighScores.txt";
    //private StreamReader reader;
    //private StreamWriter writer;
    private Rigidbody2D _rb2d;
    private int _index;
    // Use this for initialization
    void Start () 
    {
        //PlayerPrefs.DeleteKey("LeaderBoardFirstTimeSetUpCompleted");
        _rb2d = boulder.GetComponent<Rigidbody2D>();
        if (PlayerPrefs.HasKey("LeaderBoardFirstTimeSetUpCompleted"))
        {
            
            int i = 0;
            highScoresList = new List<HighScore>();
            for (int j = 1; j <=10;j++)
            {
                string name = PlayerPrefs.GetString("Rank" + j + "Name");
                int points = PlayerPrefs.GetInt("Rank" + j + "Points");                
                HighScore hs = new HighScore(name, points);
                highScoresList.Add(hs);
                
            }
            highScoresList.Sort();
            foreach (HighScore hs in highScoresList)
            {
                HighScoreNames[i].text = hs.Name;
                HighScoreScores[i].text = hs.Score.ToString();
                _index = UnityEngine.Random.Range(0, cavemenBystanders.Length);
                markers.Add(Instantiate(cavemenBystanders[_index], new Vector3(highScoresList[i].Score, highScoreMarkerYValue, 1), Quaternion.identity));
                //highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = cavemenBystanders[UnityEngine.Random.Range(0, cavemenBystanders.Length)];
                //highScoreMarkers[i].markerObject.SetActive(true);
                //highScoreMarkers[i].markerObject.transform.position = new Vector3(highScoresList[i].Score, highScoreMarkerYValue);
                i++;
            }
            //highscoreEntries = reader.ReadToEnd().Split('\n');
            //reader.Close();
            //highScoresList = new List<HighScore>();
            //highscoreEntries = HighScoresTextFile.text.Split('\n');
            /*foreach (string str in highscoreEntries)
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
            }*/
        }
        else
        {
            PlayerPrefs.SetString("LeaderBoardFirstTimeSetUpCompleted", "True");
            PlayerPrefs.SetString("Rank1Name", "Stanley");
            PlayerPrefs.SetString("Rank2Name", "Ken");
            PlayerPrefs.SetString("Rank3Name", "Juan");
            PlayerPrefs.SetString("Rank4Name", "Devin");
            PlayerPrefs.SetString("Rank5Name", "Titus");
            PlayerPrefs.SetString("Rank6Name", "Michael");
            PlayerPrefs.SetString("Rank7Name", "Jether");
            PlayerPrefs.SetString("Rank8Name", "James");
            PlayerPrefs.SetString("Rank9Name", "Neil");
            PlayerPrefs.SetString("Rank10Name", "Peter");
            PlayerPrefs.SetInt("Rank1Points", 199);
            PlayerPrefs.SetInt("Rank2Points", 183);
            PlayerPrefs.SetInt("Rank3Points", 168);
            PlayerPrefs.SetInt("Rank4Points", 149);
            PlayerPrefs.SetInt("Rank5Points", 146);
            PlayerPrefs.SetInt("Rank6Points", 132);
            PlayerPrefs.SetInt("Rank7Points", 121);
            PlayerPrefs.SetInt("Rank8Points", 110);
            PlayerPrefs.SetInt("Rank9Points", 102);
            PlayerPrefs.SetInt("Rank10Points", 89);
            int i = 0;
            highScoresList = new List<HighScore>();
            for (int j = 1; j <= 10; j++)
            {
                string name = PlayerPrefs.GetString("Rank" + j + "Name");
                int points = PlayerPrefs.GetInt("Rank" + j + "Points");
                
                HighScore hs = new HighScore(name, points);
                highScoresList.Add(hs);

            }

            highScoresList.Sort();
            foreach (HighScore hs in highScoresList)
            {
                HighScoreNames[i].text = hs.Name;
                HighScoreScores[i].text = hs.Score.ToString();
                markers.Add(Instantiate(cavemenBystanders[_index], new Vector3(highScoresList[i].Score, highScoreMarkerYValue), Quaternion.identity));
                //highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = cavemenBystanders[UnityEngine.Random.Range(0, cavemenBystanders.Length)];
                //highScoreMarkers[i].markerObject.SetActive(true);
                //highScoreMarkers[i].markerObject.transform.position = new Vector3(highScoresList[i].Score, highScoreMarkerYValue);
                i++;
            }
        }
       // StartCoroutine(CavemanGruntsRanOver());
    }

    /*public void LateUpdate()
    {
        //used to test the sound clips and sprite change when it passes the markers,
        //this code can be pasted into the actual high score implementation
        if(_rb2d.velocity.x > 0)
        {
            for (int i = 0; i < highScoresList.Count; i++)
            {
                if (highScoresList[i].Score > 0 && Mathf.Abs(boulder.transform.position.x - highScoresList[i].Score) <= 1f)
                {
                    screamsAudioSource.Stop();
                    screamsAudioSource.clip = newHighScoreScreams[UnityEngine.Random.Range(0, newHighScoreScreams.Length)];
                    screamsAudioSource.Play();

                    //highScoreNames[i].gameObject.transform.position = new Vector2(highScoresList[i].Score, highScoreMarkerYValue);
                    // highScoreNames[i].gameObject.SetActive(true);
                    Destroy(markers[i].gameObject);
                    // highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = highScoreMarkers[i].userImage;
                }

            }
        }
        
    }*/
    public void Activate()
    {
        points = (int)pointTracker.lastPosition;
        PlayerScoreDisplayText.text = "Your Score: " + points;
        if (points < highScoresList[highScoresList.Count - 1].Score)
        {
            NameEntry.interactable = false;
            NameEntry.text = "Try Again";
            NameEntryButton.gameObject.SetActive(false);
        }

#if UNITY_ANDROID
        // Debug.Log("Showing Advertisement");
        //adsController.ShowAdvertisement();
        appodealController.HideAppodealBanner();
#endif
    }
    public void EntryName()
    {
        enteredName = NameEntry.text;
        NameEntry.interactable = false;
        NameEntryButton.gameObject.SetActive(false);
        screamsAudioSource.enabled = false;
        EnterIntoHighScore();
        RefreshBoard();
        SaveLeaderBoard();
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
    private void SaveLeaderBoard()
    {
        int i = 1;
        foreach(HighScore hs in highScoresList)
        {
            PlayerPrefs.SetString("Rank" + i + "Name", hs.Name);
            PlayerPrefs.SetInt("Rank" + i + "Points", hs.Score);
            i++;
        }
        /*for(int j = 1; j <= 10;j++)
        {
            Debug.Log(PlayerPrefs.GetString("Rank" + j + "Name")+"/"+PlayerPrefs.GetInt("Rank"+j+"Points"));
        }*/
    }

    public void RetryButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    /*IEnumerator CavemanGruntsRanOver()
    {
        while (_rb2d.velocity.x == 0)
        {
            yield return null;
        }
           
        while (_rb2d.velocity.x > 0)
        {
            for (int i = 0; i < highScoresList.Count; i++)
            {
                if (highScoresList[i].Score > 0 && Mathf.Abs(boulder.transform.position.x - highScoresList[i].Score) <= 1f)
                {
                    screamsAudioSource.Stop();
                    screamsAudioSource.clip = newHighScoreScreams[UnityEngine.Random.Range(0, newHighScoreScreams.Length)];
                    screamsAudioSource.Play();

                    //highScoreNames[i].gameObject.transform.position = new Vector2(highScoresList[i].Score, highScoreMarkerYValue);
                    // highScoreNames[i].gameObject.SetActive(true);
                    Destroy(markers[i].gameObject);
                    // highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = highScoreMarkers[i].userImage;
                }
            }
            yield return null;
        }
    }*/
}
