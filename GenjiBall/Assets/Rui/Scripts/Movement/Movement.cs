using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector3 velocity;
    public Vector3 Velocity { get => velocity; set => velocity = value; }

    Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }

    private void FixedUpdate()
    {
        applyVelocity();
    }

    void applyVelocity()
    {
        myTransform.Translate(velocity * Time.timeScale, Space.World);
    }

    public void changeVelocity(float x, float y, float z)
    {
        velocity.x = x;
        velocity.y = y;
        velocity.z = z;
    }

    public void changeVelocity(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void changeVelocity_x(float x)
    {
        velocity.x = x;
    }

    public void changeVelocity_y(float y)
    {
        velocity.z = y;
    }

    public void changeVelocity_z(float z)
    {
        velocity.z = z;
    }

    public void stopMoving()
    {
        changeVelocity(Vector3.zero);
    }


    public bool isMoving()
    {
        return velocity != Vector3.zero;
    }
}
