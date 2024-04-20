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
    [SerializeField] StageEnemyManager stageEnemyManager;

    float currentSpeed;
    Transform myTransform;
    [SerializeField] Transform target;
    Movement movement;

    void IDamageable.damage(int value)
    {
        setTargetEnemy();
        currentSpeed += addSpeed;
    }


    private void Start()
    {
        currentSpeed = startSpeed;
        myTransform = transform;
        movement = GetComponent<Movement>();
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

    void setTargetEnemy()
    {
        target = stageEnemyManager.getEnemyAtShortestDistance(myTransform);
        if (target == null) { Debug.Log("ìGÇ™Ç¢Ç‹ÇπÇÒ");return; }

        Vector3 dir = target.position - myTransform.position;
        myTransform.forward = dir.normalized;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.TryGetComponent(out IDamageable damageable) == false) { return; }

        damageable.damage(10);
        if (LayerFunc.checkHitLayer(coll.gameObject, GameManager.instance.enemyLayer) == true)
        {
            currentSpeed += addSpeed;
            Transform player= GameManager.instance.player.transform;
            Vector3 dir = player.position - myTransform.position;
            target = player.transform;
            myTransform.forward = dir.normalized;
        }
    }
}
