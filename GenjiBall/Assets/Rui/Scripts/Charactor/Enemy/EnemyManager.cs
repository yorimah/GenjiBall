using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : Charactor
    {
        [Header("")]
        [SerializeField] float moveSpeed;
        [SerializeField] float moveScale;

        float count;
        Vector3 defaultPosition;
        Transform myTransform;
        public new void Start()
        {
            base.Start();
            dieAction = _DieAction;
            myTransform = transform;
            defaultPosition = myTransform.position;
        }

        void _DieAction()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {

            count += moveSpeed;
            Vector3 pos = new Vector3();
            pos.x = Mathf.Sin(Mathf.Deg2Rad * count) * moveSpeed;
            myTransform.position = defaultPosition + pos;
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.TryGetComponent(out IDirectionable _directionable) == false) { return; }

            Vector3 angle;
            angle.x = Random.value * 2 - 1;
            angle.y = Random.value;
            angle.z = Random.value * 2 - 1;
            _directionable.GiveA_Direction(angle);
            if (other.gameObject.TryGetComponent(out IPerformer performer) == false) { return; }

            performer.ExecutionByEnemy();
        }
    }
}
