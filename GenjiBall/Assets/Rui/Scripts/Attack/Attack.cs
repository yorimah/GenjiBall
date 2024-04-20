using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

[RequireComponent(typeof(AttackInput))]
public class Attack : MonoBehaviour
{
    AttackInput attackInput;

    public delegate void InputAttackAction();
    InputAttackAction inputAttackAction;

    private void Start()
    {
        attackInput = GetComponent<AttackInput>();
        attackInput.inputAttack = () => inputAttackAction();
    }

    public void setInputAttackAction(InputAttackAction action)
    {
        inputAttackAction = action;
    }
}
