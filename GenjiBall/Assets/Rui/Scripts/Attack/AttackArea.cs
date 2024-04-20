using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

public class AttackArea : MonoBehaviour
{
    [SerializeField] LayerMask hitLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (LayerFunc.checkHitLayer(other.gameObject, hitLayerMask) == false) { return; }

        if (other.TryGetComponent(out IDamageable damageable) == false) { return; }

        damageable.damage(0);
    }
}
