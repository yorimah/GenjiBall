using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    Player.PlayerManager player;

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

    private void Start()
    {
        player = GameManager.instance.player;
    }

    public void AttackToPlayer()
    {
        if (player.isAvoiding == true || player.isGuarding == true) { return; }

        if(player.TryGetComponent(out IDamageable damageable) == false) { return; }

        damageable.damage(10);
    }
}
