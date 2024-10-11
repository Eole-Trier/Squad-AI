using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace FSMMono
{
    public class AIAgent : Agent
    {
        private float MaxPercentage = 100f;
        private FSM _brain;
        private PlayerAgent _player;
        [SerializeField] private AIAgentData _agentData;

        NavMeshAgent NavMeshAgentInst;
        Material MaterialInst;

        [SerializeField] float followPlayerDistance;
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
            _brain.Update();
        }

        private void FollowPlayer()
        {
            float rand = Random.Range(0f, MaxPercentage);
            float cumulativeChances = 0f;

            if (Input.GetMouseButtonDown(0))
            {
                cumulativeChances += _agentData.SupportFirePercentage;
                if (rand <= cumulativeChances)
                    _brain.SetState(SupportFire);
            }
            if (Input.GetMouseButtonDown(1))
            {
                cumulativeChances += _agentData.CoveringFirePercentage;
                if (rand <= cumulativeChances)
                    _brain.SetState(CoveringFire);
            }
        }

        private void SupportFire()
        {

        }

        private void ProtectPlayer()
        {
           
        }

        private void HealPlayer()
        {
          
        }

        private void CoveringFire()
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

        Vector3 velocity = Vector3.zero;

        public void FixedUpdate()
        {
            // ugly hard coded position next to the player
            NavMeshAgentInst.SetDestination(Target.position + Vector3.right * 5.0f);
        }
        #endregion
    }
}