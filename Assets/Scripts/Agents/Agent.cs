using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Agent : MonoBehaviour
{
    [SerializeField] protected float BulletPower = 1000f;
    [SerializeField] protected GameObject BulletPrefab;

    [SerializeField] public Health healthComponent;
    
    protected Rigidbody rb;
    protected GameObject TargetCursor = null;
    protected GameObject NPCTargetCursor = null;
    protected Transform GunTransform;

    protected void Start()
    {
        GunTransform = transform.Find("Gun");
        rb = GetComponent<Rigidbody>();
    }
}
