using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddMath;

public class Movement : MonoBehaviour
{
    public float maximumAngleOfWallRecognition;
    public bool isHittingWall;
    public bool isHittingGround;
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public Vector3 OneFrameAgoPosition { get => oneFrameAgoPosition; set => oneFrameAgoPosition = value; }
    public Vector3 OneFrameAgoVelocity { get => oneFrameAgoVelocity; set => oneFrameAgoVelocity = value; }

    Vector3 velocity;
    Vector3 oneFrameAgoPosition;
    Vector3 oneFrameAgoVelocity;
    List<GameObject> hitWallObjects;
    List<GameObject> hitGroundObjects;
    Transform myTransform;
    Rigidbody rb;

    private void Start()
    {
        hitWallObjects = new List<GameObject>();
        hitGroundObjects = new List<GameObject>();
        myTransform = transform;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        applyVelocity();
    }

    private void LateUpdate()
    {
        OneFrameAgoPosition = myTransform.position;
        OneFrameAgoVelocity = velocity;
    }

    void applyVelocity()
    {
        rb.velocity = velocity;
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

    private void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint point in other.contacts)
        {
            Vector3 hitPos = point.point;

            // 向いている方向と、当たった座標までのベクトルを使い、角度を求める
            Vector3 fromMyToHitPosition = hitPos - OneFrameAgoPosition;
            Vector3 toPointVec = MyMath.getHorizontal(fromMyToHitPosition, Vector3.up * -Mathf.Sign(fromMyToHitPosition.y));

            Vector3 cross = Vector3.Cross(toPointVec.normalized, fromMyToHitPosition.normalized);
            float angle = Vector3.SignedAngle(toPointVec.normalized, fromMyToHitPosition.normalized, cross);
            angle = Mathf.Abs(angle);
            if (angle > 90) { angle = Mathf.Abs(angle - 180); }

            if (angle <= maximumAngleOfWallRecognition)
            {
                if (Mathf.Sign(fromMyToHitPosition.x) != Mathf.Sign(myTransform.forward.x)) { continue; }

                int index = hitWallObjects.FindIndex(x => x == other.gameObject);
                if (index == -1) { hitWallObjects.Add(other.gameObject); }
                continue;
            }
            else
            {
                float dot = Vector3.Dot(Vector3.down, OneFrameAgoVelocity);
                if (dot < 0) { continue; }

                int index = hitGroundObjects.FindIndex(x => x == other.gameObject);
                if (index == -1) { hitGroundObjects.Add(other.gameObject); }
            }
        }

        if (hitWallObjects.Count > 0){ isHittingWall = true; }
        if (hitGroundObjects.Count > 0){ isHittingGround = true; }
    }

    private void OnCollisionExit(Collision other)
    {
        hitWallObjects.Remove(other.gameObject);
        hitGroundObjects.Remove(other.gameObject);

        if (hitWallObjects.Count == 0)
        {
            isHittingWall = false; }
        if (hitGroundObjects.Count == 0)
        {
            isHittingGround = false; }
    }
}
