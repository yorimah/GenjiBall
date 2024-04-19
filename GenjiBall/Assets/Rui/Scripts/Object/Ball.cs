using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLayer;

[RequireComponent(typeof(Movement))]
public class Ball : MonoBehaviour
{
    [SerializeField] LayerMask hitLayerMask;

    Movement movement;

    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (LayerFunc.checkHitLayer(coll.gameObject, hitLayerMask) == false) { return; }

        if(coll.gameObject.TryGetComponent(out Movement _movement) == false) { return; }

        movement.changeVelocity(_movement.Velocity);
    }
}
