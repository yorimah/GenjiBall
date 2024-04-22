using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UseObjectPool;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int FPS = 60;
    [SerializeField] float gameSpeed = 1;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public bool isPlayingGame { get; private set; } = true;
    public Player.PlayerManager player { get; private set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
    }

    public void changeGameSpeed(float speed)
    {
        Time.timeScale = speed;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void setPlayer()
    {
        player = FindObjectOfType<Player.PlayerManager>();
        if (player == null) { return; }
    }

    public void gameClear()
    {
        Debug.Log("ゲームクリア");
        isPlayingGame = false;
    }

    public void gameOver()
    {
        Debug.Log("ゲームオーバー");
        isPlayingGame = false;
    }
}
