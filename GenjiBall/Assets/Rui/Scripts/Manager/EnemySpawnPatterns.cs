using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawnPatterns : MonoBehaviour
{
    [SerializeField] List<SpawnEnemyData> spawnData;

    int count = 0;
    Transform myTransform;
    GenerateObject generateObject;

    private void Start()
    {
        myTransform = transform;
        generateObject = new GenerateObject();
    }

    private void FixedUpdate()
    {
        count++;
        if (isItTimeToSpawn() == false) { return; }

        spawnNextEnemy();
    }

    bool isItTimeToSpawn()
    {
        if (spawnData.Count == 0) { return false; }

        // ゲーム開始からのカウントが指定されたカウントを過ぎると生成できるようにする
        return spawnData[0].spawnCount <= count;
    }

    void spawnNextEnemy()
    {
        Vector3 position = myTransform.position;
        RoundRoute route = generateObject.generate(spawnData[0].enemy.gameObject, position).GetComponent<RoundRoute>();
        route.passingPoint = new List<Transform>();
        Transform routeParent = GameObject.Find($"{spawnData[0].enemy.gameObject.name}_Points").transform;
        foreach (Transform item in routeParent)
        {
            route.passingPoint.Add(item);
        }

        spawnData.RemoveAt(0);
        if (spawnData.Count == 0) { gameObject.SetActive(false); }
    }
}

[Serializable]
public class SpawnEnemyData
{
    public Enemy enemy;
    public int spawnCount;
}