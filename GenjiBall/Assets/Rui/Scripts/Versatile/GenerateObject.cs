using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UseObjectPool;

public class GenerateObject
{
    public GameObject generate(GameObject obje, Vector2 pos = default, Vector2 dir = default)
    {
        if (ObjectPool.checkSetting(obje) == false) { ObjectPool.Set(obje); }
        GameObject generatedObje = ObjectPool.Get(obje);
        generatedObje.transform.position = pos;
        generatedObje.transform.right = dir;

        return generatedObje;
    }
}
