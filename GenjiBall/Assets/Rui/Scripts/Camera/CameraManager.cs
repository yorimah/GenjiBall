using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraMovement),typeof(CameraInput))]
public class CameraManager : MonoBehaviour
{
    Transform myTransform;
    CameraMovement movement;
    CameraInput input;

    private void Start()
    {
        myTransform = transform;
        movement = GetComponent<CameraMovement>();
        input = GetComponent<CameraInput>();
        input.inputMoveCamera = (Vector2 vector) => movement.AngleMove(vector);
    }
}
