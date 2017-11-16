using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;

public class HighScoreManager : MonoBehaviour {

    private String connectionString;

    private List<HighScore> scoreList = new List<HighScore>();

    public GameObject scorePrefab;

    public Transform scoreParent;

    public int topRanks;

    public int saveScores;

    public InputField enterName;

    public GameObject nameDialog;

    // Use this for initialization
    void Start() {
        connectionString = "URI=file:" + Application.dataPath + "/LeaderBoardDB.sqlite";
        CreateTable();
        ShowScores();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            nameDialog.SetActive(!nameDialog.activeSelf);
        }
    }

    public void EnterName() {
        if (enterName.text != string.Empty) {
            // Score that is passed in
            int score = UnityEngine.Random.Range(200, 500);

            // The name that the player enters
            InsertScore(enterName.text, score);
            enterName.text = string.Empty;

            ShowScores();

            nameDialog.SetActive(false);
        }
    }

    private void CreateTable() {
        using (System.Data.IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            // Open database connection   
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = String.Format("CREATE TABLE IF NOT EXISTS LEADERBOARD (PlayerID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE , Name TEXT NOT NULL, Score INTEGER NOT NULL)");

                // Give the command to the DB to execute
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    // Display all names and scores
    private void GetScores() {

        scoreList.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            // Open database connection   
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = "SELECT * FROM LEADERBOARD";

                // Give the command to the DB to execute
                dbCmd.CommandText = sqlQuery;

                // Reader to read data
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        scoreList.Add(new HighScore(reader.GetInt32(0), reader.GetString(1),
                           reader.GetInt32(2)));
                        Debug.Log(reader.GetString(1) + " | " + reader.GetInt32(2));
                    }

                    // Close connection and reader
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        scoreList.Sort();
    }

    // Insert score into database
    private void InsertScore(string name, int score) {
        GetScores();
        int hsCount = scoreList.Count;
        if (scoreList.Count > 0) {
            HighScore lowestScore = scoreList[scoreList.Count - 1];
            if (lowestScore != null && scoreList.Count >= saveScores && score > lowestScore.Score) {
                DeleteScore(lowestScore.ID);
                hsCount--;
            }
        }

        if (hsCount < saveScores) {
            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
                // Open database connection   
                dbConnection.Open();

                using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                    string sqlQuery = String.Format("INSERT INTO LEADERBOARD(Name, Score) VALUES(\"{0}\", \"{1}\")", name, score);

                    // Give the command to the DB to execute
                    dbCmd.CommandText = sqlQuery;
                    dbCmd.ExecuteScalar();
                    dbConnection.Close();
                }
            }
        }
    }

    // Delete function
    private void DeleteScore(int id) {
        //DELETE FROM HIGHSCORES WHERE PLAYERID = "7";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            // Open database connection   
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = String.Format("DELETE FROM LEADERBOARD WHERE PLAYERID = \"{0}\"", id);

                // Give the command to the DB to execute
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    private void ShowScores() {
        GetScores();

        foreach (GameObject score in GameObject.FindGameObjectsWithTag("Score")) {
            Destroy(score);
        }

        for (int i = 0; i < topRanks; i++) {

            if (i <= scoreList.Count - 1) {
                GameObject tempObj = (GameObject)Instantiate(scorePrefab);
                HighScore tempScore = scoreList[i];

                tempObj.GetComponent<HighScoreScript>().SetScore(("#" + (i + 1)).ToString(),
                   tempScore.Name.ToString(), tempScore.Score.ToString());

                tempObj.transform.SetParent(scoreParent);
                tempObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void DeleteExtraScores() {
        GetScores();
        if (saveScores <= scoreList.Count) {
            int deleteCount = scoreList.Count - saveScores;
            scoreList.Reverse();

            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
                // Open database connection   
                dbConnection.Open();

                using (IDbCommand dbCmd = dbConnection.CreateCommand()) {

                    for (int i = 0; i < deleteCount; i++) {
                        string sqlQuery = String.Format("DELETE FROM LEADERBOARD WHERE PLAYERID = \"{0}\"", scoreList[i]);

                        // Give the command to the DB to execute
                        dbCmd.CommandText = sqlQuery;
                        dbCmd.ExecuteScalar();
                    }
                    dbConnection.Close();
                }
            }
        }
    }

    public void LoadMainMenu() {
        //SceneManager.LoadScene("");
    }
}
