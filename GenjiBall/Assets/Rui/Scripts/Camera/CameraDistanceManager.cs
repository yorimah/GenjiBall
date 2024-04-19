using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;
    
public class CameraDistanceManager : MonoBehaviour
{
    // カメラ本体は回転の中心となる親オブジェクトの子で、親オブジェクトにアタッチするため、ここでカメラ本体をセットする
    [SerializeField] Transform cameraObject;
    [SerializeField] LayerMask obstaclesLayer;

    float maxDistance;
    Vector3 fromMyToCameraVector;
    Transform myTransform;
    private void Start()
    {
        maxDistance = cameraObject.localPosition.z;
        myTransform = transform;
    }

    private void Update()
    {
        fromMyToCameraVector = cameraObject.position - myTransform.position;
        float distance = checkAndGetDistanceToObstacles();
        adjustDistance(distance);
    }

    float checkAndGetDistanceToObstacles()
    {
        Vector3 position = myTransform.position;
        Vector3 direction = fromMyToCameraVector.normalized;
        float distance = Mathf.Abs(maxDistance);

        if (Physics.Raycast(position, direction, out RaycastHit hit, distance, obstaclesLayer))
        {
            return hit.distance;
        }

        return distance;
    }

    void adjustDistance(float distance)
    {
        Vector3 localPosition = cameraObject.localPosition;
        localPosition.z = distance * Mathf.Sign(maxDistance);
        cameraObject.localPosition = localPosition;
    }
}
