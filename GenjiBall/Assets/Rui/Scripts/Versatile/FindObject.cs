using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

public class FindObject : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    int count;
    Collider _collider;
    List<GameObject> foundObjectList;

    public delegate void FoundObjectAction(List<GameObject> objects);
    FoundObjectAction foundObjectAction;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void setFoundObjecyAction(FoundObjectAction action)
    {
        foundObjectAction = action;
    }

    public void startFind()
    {
        foundObjectList = new List<GameObject>();
        _collider.enabled = true;
        count = 2;
        StartCoroutine(find());
    }

    IEnumerator find()
    {
        yield return null;

        _collider.enabled = false;
        //　対象のオブジェクトが見つからなかった時は処理をしない
        if (foundObjectList.Count == 0) { Debug.Log("見つからなかった"); yield break; }

        if (foundObjectAction != null) { foundObjectAction(foundObjectList); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerFunc.checkHitLayer(other.gameObject, targetLayer) == false) { return; }

        foundObjectList.Add(other.gameObject);
    }
}
