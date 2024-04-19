using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjects : MonoBehaviour
{
    // �Z�b�g�����I�u�W�F�N�g�Ɠ������W�ɂ���N���X
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
