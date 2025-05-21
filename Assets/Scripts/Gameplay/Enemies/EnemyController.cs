using UnityEngine;

namespace GameMechanic
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyAttack _attack;
        [SerializeField] private EnemyHealth _health;

        public Transform Target;
        public EnemyPool Pool { get; set; }
        public bool IsAttacking => _attack.IsAttacking;
        public bool IsDead => _health.IsDead;
        public int EnemyTypeID { get; set; }
        public void ResetState()
        {
            _health.Reset();
            _movement.StopImmediately();
            _attack.ResetAttackState();
            _attack.IsPlayerLose = false;
            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            CancelInvoke(); 
        }

        private void Start()
        {
            _health.Init();
            _health.SetController(this);

            _movement.Init(Target);
            _attack.Init(Target);
        }

        private void Update()
        {
            _health.RotateCanvasToCamera();
        }

        private void FixedUpdate()
        {
            if (_health.IsDead || _attack.IsPlayerLose || _health.IsKnockedBack) return;

            float distance = Vector3.Distance(transform.position, Target.position);
            if (distance <= _attack.AttackRange)
            {
                _attack.TryAttack();
            }
            else if (distance <= _attack.DetectionRange)
            {
                _movement.MoveTowards(Target);
                _attack.ResetAttackState();
            }
            else
            {
                _movement.Idle();
            }
        }

        public void TakeDamage(int dmg, Vector3 hitSource)
        {
            _health.TakeDamage(dmg, hitSource);
        }
        public void DieAndReturnToPool()
        {
            _movement.StopImmediately();
            gameObject.SetActive(false); 
            Pool.Return(EnemyTypeID, this);
        }

        public void OnPlayerLose()
        {
            if (_health.IsDead) return;

            _attack.SetPlayerLose();
            _movement.StopImmediately();
        }
    }
}