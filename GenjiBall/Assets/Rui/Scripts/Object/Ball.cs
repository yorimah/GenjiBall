using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;
using AddMath;

[RequireComponent(typeof(Movement))]
public class Ball : MonoBehaviour, IDirectionable, IPerformer
{
    [SerializeField] int damage;
    [SerializeField, Tooltip("初めの速度")] float startSpeed;
    [SerializeField, Tooltip("加速していく速度")] float addSpeed;
    [SerializeField, Tooltip("１フレームで回転する角度")] float rotationAnglePerFrame;
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
        //　プレイヤーによってボールが跳ね返されるならターゲットを敵にする
        setEnemyAsTarget();
    }

    void IPerformer.ExecutionByEnemy()
    {
        //　敵によってボールが跳ね返されるならターゲットをプレイヤーにする
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
        if (enemy == null) { Debug.Log("敵がいません"); return; }

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
            //　ステージ内の敵がいないならゲームクリアになる
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

        // x軸を軸にして毎秒2度、回転させるQuaternionを作成（変数をrotとする）
        Vector3 eulerAngle = myTransform.eulerAngles;
        eulerAngle.x += (-_angleOfTwoVectors * 2) * Mathf.Abs(_normal.normalized.y);
        eulerAngle.y += (_angleOfTwoVectors * 2) * (1 - Mathf.Abs(_normal.normalized.y));
        myTransform.eulerAngles = eulerAngle;

        //Quaternion rot = Quaternion.AngleAxis(_angleOfTwoVectors * 2, Vector3.up);
        //// 現在の自信の回転の情報を取得する。
        //Quaternion q = myTransform.localRotation;
        //// 合成して、自身に設定
        //myTransform.localRotation = q * rot;
    }
}
