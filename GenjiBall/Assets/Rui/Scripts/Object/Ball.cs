using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

[RequireComponent(typeof(Movement))]
public class Ball : MonoBehaviour
{
    [SerializeField,Tooltip("èâÇﬂÇÃë¨ìx")] float startSpeed;
    [SerializeField,Tooltip("â¡ë¨ÇµÇƒÇ¢Ç≠ë¨ìx")] float addSpeed;
    [SerializeField,Tooltip("ÇPÉtÉåÅ[ÉÄÇ≈âÒì]Ç∑ÇÈäpìx")] float rotationAnglePerFrame;
    [SerializeField] FindObject findEnemy;

    float currentSpeed;
    Transform myTransform;
    [SerializeField] Transform targetEnemy;
    Movement movement;

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
        if (targetEnemy == null) { return; }

        Vector3 fromMyTotarget = (targetEnemy.position - myTransform.position).normalized;
        Vector3 forward = myTransform.forward;
        Vector3 cross = Vector3.Cross(forward, fromMyTotarget);
        float angle = Vector3.SignedAngle(forward, fromMyTotarget, cross);
        float turnPercent = rotationAnglePerFrame / angle;
        myTransform.forward = Vector3.Lerp(forward, fromMyTotarget, turnPercent);
    }

    void flyToEnemy()
    {
        if (targetEnemy == null) { return; }

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

        targetEnemy = foundList[index].transform;
        myTransform.forward = fromMyToClosestEnemy.normalized;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.TryGetComponent(out Movement _movement) == false) { return; }

        currentSpeed += addSpeed;
        if (LayerFunc.checkHitLayer(coll.gameObject, GameManager.instance.playerLayer) == true)
        {
            findEnemy.startFind();
        }

        if (LayerFunc.checkHitLayer(coll.gameObject, GameManager.instance.enemyLayer) == true)
        {
            Vector3 dir = GameManager.instance.player.transform.position - myTransform.position;
            movement.changeVelocity(dir.normalized * currentSpeed);
            myTransform.forward = dir.normalized;
            targetEnemy = null;
        }
    }
}
