﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

namespace FSMMono
{
    public class AIAgent : MonoBehaviour, IDamageable
    {

        [SerializeField]
        int MaxHP = 100;
        [SerializeField]
        float BulletPower = 1000f;
        [SerializeField]
        GameObject BulletPrefab;

        //Defense Formation Setup
        public float baseRadius = 5f;
        public float radiusStep = 0.5f;

        [SerializeField]
        Slider HPSlider = null;

        Transform GunTransform;
        NavMeshAgent NavMeshAgentInst;
        Material MaterialInst;

        bool IsDead = false;
        int CurrentHP;

        [SerializeField] List<GameObject> units = new();
        private List<Vector3> unitsTargetPos = new();
        private List<Quaternion> unitsTargetRot = new();

        private void SetMaterial(Color col)
        {
            MaterialInst.color = col;
        }

        public void SetWhiteMaterial() { SetMaterial(Color.white); }
        public void SetRedMaterial() { SetMaterial(Color.red); }
        public void SetBlueMaterial() { SetMaterial(Color.blue); }
        public void SetYellowMaterial() { SetMaterial(Color.yellow); }

        public Transform Target;

        #region MonoBehaviour

        private void Awake()
        {
            CurrentHP = MaxHP;

            NavMeshAgentInst = GetComponent<NavMeshAgent>();

            Renderer rend = transform.Find("Body").GetComponent<Renderer>();
            MaterialInst = rend.material;

            GunTransform = transform.Find("Body/Gun");
            if (GunTransform == null)
                Debug.Log("could not fin gun transform");

            if (HPSlider != null)
            {
                HPSlider.maxValue = MaxHP;
                HPSlider.value = CurrentHP;
            }

            Target = Transform.FindAnyObjectByType<PlayerAgent>().transform;

            //NavMeshAgentInst.updatePosition = false;
        }

        private void Start()
        {
        }

        private void OnTriggerEnter(Collider other)
        {

        }

        private void OnTriggerExit(Collider other)
        {

        }
        private void OnDrawGizmos()
        {
        }

        #endregion

        #region Perception methods

        #endregion

        #region MoveMethods
        public void StopMove()
        {
            NavMeshAgentInst.isStopped = true;
        }
        public void MoveTo(Vector3 dest)
        {
            NavMeshAgentInst.isStopped = false;
            NavMeshAgentInst.SetDestination(dest);
        }
        public bool HasReachedPos()
        {
            return NavMeshAgentInst.remainingDistance - NavMeshAgentInst.stoppingDistance <= 0f;
        }

        public void DefenseFormation()
        {
            int unitCount = units.Count;
            float radius = baseRadius + radiusStep * unitCount;
            float angleStep = 360f / unitCount;

            
        }

        public void MoveFormation()
        {
            NavMeshAgentInst.SetDestination(Target.position + Vector3.forward * 5f);
        }

        #endregion

        #region ActionMethods

        public void AddDamage(int amount)
        {
            CurrentHP -= amount;
            if (CurrentHP <= 0)
            {
                IsDead = true;
                CurrentHP = 0;
            }

            if (HPSlider != null)
            {
                HPSlider.value = CurrentHP;
            }
        }

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

        Vector3 velocity = Vector3.zero;

        public void Update()
        {
            NavMeshAgentInst.SetDestination(Target.position + Vector3.right);
        }
        #endregion
    }
}