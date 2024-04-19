using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Status
{
    public Status(int hpValue)
    {
        hp = new HP(hpValue);
    }

    public HP getHP()
    {
        return hp;
    }

    private HP hp;
}

[Serializable]
public class HP
{
    public HP(int _maxHP)
    {
        maxHP = _maxHP;
        currentHP = _maxHP;
    }

    public void damage(int value)
    {
        if (value <= 0) { return; }

        currentHP -= value;
        currentHP = Mathf.Max(currentHP,0);
    }

    public void recovery(int value)
    {
        if (value <= 0) { return; }

        currentHP += value;
        currentHP = Mathf.Min(currentHP, maxHP);
    }

    public int getMaxHP()
    {
        return maxHP;
    }

    public int getCurrentHP()
    {
        return currentHP;
    }

    private int maxHP;
    private int currentHP;
}
