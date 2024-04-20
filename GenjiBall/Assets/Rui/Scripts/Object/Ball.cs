using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

[RequireComponent(typeof(Movement))]
public class Ball : MonoBehaviour, IDamageable
{
    [SerializeField, Tooltip("èâÇﬂÇÃë¨ìx")] float startSpeed;
    [SerializeField, Tooltip("â¡ë¨ÇµÇƒÇ¢Ç≠ë¨ìx")] float addSpeed;
    [SerializeField, Tooltip("ÇPÉtÉåÅ[ÉÄÇ≈âÒì]Ç∑ÇÈäpìx")] float rotationAnglePerFrame;
    [SerializeField] FindObject findEnemy;

    float currentSpeed;
    Transform myTransform;
    [SerializeField] Transform target;
    Movement movement;

    void IDamageable.damage(int value)
    {
        findEnemy.startFind();
        currentSpeed += addSpeed;
    }


    private void Start()
    {
        currentSpeed = startSpeed;
        myTransform = transform;
        movement = GetComponent<Movement>();
        findEnemy.setFoundObjecyAction((List<GameObject> foundList) => setTargetEnemy(foundList));
    }

    private void Update()
    {
        turnToEnemy();
        flyToEnemy();
    }

    void turnToEnemy()
    {
        if (target == null) { return; }

        Vector3 fromMyTotarget = (target.position - myTransform.position).normalized;
        Vector3 forward = myTransform.forward;
        Vector3 cross = Vector3.Cross(forward, fromMyTotarget);
        float angle = Vector3.SignedAngle(forward, fromMyTotarget, cross);
        float turnPercent = rotationAnglePerFrame / angle;
        myTransform.forward = Vector3.Lerp(forward, fromMyTotarget, turnPercent);
    }

    void flyToEnemy()
    {
        if (target == null) { return; }

        movement.changeVelocity(myTransform.forward * currentSpeed);
    }

    void setTargetEnemy(List<GameObject> foundList)
    {
        int index = 0;
        int length = foundList.Count;
        Vector3 fromMyToClosestEnemy = foundList[0].transform.position - myTransform.position;

        if (length > 1)
        {
            for (int i = 1; i < length; i++)
            {
                float distance = (foundList[i].transform.position - myTransform.position).sqrMagnitude;
                //Å@å©Ç¬ÇØÇΩìGÇ∆ÇÃãóó£Ç™å©Ç¬ÇØÇΩç≈è¨ÇÃãóó£ÇÊÇËí∑Ç¢Ç»ÇÁèàóùÇÇµÇ»Ç¢
                if (distance >= fromMyToClosestEnemy.sqrMagnitude) { continue; }

                index = i;
                fromMyToClosestEnemy = foundList[i].transform.position - myTransform.position;
            }
        }

        target = foundList[index].transform;
        myTransform.forward = fromMyToClosestEnemy.normalized;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.TryGetComponent(out IDamageable damageable) == false) { return; }

        damageable.damage(10);
        if (LayerFunc.checkHitLayer(coll.gameObject, GameManager.instance.enemyLayer) == true)
        {
            currentSpeed += addSpeed;
            Vector3 dir = GameManager.instance.player.transform.position - myTransform.position;
            target = GameManager.instance.player.transform;
            myTransform.forward = dir.normalized;
        }
    }
}
