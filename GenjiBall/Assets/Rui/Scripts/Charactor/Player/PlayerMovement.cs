using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Movement))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform forwardObject;

    Movement movement;

    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    public void OnMove(Vector2 _vector)
    {
        Vector3 vector = _vector;
        vector.z = vector.y;

        Vector3 forward = forwardObject.forward;
        forward.y = 0;
        Vector3 right = forwardObject.right;
        right.y = 0;

        vector = (vector.x * right.normalized + vector.z * forward.normalized);
        vector = vector.normalized * moveSpeed;
        movement.changeVelocity_x(vector.x);
        movement.changeVelocity_z(vector.z);
    }
}
