using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAnimation : MonoBehaviour
{
    Animator animator;
    [SerializeField] AnimationState attackAnimationState;

    public delegate void EndAttackAction();
    public EndAttackAction endAttackAction;

    private void Awake()
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
        if (endAttackAction == null) { return; }

        endAttackAction();
    }

    public void StartAnimation(string str)
    {
        animator.SetBool(str, true);
    }

    public void EndAnimation(string str)
    {
        animator.SetBool(str, false);
    }

    public void CauseDamage()
    {
        CombatManager.instance.AttackToPlayer();
    }

    public string getAnimationStateMotionName(string stateName)
    {
        // AnimatorControllerを取得
        var controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

        // "Attack"ステートのMotionを取得
        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.name == stateName)
                {
                    var motion = state.state.motion;
                    return motion.name;
                }
            }
        }

        return "";
    }

    public void overrideAnimationClip(AnimationClip afterClip, string DefaultAttackName)
    {
        var AOC = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = AOC;
        AOC[DefaultAttackName] = afterClip;
    }

    public void playAnimation(string stateName)
    {
        animator.Play(stateName, 0, 0);
    }
}
