using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogiCardDestroy : MonoBehaviour {

    private Explodable _explodable;

    void Start()
    {
        _explodable = GetComponent<Explodable>();
        _explodable.explode();
        ExplosionForce.Instance.doExplosion(transform.position);
    }
}