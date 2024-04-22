using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour, IDamageable
{

    Status status;
    [SerializeField] int MaxHP;
    [SerializeField] Transform model;
    public CharactorAnimation charactorAnimation { get; private set; }

    Attack attack;

    public delegate void DamageAction();
    public DamageAction damageAction;
    public delegate void DieAction();
    public DieAction dieAction;

    public void Start()
    {
        status = new Status(MaxHP);
        charactorAnimation = model.GetComponent<CharactorAnimation>();
        attack = model.GetComponent<Attack>();
        if (attack != null) { attack.SetInputAttackAction(startAttack); }
    }

    void IDamageable.damage(int value)
    {
        damage(value);
    }

    public void damage(int damageValue)
    {
        status.getHP().damage(damageValue);

        if (isDying() == false)
        {
            if (damageAction != null) { damageAction(); }
        }
        else
        {
            if (dieAction != null) { dieAction(); }
        }
    }

    public bool isDying()
    {
        return status.getHP().getCurrentHP() <= 0;
    }

    void startAttack()
    {
        charactorAnimation.startAttack();
    }
}
