using System.Collections;
using System.Collections.Generic;

public class Score
{
    public Score()
    {
        score = 0;
    }

    public void resetScore()
    {
        score = 0;
    }

    public void addScore(int value)
    {
        score += value;
    }

    public void decreaseScore(int value)
    {
        score -= value;
    }

    public void setScpre(int value)
    {
        score = value;
    }

    public int getCurrentScore()
    {
        return score;
    }

    private int score;
}
