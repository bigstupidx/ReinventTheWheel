using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to the HighScoreManager to initialize
/// </summary>
public class HighScoreScript : MonoBehaviour {

   public GameObject score;
   public GameObject scoreName;
   public GameObject rank;
   
   public void SetScore(string rank, string name, string score) {
      this.rank.GetComponent<Text>().text = rank;
      this.scoreName.GetComponent<Text>().text = name;
      this.score.GetComponent<Text>().text = score;
   }	
}
