using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    [SerializeField] int MaxHP;

    Status status;

    public delegate void DamageAction();
    public DamageAction damageAction;
    public delegate void DieAction();
    public DieAction dieAction;

    public void Start()
    {
        status = new Status(MaxHP);
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
}
