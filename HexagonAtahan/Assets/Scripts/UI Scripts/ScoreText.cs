using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    [SerializeField]private Text scoreText;
    private int score = 0;
    private int mileStoneScore = 0;

    public VoidEvent ScoreMilestoneEvent;

    public void addToScore() {
        if (HexMap.isInitialized) {
            score += 5;
            mileStoneScore += 5;
            scoreText.text = "Score: " + score;


            if (mileStoneScore>1000) {
                mileStoneScore -= 1000;
                ScoreMilestoneEvent.Raise(new Void());

            }


        }
    }
        

}
