using DG.Tweening;
using GameMechanic;
using GameProject.Levels;
using Project;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private Image healthBar;
    [SerializeField] private Canvas canvasHP;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip damageClip;

    public bool IsKnockedBack { get; private set; }
    public bool IsDead { get; private set; }

    private int currentHP;
    private EnemyController _controller;

    public void SetController(EnemyController controller)
    {
        _controller = controller;
    }

    public void Init()
    {
        currentHP = maxHP;
        UpdateHealthBar();
        canvasHP.worldCamera = Camera.main;
        rb.isKinematic = false;
    }

    public void TakeDamage(int damage, Vector3 hitSource)
    {
        if (IsDead) return;

        SoundManager.GetInstance().PlaySound(damageClip);

        currentHP -= damage;
        UpdateHealthBar();
        animator.SetBool("IsDamage", true);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 knockbackDir = (transform.position - hitSource).normalized;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        IsKnockedBack = true;
        Invoke(nameof(ResetKnockback), 0.5f);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        animator.SetBool("Die", true);
        canvasHP.transform.DOScale(Vector3.zero, 0.3f);
        rb.isKinematic = true;

        GetComponent<Collider>().enabled = false;

        LevelManager.Instance.GetBoard().EnemyDefeated();
        LevelManager.Instance.IncreaseEnemies();

        Invoke(nameof(ReturnToPool), 3f);
    }

    private void ReturnToPool()
    {
        _controller.DieAndReturnToPool();
    }


    
    
    private void ResetKnockback()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        IsKnockedBack = false;
        animator.SetBool("IsDamage", false);
    }
    public void Reset()
    {
        IsDead = false;
        IsKnockedBack = false;
        currentHP = maxHP;

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GetComponent<Collider>().enabled = true;

        canvasHP.transform.localScale = Vector3.one;
        animator.Rebind();
        animator.Update(0f);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float fill = (float)currentHP / maxHP;
            healthBar.DOFillAmount(fill, 0.3f);
        }
    }

    public void RotateCanvasToCamera()
    {
        if (canvasHP == null || Camera.main == null) return;
        canvasHP.transform.LookAt(Camera.main.transform);
        canvasHP.transform.Rotate(0, 180f, 0);
    }
}
