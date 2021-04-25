using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI scoreText;
    private int score = 0;
    private int mileStoneScore = 0;
    public int milestoneCap = 1000;

    public VoidEvent ScoreMilestoneEvent;

    public void addToScore() {
        if (HexMap.isInitialized) {
            score += 5;
            mileStoneScore += 5;

            if (scoreText != null) {
                scoreText.text = "Score: " + score;
            }
            
            if (mileStoneScore> milestoneCap) {
                mileStoneScore -= milestoneCap;
                ScoreMilestoneEvent.Raise(new Void());

            }


        }
    }
        

}
