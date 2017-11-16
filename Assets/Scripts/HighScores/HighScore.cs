using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to encapsulate an individual score
/// </summary>
class HighScore : IComparable<HighScore> {

   public int ID { get; set; }

   public string Name { get; set; }

   public int Score { get; set; }

   public HighScore(int id, string name, int score) {
      this.ID = id;
      this.Name = name;
      this.Score = score;
   }

   /// <summary>
   /// Comparing a score to another based solely on value.
   /// </summary>
   /// <param name="other"> What the score is being compared to </param>
   /// <returns></returns>
   public int CompareTo(HighScore other) {
      if (other.Score < this.Score) {
         return -1;
      } else if (other.Score > this.Score) {
         return 1;
      }
      return 0;
   }
}
