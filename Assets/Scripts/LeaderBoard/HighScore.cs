using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HighScore : IComparable<HighScore> {

    //public int ID { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }

    public HighScore(string name, int score) {
        //this.ID = id;
        this.Name = name;
        this.Score = score;
    }

    public int CompareTo(HighScore other) {
        if (other.Score < this.Score) {
            return -1;
        } else if (other.Score > this.Score) {
            return 1;
        }
        return 0;
    }
}
