using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjects : MonoBehaviour
{
    // セットしたオブジェクトと同じ座標にするクラス
    [SerializeField] Transform followObject;

    Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }

    private void Update()
    {
        myTransform.position = followObject.position;
    }
}
