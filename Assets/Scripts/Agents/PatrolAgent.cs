using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAgent : Agent
{
    [SerializeField]  List<GameObject> pathGo;
    [SerializeField]  float _idleWaitTime;
    [SerializeField]  float _shootFrequency = 1f;

    private FSM _brain;
    private int _pathIndex;
    float NextShootDate = 0f;

    private GameObject _target;

    void ShootToPosition(Vector3 pos)
    {
        // look at target position
        transform.LookAt(pos + Vector3.up * transform.position.y);

        // instantiate bullet
        if (BulletPrefab)
        {
            GameObject bullet = Instantiate<GameObject>(BulletPrefab, GunTransform.position + transform.forward * 0.5f, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * BulletPower);
        }
    }
    private new void Start()
    {
        base.Start();

        GunTransform = transform.Find("Body/Gun");
        if (GunTransform == null)
            Debug.Log("could not find gun transform");

        _brain = new FSM();
        _brain.SetState(Idle);
    }

    void Update()
    {
        if (_target && Time.time >= NextShootDate)
        {
            NextShootDate = Time.time + _shootFrequency;
            ShootToPosition(_target.transform.position);
        }
        _brain.Update();
    }

    private void Idle()
    {
        StopMove();
        _idleWaitTime += Time.deltaTime;

        if (_idleWaitTime >= 2f)
        {
            _idleWaitTime = 0;
            _brain.SetState(Patrol);
        }
        if (_target)
        {
            _idleWaitTime = 0;
            _brain.SetState(Chase);
        }
    }

    private void Patrol()
    {
        MoveTo(pathGo[_pathIndex].transform.position);
        if (HasReachedPos())
        {
            StopMove();
            _pathIndex = (_pathIndex + 1) % pathGo.Count;
        }
        if (_target)
        {
            _idleWaitTime = 0;
            _brain.SetState(Chase);
        }
    }

    private void Chase()
    {
        MoveToTarget();
        if (!_target)
        {
            _brain.SetState(Idle);
        }
    }

    public void MoveToTarget()
    {
        if (_target == null)
            return;
        Vector3 targetPos = _target.transform.position;
        targetPos.y = 0f;
        MoveTo(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_target == null && other.gameObject.layer == LayerMask.NameToLayer("Allies"))
        {
            _target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_target != null && other.gameObject == _target)
        {
            _target = null;
        }
    }
}
