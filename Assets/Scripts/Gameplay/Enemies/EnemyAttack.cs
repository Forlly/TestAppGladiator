using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AudioClip _attackSound;

    public float AttackRange = 1.5f;
    public float DetectionRange = 10f;

    private Transform _target;
    private bool _isAttacking = false;
    public bool IsPlayerLose { get; set; } = false;
    public bool IsAttacking => _isAttacking;
    public void Init(Transform target)
    {
        _target = target;
    }

    public void TryAttack()
    {
        if (_isAttacking || _target == null) return;

        _isAttacking = true;
        _animator.SetBool("IsAttack", true);
        _animator.SetBool("IsRun", false);
        _rb.velocity = Vector3.zero;

        Invoke(nameof(CheckDistanceAfterAttack), 1f);
    }

    private void CheckDistanceAfterAttack()
    {
        if (_target == null) return;

        float dist = Vector3.Distance(transform.position, _target.position);
        if (dist > AttackRange)
        {
            ResetAttackState();
        }
        else
        {
            Invoke(nameof(CheckDistanceAfterAttack), 1f);
        }
    }

    public void ResetAttackState()
    {
        _isAttacking = false;
        _animator.SetBool("IsAttack", false);
    }

    public void SetPlayerLose()
    {
        IsPlayerLose = true;
        _animator.SetBool("IsLose", true);
    }
}