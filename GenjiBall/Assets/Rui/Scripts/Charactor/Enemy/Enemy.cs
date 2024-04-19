using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveScale;

    float count;
    Vector3 defaultPosition;
    Transform myTransform;
    public new void Start()
    {
        base.Start();
        dieAction = _dieAction;
        myTransform = transform;
        defaultPosition = myTransform.position;
    }

    void _dieAction()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        count += moveSpeed;
        Vector3 pos = new Vector3();
        pos.x = Mathf.Sin(Mathf.Deg2Rad * count) * moveSpeed;
        myTransform.position = defaultPosition + pos;
    }
}
