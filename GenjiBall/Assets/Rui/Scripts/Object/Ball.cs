using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;
using AddMath;

[RequireComponent(typeof(Movement))]
public class Ball : MonoBehaviour, IDirectionable, IPerformer
{
    [SerializeField] int damage;
    [SerializeField, Tooltip("���߂̑��x")] float startSpeed;
    [SerializeField, Tooltip("�������Ă������x")] float addSpeed;
    [SerializeField, Tooltip("�P�t���[���ŉ�]����p�x")] float rotationAnglePerFrame;
    [SerializeField] StageEnemyManager stageEnemyManager;
    [SerializeField] LayerMask stageLayer;

    float currentSpeed;
    Transform myTransform;
    [SerializeField] Transform target;
    Movement movement;

    void IDirectionable.GiveA_Direction(Vector3 _direction)
    {
        myTransform.forward = _direction.normalized;
    }

    void IPerformer.ExecutionByPlayer()
    {
        //�@�v���C���[�ɂ���ă{�[�������˕Ԃ����Ȃ�^�[�Q�b�g��G�ɂ���
        setEnemyAsTarget();
    }

    void IPerformer.ExecutionByEnemy()
    {
        //�@�G�ɂ���ă{�[�������˕Ԃ����Ȃ�^�[�Q�b�g���v���C���[�ɂ���
        setPlayerAsTarget();
    }

    private void Start()
    {
        currentSpeed = startSpeed;
        myTransform = transform;
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        //turnToTarget();
        flyToTarget();
        //if (movement.isHittingWall == true) { movement.changeVelocity_x(-movement.Velocity.x); myTransform.forward = movement.Velocity; }
        //if (movement.isHittingGround == true) { movement.changeVelocity_y(-movement.Velocity.y); myTransform.forward = movement.Velocity; }
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

    void setEnemyAsTarget()
    {
        Transform enemy = stageEnemyManager.getEnemyAtShortestDistance(myTransform);
        if (enemy == null) { Debug.Log("�G�����܂���"); return; }

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
        currentSpeed += addSpeed;
    }

    private void OnCollisionEnter(Collision coll)
    {
        HitStage(coll);

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
        }
    }

    void HitStage(Collision coll)
    {
        if (LayerFunc.checkHitLayer(coll.gameObject, stageLayer) == false) { return; }

        ContactPoint point = coll.contacts[0];

        Vector3 _normal = point.normal;
        Vector3 _normalsOfTwoVectors = Vector3.Cross(_normal, movement.OneFrameAgoVelocity.normalized).normalized;
        //_normalsOfTwoVectors.y = Mathf.Abs(_normalsOfTwoVectors.y);
        float _angleOfTwoVectors = Vector3.SignedAngle(_normal, movement.OneFrameAgoVelocity.normalized, _normalsOfTwoVectors);
        Debug.Log(_angleOfTwoVectors);
        if (_angleOfTwoVectors > 0) { _angleOfTwoVectors -= 90; }
        if (_angleOfTwoVectors < 0) { _angleOfTwoVectors += 90; }
        Debug.Log(_angleOfTwoVectors);

        //int _signOfAngleOfTwoVectors = (int)Mathf.Sign(_angleOfTwoVectors); 
        //_angleOfTwoVectors = (Mathf.Abs(_angleOfTwoVectors) - 90) * _signOfAngleOfTwoVectors;

        //Debug.Log($"{-movement.OneFrameAgoVelocity.normalized}:{_normal}:{_normalsOfTwoVectors}");

        // x�������ɂ��Ė��b2�x�A��]������Quaternion���쐬�i�ϐ���rot�Ƃ���j
        Vector3 eulerAngle = myTransform.eulerAngles;
        eulerAngle.x += (-_angleOfTwoVectors * 2) * Mathf.Abs(_normal.normalized.y);
        eulerAngle.y += (_angleOfTwoVectors * 2) * (1 - Mathf.Abs(_normal.normalized.y));
        myTransform.eulerAngles = eulerAngle;

        //Quaternion rot = Quaternion.AngleAxis(_angleOfTwoVectors * 2, Vector3.up);
        //// ���݂̎��M�̉�]�̏����擾����B
        //Quaternion q = myTransform.localRotation;
        //// �������āA���g�ɐݒ�
        //myTransform.localRotation = q * rot;
    }
}
