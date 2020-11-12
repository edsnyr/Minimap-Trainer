using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{

    public TextMeshProUGUI topScoreBox;
    public TextMeshProUGUI botScoreBox;
    public TextMeshProUGUI bestScoreBox;
    public TextMeshProUGUI averageScoreBox;

    public Color defaultTextColor;
    public Color hightlightTextColor;

    float topScore = 0;
    float botScore = 0;
    float bestScore = 0;
    float averageScore = 0;
    float totalScore = 0;
    float scores = 0;

    void Start()
    {
        UpdateScores();
    }

    /// <summary>
    /// Sets the time for the specified lane and highlights it on the scoreboard.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="position"></param>
    public void Score(float time, Position position) {
        if(position == Position.Top) {
            topScore = time;
            topScoreBox.color = hightlightTextColor;
            botScoreBox.color = defaultTextColor;
        } else if(position == Position.Bot) {
            botScore = time;
            botScoreBox.color = hightlightTextColor;
            topScoreBox.color = defaultTextColor;
        }
        UpdateAverage(time);
        UpdateBest(time);
        UpdateScores();
    }

    /// <summary>
    /// Finds the new average time for the player
    /// </summary>
    /// <param name="time"></param>
    private void UpdateAverage(float time) {
        totalScore += time;
        averageScore = ((int)(totalScore / ++scores * 1000))/1000f;
    }

    /// <summary>
    /// Updates the player's best response time. If the new time is the best, highlights it.
    /// </summary>
    /// <param name="time"></param>
    private void UpdateBest(float time) {
        if(bestScore == 0 || time < bestScore) {
            bestScore = time;
            bestScoreBox.color = hightlightTextColor;
        } else {
            bestScoreBox.color = defaultTextColor;
        }
    }

    /// <summary>
    /// Displays all scores to the scoreboard.
    /// </summary>
    private void UpdateScores() {
        topScoreBox.text = "Top: " + topScore + "s";
        botScoreBox.text = "Bot: " + botScore + "s";
        bestScoreBox.text = "Best: " + bestScore + "s";
        averageScoreBox.text = "Average: " + averageScore + "s";
    }
}
