using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAgent : Agent
{
    [SerializeField]  List<GameObject> pathGo;
    [SerializeField]  float _idleWaitTime;
    [SerializeField]  float _shootWaitTime;
    [SerializeField]  float _shootFrequency = 1f;
    [SerializeField] float _shootRange;

    private float _idleTime = 0f;
    private float _shootWaitTimer = 0f;

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
        _brain.Update();
    }

    private void Idle()
    {
        StopMove();
        _idleTime += Time.deltaTime;

        if (_idleTime >= _idleWaitTime)
        {
            _idleTime = 0;
            _brain.SetState(Patrol);
        }
        if (_target)
        {
            _idleTime = 0;
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
            _brain.SetState(Chase);
        }
    }

    private void Chase()
    {
        if (!_target)
        {
            _brain.SetState(Idle);
            return;
        }

        MoveToTarget();
        if (Vector3.Distance(transform.position, _target.transform.position) <= _shootRange)
        {
            _brain.SetState(Shoot);
        }
    }

    private void Shoot()
    {
        _shootWaitTimer += Time.deltaTime;

        if (!_target)
        {
            _brain.SetState(Idle);
            return;
        }
        if (Time.time >= NextShootDate)
        {
            NextShootDate = Time.time + _shootFrequency;
            ShootToPosition(_target.transform.position);
        }
        if (_shootWaitTimer < _shootWaitTime)
        {
            StopMove();
            return;
        }
        _shootWaitTimer = 0;
        _brain.SetState(Chase);
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
