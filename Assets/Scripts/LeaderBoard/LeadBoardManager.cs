using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Individual High Score markers, have a caveman sprite that will be switched
//to the player drawn image when the player passes the highscore marker
/*[System.Serializable]
public class HighScoreMarker
{ 
    public GameObject markerObject;
    ////marker sprite before the player passes the score
    //public Sprite[] caveman;
    ////image after the user passes the score and runs over the caveman, can probably be switched to a UI image if needed
    //public Sprite userImage;
}*/

public class LeadBoardManager : MonoBehaviour {

    private string connectionString;
    private List<HighScore> scoreList = new List<HighScore>();
    public Transform scoreParent;
    public GameObject scorePrefab;
    public int topRanks;
    public int saveScores = 10;
    public Text enterName;
    public GameObject nameDialog;
    //markers for the highscores
    public HighScoreMarker[] highScoreMarkers;
    //height at which the ground ends up leveling out and the score marker is placed, the x value is from the actual score
    public float highScoreMarkerYValue;
    public AudioSource audioS;
    //sound clips to play as the boulder passes high scores
    public AudioClip[] newHighScoreScreams;
    public GameObject boulder;
    public Sprite[] cavemenBystanders;
    public Text[] highScoreNames;



    void Start() {
        connectionString = "URI=file:" + Application.dataPath + "/LeaderBoardDB.sqlite";
        CreateTable();
        InsertScore("Stan", 67);
        InsertScore("Juan", 56);
        InsertScore("Devin", 69);
        InsertScore("Titus", 43);
        InsertScore("Ken", 89);
        InsertScore("Michelle", 98);
        InsertScore("Ruben", 99);
        InsertScore("James", 57);
        InsertScore("Jet", 32);
        InsertScore("Kelvin", 69);
        DeleteExtraScores();
        ShowScores();

        //iterates throught the current highscores, activates and places the markers for the high scores that have been set
        if (scoreList.Count > 0)
        {
            for (int i = 0; i < scoreList.Count; i++)
            {
                if (scoreList[i].Score > 0)
                {
                    highScoreNames[i].text = scoreList[i].Name;
                    highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = cavemenBystanders[Random.Range(0, cavemenBystanders.Length)];
                    highScoreMarkers[i].markerObject.SetActive(true);
                    highScoreMarkers[i].markerObject.transform.position = new Vector3(scoreList[i].Score, highScoreMarkerYValue);
                }
            }
        }
    }

