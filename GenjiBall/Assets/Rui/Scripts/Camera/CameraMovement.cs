using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float moveSensitivity;
    [SerializeField] float angleLimit_x_axis;

    Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }

    public void AngleMove(Vector2 inputVector)
    {
        Vector3 eulerAngle = myTransform.eulerAngles;
        eulerAngle.x += -inputVector.y * moveSensitivity;
        eulerAngle.y += inputVector.x * moveSensitivity;

        if (eulerAngle.x > 90) { eulerAngle.x -= 360; }
        eulerAngle.x = Mathf.Clamp(eulerAngle.x, -angleLimit_x_axis, angleLimit_x_axis);

        myTransform.eulerAngles = eulerAngle;
    }
}
