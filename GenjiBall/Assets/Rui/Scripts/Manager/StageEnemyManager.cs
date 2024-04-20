using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

public class StageEnemyManager : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;

   [SerializeField] List<Transform> enemyList;

    private void Start()
    {
        Transform[] enemyArray = GetComponentsInChildren<Transform>();
        int length = enemyArray.Length;
        enemyList = new List<Transform>();
        for (int i = 0; i < length; i++)
        {
            if (LayerFunc.checkHitLayer(enemyArray[i].gameObject, enemyLayer) == false) { continue; }

            enemyList.Add(enemyArray[i]);
        }
    }

    public Transform getEnemyAtShortestDistance(Transform fromObject)
    {
        excludeInactivity();

        int length = enemyList.Count;
        if (length == 0) { return null; }

        Transform targetEnemy = enemyList[0];
        Vector3 fromMyToClosestEnemy = targetEnemy.position - fromObject.position;
        for (int i = 1; i < length; i++)
        {
            targetEnemy = enemyList[i];
            float distance = (targetEnemy.position - fromObject.position).sqrMagnitude;
            //�@�������G�Ƃ̋������������ŏ��̋�����蒷���Ȃ珈�������Ȃ�
            if (distance >= fromMyToClosestEnemy.sqrMagnitude) { continue; }
            fromMyToClosestEnemy = targetEnemy.position - fromObject.position;
        }

        return targetEnemy;
    }

    void excludeInactivity()
    {
        int length = enemyList.Count;
        for (int i = 0; i < length; i++)
        {
            if (enemyList[i].gameObject.activeSelf == false)
            {
                enemyList.RemoveAt(i);
                i--;
                length--;
                continue;
            }
        }
    }
}
