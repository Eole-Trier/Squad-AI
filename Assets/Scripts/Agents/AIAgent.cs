using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace FSMMono
{
    public class AIAgent : Agent
    {
        [SerializeField] AIAgentData _agentData;
        [SerializeField] float followPlayerDistance;
        [SerializeField] float startHealingPlayerHpPercentage;
        [SerializeField] float distanceToHealPlayer;
        private float MaxPercentage = 100f;
        private FSM _brain;
        private PlayerAgent _player;

        private float rand = 0f;
        private float cumulativeChances = 0f;

        private Vector3 offsetFromPlayer = Vector3.right * 5.0f;

        NavMeshAgent NavMeshAgentInst;
        Material MaterialInst;

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
            base.Start();

            NavMeshAgentInst = GetComponent<NavMeshAgent>();

            Renderer rend = transform.Find("Body").GetComponent<Renderer>();
            MaterialInst = rend.material;

            GunTransform = transform.Find("Body/Gun");
            if (GunTransform == null)
                Debug.Log("could not fin gun transform");
            

            Target = Transform.FindAnyObjectByType<PlayerAgent>().transform;

            //NavMeshAgentInst.updatePosition = false;
            _player = FindObjectOfType<PlayerAgent>();
            _brain = new FSM();
            _brain.SetState(FollowPlayer);
            
        }

        private void Update()
        {
            rand = Random.Range(0f, MaxPercentage);
            cumulativeChances = 0f;
            _brain.Update();
        }

        private void FollowPlayer()
        {
            RunToPlayer(offsetFromPlayer);

            if (Input.GetMouseButtonDown(0))
            {
                cumulativeChances += _agentData.SupportFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(SupportFire);
                    return;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                cumulativeChances += _agentData.CoveringFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(CoveringFire);
                    return;
                }
            }
            if (_player.healthComponent.GetHpPercentage() <= startHealingPlayerHpPercentage)
            {
                cumulativeChances += _agentData.HealPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(HealPlayer);
                    return;
                }
            }
            if (false) // enemy shoot on the player
            {
                cumulativeChances += _agentData.ProtectPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(ProtectPlayer);
                    return;
                }
            }
        }

        private void SupportFire()
        {
            if (Input.GetMouseButtonDown(1))
            {
                cumulativeChances += _agentData.CoveringFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(CoveringFire);
                    return;
                }
            }
            if (_player.healthComponent.GetHpPercentage() <= startHealingPlayerHpPercentage)
            {
                cumulativeChances += _agentData.HealPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(HealPlayer);
                    return;
                }
            }
            if (false) // enemy shoot on the player
            {
                cumulativeChances += _agentData.ProtectPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(ProtectPlayer);
                    return;
                }
            }
            _brain.SetState(FollowPlayer);
        }

        private void ProtectPlayer()
        {
            if (Input.GetMouseButtonDown(0))
            {
                cumulativeChances += _agentData.SupportFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(SupportFire);
                    return;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                cumulativeChances += _agentData.CoveringFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(CoveringFire);
                    return;
                }
            }
            if (_player.healthComponent.GetHpPercentage() <= startHealingPlayerHpPercentage)
            {
                cumulativeChances += _agentData.HealPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(HealPlayer);
                    return;
                }
            }
            _brain.SetState(FollowPlayer);
        }

        private void HealPlayer()
        {
            RunToPlayer(Vector3.zero);

            if (!HasReachedPos(distanceToHealPlayer))
                return;
            
            _player.healthComponent.SetHp(_player.healthComponent.CurrentHp() + _agentData.healingPoints);

            if (Input.GetMouseButtonDown(0))
            {
                cumulativeChances += _agentData.SupportFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(SupportFire);
                    return;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                cumulativeChances += _agentData.CoveringFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(CoveringFire);
                    return;
                }
            }
            if (false) // enemy shoot on the player
            {
                cumulativeChances += _agentData.ProtectPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(ProtectPlayer);
                    return;
                }
            }
            _brain.SetState(FollowPlayer);
        }

        private void CoveringFire()
        {
            if (Input.GetMouseButtonDown(0))
            {
                cumulativeChances += _agentData.SupportFirePercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(SupportFire);
                    return;
                }
            }
            if (_player.healthComponent.GetHpPercentage() <= startHealingPlayerHpPercentage)
            {
                cumulativeChances += _agentData.HealPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(HealPlayer);
                    return;
                }
            }
            if (false) // enemy shoot on the player
            {
                cumulativeChances += _agentData.ProtectPlayerPercentage;
                if (rand <= cumulativeChances)
                {
                    _brain.SetState(ProtectPlayer);
                    return;
                }
            }
            _brain.SetState(FollowPlayer);
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
        public bool HasReachedPos(float offset = 0f)
        {
            return NavMeshAgentInst.remainingDistance - NavMeshAgentInst.stoppingDistance <= offset;
        }
       

        #endregion

        #region ActionMethods

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

        void RunToPlayer(Vector3 offset)
        {
            NavMeshAgentInst.SetDestination(Target.position + offset);
        }

        Vector3 velocity = Vector3.zero;

        #endregion
    }
}