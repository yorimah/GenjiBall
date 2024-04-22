using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

public class AttackArea : MonoBehaviour
{
    [SerializeField] LayerMask hitLayerMask;
    Transform mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerFunc.checkHitLayer(other.gameObject, hitLayerMask) == false) { return; }

        if (other.TryGetComponent(out IDirectionable directionable) == false) { return; }

        directionable.GiveA_Direction(mainCamera.forward);
        if (other.TryGetComponent(out IPerformer performer) == false) { return; }

        performer.ExecutionByPlayer();
    }
}
