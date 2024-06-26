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

    // リスト内の非アクティブのオブジェクトをリストから除外する
    public void excludeInactivity()
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

    public Transform getEnemyAtShortestDistance(Transform fromObject)
    {
        int length = enemyList.Count;
        if (length == 0) { return null; }

        Transform targetEnemy = enemyList[0];
        Vector3 fromMyToClosestEnemy = targetEnemy.position - fromObject.position;
        for (int i = 1; i < length; i++)
        {
            targetEnemy = enemyList[i];
            float distance = (targetEnemy.position - fromObject.position).sqrMagnitude;
            //　見つけた敵との距離が見つけた最小の距離より長いなら処理をしない
            if (distance >= fromMyToClosestEnemy.sqrMagnitude) { continue; }
            fromMyToClosestEnemy = targetEnemy.position - fromObject.position;
        }

        return targetEnemy;
    }

    public int getNumberOfEnemyRemaining()
    {
        return enemyList.Count;
    }
}
