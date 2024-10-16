using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    public delegate void ScoreChangedDelegate(int newScore);
    public event ScoreChangedDelegate OnScoreChanged;

    private int score = 0;

    public void IncreaseScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    public void DecreaseScore(int amount)
    {
        score -= amount;
        OnScoreChanged?.Invoke(score);
    }

    public int GetScore()
    {
        return score;
    }
}
