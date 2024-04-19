using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UseObjectPool;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int FPS;
    [SerializeField] float gameSpeed;
    [SerializeField] int playerLife;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public Player player;

    int remainPlayerLife;
    Score score;
    Score HiScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            score = new Score();
            HiScore = new Score();
        }
        else
        {
            instance.setPlayer();
            Destroy(gameObject);
        }

        ObjectPool.Init();
    }


    private void OnEnable()
    {
        setPlayer();
    }

    private void Start()
    {
        Application.targetFrameRate = FPS;
        changeGameSpeed(gameSpeed);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        remainPlayerLife = playerLife;
        UIManager.instance.updatePlayerLife(remainPlayerLife);
    }

    public void changeGameSpeed(float speed)
    {
        Time.timeScale = speed;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void setPlayer()
    {
        player = FindObjectOfType<Player>();
        if (player == null) { return; }

        player.damageAction += decreasePlayerLife;
    }

    void decreasePlayerLife()
    {
        remainPlayerLife--;
        if (remainPlayerLife <= 0)
        {
            remainPlayerLife = 0;
            player.gameObject.SetActive(false);
        }
        UIManager.instance.updatePlayerLife(remainPlayerLife);
    }

    public void addScore(int value)
    {
        score.addScore(value);
        int currentScore= score.getCurrentScore();
        UIManager.instance.updateScore(currentScore);
        if (HiScore.getCurrentScore() < currentScore)
        {
            HiScore.setScpre(currentScore);
            UIManager.instance.updateHiScore(currentScore);
        }
    }

    void resetScore()
    {
        score.resetScore();
    }
}
