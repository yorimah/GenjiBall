using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : Charactor
    {
        [SerializeField] AttackPatternData[] attackPatternDatas;

        int attackIndex;
        string defaultAttackName;
        Transform myTransform;

        private void OnDisable()
        {
            charactorAnimation.endAttackAction -= NextAttack;
        }

        public new void Start()
        {
            base.Start();
            dieAction = _DieAction;
            myTransform = transform;
            charactorAnimation.endAttackAction += NextAttack;
            defaultAttackName = charactorAnimation.getAnimationStateMotionName("Attack");
            charactorAnimation.overrideAnimationClip(attackPatternDatas[0].clip[0], defaultAttackName);
            charactorAnimation.startAttack();
        }

        void _DieAction()
        {
            gameObject.SetActive(false);
        }

        void NextAttack()
        {
            attackIndex++;
            AttackPatternData _pattarnData = attackPatternDatas[0];
            if (attackIndex >= _pattarnData.clip.Length) { return; }

            charactorAnimation.startAttack();
            charactorAnimation.playAnimation(defaultAttackName);
            StartCoroutine(overrideAnimationClip(_pattarnData));
        }

        IEnumerator overrideAnimationClip(AttackPatternData _patternData)
        {
            yield return null;
            charactorAnimation.overrideAnimationClip(_patternData.clip[attackIndex], defaultAttackName);
        }

    }
}
