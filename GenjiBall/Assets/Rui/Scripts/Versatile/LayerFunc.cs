using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AddLayer
{
    public class LayerFunc : MonoBehaviour
    {
        // ターゲットのレイヤーがレイヤーマスクに含まれているかを返す
        public static bool checkHitLayer(GameObject targetObject, LayerMask layermask)
        {
            int targetLayer = targetObject.layer;
            targetLayer = 1 << targetLayer;
            if ((targetLayer & layermask) > 0) { return true; }
            else { return false; }
        }
    }
}
