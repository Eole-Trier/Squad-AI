using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    public NavMeshAgent NavMeshAgentInst;

    protected void Start()
    {
        GunTransform = transform.Find("Gun");
        rb = GetComponent<Rigidbody>();
    }

    public void StopMove()
    {
        NavMeshAgentInst.isStopped = true;
    }
    public void MoveTo(Vector3 dest)
    {
        NavMeshAgentInst.isStopped = false;
        NavMeshAgentInst.SetDestination(dest);
    }
    public bool HasReachedPos(float offset = 0f)
    {
        return NavMeshAgentInst.remainingDistance - NavMeshAgentInst.stoppingDistance <= offset;
    }
}
