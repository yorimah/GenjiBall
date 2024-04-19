using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] Text remaingPlayerLifeText;
    [SerializeField] Text scoreText;
    [SerializeField] Text HiScoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void updatePlayerLife(int value)
    {
        remaingPlayerLifeText.text = value.ToString();
    }

    public void updateScore(int value)
    {
        scoreText.text = value.ToString("D6");
    }

    public void updateHiScore(int value)
    {
        HiScoreText.text = value.ToString("D6");
    }
}
