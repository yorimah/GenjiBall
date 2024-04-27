using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackInput : MonoBehaviour
{
    InputController _inputController;

    public delegate void Callback_Void();
    public Callback_Void inputAttack;

    private void OnEnable()
    {
        _inputController = new InputController();

        _inputController.Player.Attack_CloseRange.started += OnAttack;

        _inputController.Enable();
    }

    private void OnDisable()
    {
        _inputController.Player.Attack_CloseRange.started -= OnAttack;

        _inputController.Disable();
    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (inputAttack == null) { return; }

        inputAttack();
    }
}
