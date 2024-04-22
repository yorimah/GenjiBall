using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

[RequireComponent(typeof(Movement))]
public class Ball : MonoBehaviour, IDamageable
{
    [SerializeField] int damage;
    [SerializeField, Tooltip("���߂̑��x")] float startSpeed;
    [SerializeField, Tooltip("�������Ă������x")] float addSpeed;
    [SerializeField, Tooltip("�P�t���[���ŉ�]����p�x")] float rotationAnglePerFrame;
    [SerializeField] StageEnemyManager stageEnemyManager;

    float currentSpeed;
    Transform myTransform;
    [SerializeField] Transform target;
    Movement movement;

    void IDamageable.damage(int _)
    {
        setEnemyAsTargets();
    }

    private void Start()
    {
        currentSpeed = startSpeed;
        myTransform = transform;
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        turnToTarget();
        flyToTarget();
    }

    void turnToTarget()
    {
        if (target == null) { return; }

        Vector3 fromMyTotarget = (target.position - myTransform.position).normalized;
        Vector3 forward = myTransform.forward;
        Vector3 cross = Vector3.Cross(forward, fromMyTotarget);
        float angle = Vector3.SignedAngle(forward, fromMyTotarget, cross);
        float turnPercent = rotationAnglePerFrame / angle;
        myTransform.forward = Vector3.Lerp(forward, fromMyTotarget, turnPercent);
    }

    void flyToTarget()
    {
        if (target == null) { return; }

        movement.changeVelocity(myTransform.forward * currentSpeed);
    }

    void setEnemyAsTargets()
    {
        Transform enemy = stageEnemyManager.getEnemyAtShortestDistance(myTransform);
        if (enemy == null) { Debug.Log("�G�����܂���");return; }

        setTarget(enemy);
    }

    void setPlayerAsTarget()
    {
        Transform player = GameManager.instance.player.transform;
        setTarget(player);
    }

    void setTarget(Transform target)
    {
        this.target = target.transform;
        Vector3 dir = target.position - myTransform.position;
        myTransform.forward = dir.normalized;
        currentSpeed += addSpeed;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.TryGetComponent(out IDamageable damageable) == false) { return; }

        damageable.damage(damage);

        if (LayerFunc.checkHitLayer(coll.gameObject, GameManager.instance.playerLayer) == true)
        {
            GameManager.instance.gameOver();
            movement.stopMoving();
            target = null;
            return;
        }

        if (LayerFunc.checkHitLayer(coll.gameObject, GameManager.instance.enemyLayer) == true)
        {
            stageEnemyManager.excludeInactivity();
            //�@�X�e�[�W���̓G�����Ȃ��Ȃ�Q�[���N���A�ɂȂ�
            if (stageEnemyManager.getNumberOfEnemyRemaining() == 0)
            {
                GameManager.instance.gameClear();
                movement.stopMoving();
                target = null;
                return; 
            }
            
            //�@�G�ɓ�����Ǝ��̓v���C���[�Ɍ�����
            setPlayerAsTarget();
        }
    }
}
