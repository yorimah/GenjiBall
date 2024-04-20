using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAnimation : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void startAttack()
    {
        animator.SetBool("Attack", true);
    }

    public void endAttack()
    {
        animator.SetBool("Attack", false);
    }
}
