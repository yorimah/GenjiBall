using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AddLayer
{
    public class LayerFunc : MonoBehaviour
    {
        // �^�[�Q�b�g�̃��C���[�����C���[�}�X�N�Ɋ܂܂�Ă��邩��Ԃ�
        public static bool checkHitLayer(GameObject targetObject, LayerMask layermask)
        {
            int targetLayer = targetObject.layer;
            targetLayer = 1 << targetLayer;
            if ((targetLayer & layermask) > 0) { return true; }
            else { return false; }
        }
    }
}
