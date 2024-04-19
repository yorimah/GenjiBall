using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AddMath;
public class RoundRoute : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public GameObject folderOfFolder;
    public GameObject pointFolder;
    public List<Transform> passingPoint = new List<Transform>();

    // 目指すポイントのインデックス
    int targetIndex;
    Transform myTransform;
    Movement movement;
    Vector3 fromMyToTargetPoint;
    Vector3 fromCurrentToNextPoint;

    public delegate void FinishMovingAction();
    FinishMovingAction finishMovingAction;

    private void Start()
    {
        if (passingPoint.Count == 0){ Debug.LogError("ルートが設定されていません"); }
        myTransform = transform;
        movement = GetComponent<Movement>();

        targetIndex = 0;
        myTransform.position = passingPoint[targetIndex].position;
    }

    private void Update()
    {
        fromMyToTargetPoint = passingPoint[targetIndex].position - myTransform.position;
        fromCurrentToNextPoint = passingPoint[targetIndex].position - passingPoint[getPreviousPointIndex()].position;
        if (checkArrival() == false) { return; }

        nextMove();
        if (checkFinishMoving() == true) { finishMovingAction(); }
    }

    public void setFinishMovingAction(FinishMovingAction action)
    {
        finishMovingAction = action;
    }

    void nextMove()
    {
        targetIndex = getNextPointIndex();
        Vector3 nextDir = (passingPoint[targetIndex].position - passingPoint[getPreviousPointIndex()].position).normalized;
        movement.changeVelocity(nextDir * moveSpeed);
    }

    public Vector3 getDir(int endindex)
    {
        int startIndex = getPreviousPointIndex();
        return (passingPoint[endindex].position - passingPoint[startIndex].position).normalized;
    }

    public int getPointCount()
    {
        return passingPoint.Count;
    }

    // 一つ次のインデックスを得る
    public int getNextPointIndex()
    {
        return MyMath.wrap(targetIndex + 1, 0, getPointCount());
    }

    // 一つ前のインデックスを得る
    public int getPreviousPointIndex()
    {
        return MyMath.wrap(targetIndex - 1, 0, getPointCount());
    }

    public bool checkArrival()
    {
        // 速度のベクトルが向かっている方向ベクトルと違うなら到着したことにする
        return Vector3.Dot(fromCurrentToNextPoint, fromMyToTargetPoint) <= 0;
    }

    bool checkFinishMoving()
    {
        return targetIndex == 0;
    }
}