using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAction : MonoBehaviour
{
    InputController _inputController;
    Vector2 moveVector;

    public delegate void Callback_Vectotr2(Vector2 vector);
    public Callback_Vectotr2 inputMove;

    private void OnEnable()
    {
        _inputController = new InputController();

        _inputController.Player.Move.started += OnMove;
        _inputController.Player.Move.performed += OnMove;
        _inputController.Player.Move.canceled += OnMove;
        _inputController.Camera.AngleMovement.started += inputMoveCamera;

        _inputController.Enable();
    }

    private void OnDisable()
    {
        _inputController.Player.Move.started -= OnMove;
        _inputController.Player.Move.performed -= OnMove;
        _inputController.Player.Move.canceled -= OnMove;
        _inputController.Camera.AngleMovement.started -= inputMoveCamera;

        _inputController.Disable();
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        if (inputMove == null) { return; }

        switch (obj.phase)
        {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                moveVector = obj.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                moveVector = Vector2.zero;
                break;
        }

        inputMove(moveVector);
    }

    private void inputMoveCamera(InputAction.CallbackContext obj)
    {
        inputMove(moveVector);
    }
}