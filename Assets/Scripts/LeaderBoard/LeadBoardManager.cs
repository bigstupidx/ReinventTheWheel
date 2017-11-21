using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeadBoardManager : MonoBehaviour {

    private string connectionString;
    private List<HighScore> scoreList = new List<HighScore>();
    public Transform scoreParent;
    public GameObject scorePrefab;
    public int topRanks;
    public int saveScores = 10;
    public Text enterName;
    public GameObject nameDialog;
    
    void Start() {
        connectionString = "URI=file:" + Application.dataPath + "/LeaderBoardDB.sqlite";
        CreateTable();
        DeleteExtraScores();
        ShowScores();
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
    /// When a name is entered, inserts score into the DB with that name.
    /// </summary>
    public void EnterName() {
        if (enterName.text != string.Empty) {
            int score = PlayerPrefs.GetInt("HighScore9", 0); // The score passed in from PlayerPrefs
            InsertScore(enterName.text, score);
            enterName.text = string.Empty;
            ShowScores();
            nameDialog.SetActive(false);
        }
    }
}
