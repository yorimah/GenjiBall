using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInput : MonoBehaviour
{
    InputController _inputController;

    public delegate void Callback_Vector2(Vector2 vector);
    public Callback_Vector2 inputMoveCamera;

    private void OnEnable()
    {
        _inputController = new InputController();

        _inputController.Camera.AngleMovement.started += inputVariationMoveCamera;

        _inputController.Enable();
    }

    private void OnDisable()
    {
        _inputController.Camera.AngleMovement.started -= inputVariationMoveCamera;

        _inputController.Disable();
    }

    private void inputVariationMoveCamera(InputAction.CallbackContext obj)
    {
        if (inputMoveCamera == null) { return; }

        Vector2 vector = obj.ReadValue<Vector2>();
        inputMoveCamera(vector);
    }
}
