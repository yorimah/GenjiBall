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

    // �ڎw���|�C���g�̃C���f�b�N�X
    int targetIndex;
    Transform myTransform;
    Movement movement;
    Vector3 fromMyToTargetPoint;
    Vector3 fromCurrentToNextPoint;

    public delegate void FinishMovingAction();
    FinishMovingAction finishMovingAction;

    private void Start()
    {
        if (passingPoint.Count == 0){ Debug.LogError("���[�g���ݒ肳��Ă��܂���"); }
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

    // ����̃C���f�b�N�X�𓾂�
    public int getNextPointIndex()
    {
        return MyMath.wrap(targetIndex + 1, 0, getPointCount());
    }

    // ��O�̃C���f�b�N�X�𓾂�
    public int getPreviousPointIndex()
    {
        return MyMath.wrap(targetIndex - 1, 0, getPointCount());
    }

    public bool checkArrival()
    {
        // ���x�̃x�N�g�����������Ă�������x�N�g���ƈႤ�Ȃ瓞���������Ƃɂ���
        return Vector3.Dot(fromCurrentToNextPoint, fromMyToTargetPoint) <= 0;
    }

    bool checkFinishMoving()
    {
        return targetIndex == 0;
    }
}