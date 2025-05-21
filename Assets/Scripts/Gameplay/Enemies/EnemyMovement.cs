using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 3f;

    private Transform _target;

    public void Init(Transform target)
    {
        _target = target;
    }

    public void MoveTowards(Transform target)
    {
        if (target == null) return;

        _animator.SetBool("IsRun", true);
        _animator.SetBool("IsAttack", false);

        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 velocity = dir * _speed;

        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRot, Time.fixedDeltaTime * 10f));
    }

    public void Idle()
    {
        _animator.SetBool("IsRun", false);
        _animator.SetBool("IsAttack", false);
        _rb.velocity = Vector3.zero;
    }

    public void StopImmediately()
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool("IsRun", false);
    }
}