    /// <summary>
    /// Function for inserting a score into the LeaderBoard database
    /// </summary>
    /// <param name="name"> Your name </param>
    /// <param name="score"> Your Score </param>
    private void InsertScore(string name, int score) {
        GetScores();
        int hsCount = scoreList.Count;
        if (scoreList.Count > 0) {
            HighScore lowestScore = scoreList[scoreList.Count - 1];
            if (lowestScore != null && scoreList.Count >= saveScores && score > lowestScore.Score) {
                DeleteScore(lowestScore.Name);
                hsCount--;
            }
        }

        if (hsCount < saveScores) {
            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
                dbConnection.Open();

                using (IDbCommand dbCommand = dbConnection.CreateCommand()) {
                    dbCommand.CommandText = string.Format("INSERT INTO LeaderBoard(Name, Score) VALUES(\"{0}\", \"{1}\")", name, score);
                    dbCommand.ExecuteScalar();
                    dbConnection.Close();
                }
            }
        }
    }

    /// <summary>
    /// Grab all scores from LeaderBoard database
    /// </summary>
    private void GetScores() {

        scoreList.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand()) {
                dbCommand.CommandText = "SELECT * FROM LeaderBoard";

                using (IDataReader reader = dbCommand.ExecuteReader()) {
                    while (reader.Read()) {
                        scoreList.Add(new HighScore(reader.GetString(0), reader.GetInt32(1)));
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        scoreList.Sort();
    }

    /// <summary>
    /// Deletes a score given the rank/id
    /// </summary>
    /// <param name="id"></param>
    private void DeleteScore(string name) {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand()) {
                //dbCommand.CommandText = string.Format("DELETE FROM LeaderBoard Where Rank = \"{0}\"", id);
                dbCommand.CommandText = string.Format("DELETE FROM LeaderBoard Where Name = \"{0}\"", name);
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    /// <summary>
    /// Display the Scores on the UI
    /// </summary>
    private void ShowScores() {
        GetScores();

        foreach (GameObject score in GameObject.FindGameObjectsWithTag("Score")) {
            Destroy(score);
        }

        for (int i = 0; i < topRanks; i++) {
            if (i <= scoreList.Count - 1) {
                GameObject temp = Instantiate(scorePrefab);
                HighScore tempScore = scoreList[i];
                temp.GetComponent<HighScoreScript>().SetScore(("#" + (i + 1)).ToString(), tempScore.Name, tempScore.Score.ToString());
                temp.transform.SetParent(scoreParent);
            }
        }
    }

    /// <summary>
    /// Grabs only the top # of scores, and deletes the rest
    /// </summary>
    private void DeleteExtraScores() {
        GetScores();
        if (saveScores <= scoreList.Count) {
            int deleteCount = scoreList.Count - saveScores;
            scoreList.Reverse();

            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
                dbConnection.Open();

                using (IDbCommand dbCommand = dbConnection.CreateCommand()) {

                    for (int i = 0; i < deleteCount; i++) {
                        //dbCommand.CommandText = string.Format("DELETE FROM LeaderBoard WHERE RANK = \"{0}\"", scoreList[i]);
                        dbCommand.ExecuteScalar();
                    }
                    dbConnection.Close();
                }
            }
        }
    }

    /// <summary>
    /// Creates the table for LeadBoard in DB anyone that doesn't have it
    /// </summary>
    private void CreateTable() {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand()) {
                //dbCommand.CommandText = string.Format("CREATE TABLE if not exists HighScores (Rank INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE ,Name TEXT NOT NULL , Score INTEGER NOT NULL)");
                dbCommand.CommandText = string.Format("CREATE TABLE if not exists LeaderBoard (Name TEXT PRIMARY KEY NOT NULL , Score INTEGER NOT NULL)");
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    /// <summary>
    /// Wipes database
    /// </summary>
    private void ClearDatabase() {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand()) {
                //dbCommand.CommandText = string.Format("DELETE FROM LeaderBoard Where Rank = \"{0}\"", id);
                dbCommand.CommandText = "DELETE * LeaderBoard";
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    /// <summary>
    /// When a name is entered, inserts score into the DB with that name.
    /// </summary>
    public void EnterName() {
        if (enterName.text != string.Empty) {
            int score = PlayerPrefs.GetInt("HighScore9", 0); // The score passed in from PlayerPrefs
            InsertScore(enterName.text, score);
            enterName.text = string.Empty;
            ShowScores();
            nameDialog.SetActive(false);

            SceneManager.LoadScene("MVPScene");
        }
    }

    public void LateUpdate()
    {
        //used to test the sound clips and sprite change when it passes the markers,
        //this code can be pasted into the actual high score implementation
        for (int i = 0; i < scoreList.Count; i++)
        {
            if (scoreList[i].Score > 0 && Mathf.Abs(boulder.transform.position.x - scoreList[i].Score) <= 1f)
            {
                audioS.clip = newHighScoreScreams[Random.Range(0, newHighScoreScreams.Length)];
                audioS.Play();

                highScoreNames[i].gameObject.transform.position = new Vector2(scoreList[i].Score, highScoreMarkerYValue);
               // highScoreNames[i].gameObject.SetActive(true);
                Destroy(highScoreMarkers[i].markerObject);
                // highScoreMarkers[i].markerObject.GetComponent<SpriteRenderer>().sprite = highScoreMarkers[i].userImage;
            }

        }
    }
    public void RetryButton()
    {
        SceneManager.LoadScene("MVPScene");
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
