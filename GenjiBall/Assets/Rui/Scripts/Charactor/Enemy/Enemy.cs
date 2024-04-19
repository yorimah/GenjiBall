using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    [Header("")]
    [SerializeField] int score;

    public new void Start()
    {
        base.Start();
        dieAction = _dieAction;
    }

    void _dieAction()
    {
        gameObject.SetActive(false);
        GameManager.instance.addScore(score);
    }
}
