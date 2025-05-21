using System.Collections;
using DG.Tweening;
using GameMechanic;
using Project;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _characterModel;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private ParticleSystem _effectStan;
    [SerializeField] private Canvas _canvasHP;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Sword _sword;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private Rigidbody rb;

    [Header("Stats")]
    public float speed = 5f;
    public float jumpForce = 7f;
    public int maxHP = 100;

    [Header("Input")]
    public VariableJoystick variableJoystick;

    private Button _attackButton;
    private int _currentHP;
    private int _attackDamage = 10;

    private bool _isAttacking;
    private bool _isJumping;
    private bool _isTakingDamage;
    private bool _isGrounded = true;
    private bool _isGameOver;

    #region Initialization

    public void Init(Button attack, Button jump, VariableJoystick joystick)
    {
        _attackButton = attack;
        variableJoystick = joystick;

        LoadBonuses();
        _currentHP = maxHP;

        UpdateHealthBar();
        _canvasHP.worldCamera = Camera.main;

        attack.onClick.AddListener(Attack);
        jump.onClick.AddListener(Jump);
    }

    private void LoadBonuses()
    {
        maxHP += PlayerPrefs.GetInt("Heal", 0);
        _attackDamage += PlayerPrefs.GetInt("Attack", 0);
    }

    #endregion

    #region Update

    private void Update() => RotateHealthBar();

    private void FixedUpdate()
    {
        if (_isAttacking || _isJumping || _isGameOver || variableJoystick == null) return;

        Vector3 input = GetInputDirection();
        if (input.sqrMagnitude > 0.01f)
            TutorialManager.Instance.OnJoystickUsed();

        rb.AddForce(input * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        bool isMoving = input.sqrMagnitude > 0.01f;
        _animator.SetBool("IsRun", isMoving);

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input, Vector3.up);
            _characterModel.transform.rotation = Quaternion.Slerp(_characterModel.transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    private Vector3 GetInputDirection()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        if (direction == Vector3.zero)
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        return direction.normalized;
    }

    #endregion

    #region Combat

    public void Attack()
    {
        if (_isAttacking || _isGameOver) return;

        TutorialManager.Instance.OnAttackPressed();

        _attackButton.enabled = false;
        PlaySound(_attackSound);
        VibrateIfEnabled();

        _isAttacking = true;
        _animator.SetBool("IsRun", false);
        _animator.SetBool("IsAttack", true);

        _sword.Activate(_attackDamage);
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        _effect?.Play();

        _isAttacking = false;
        _sword.Deactivate();

        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("IsAttack", false);
        _attackButton.enabled = true;
    }

    #endregion

    #region Jump

    public void Jump()
    {
        if (_isJumping || _isGameOver || !_isGrounded) return;

        TutorialManager.Instance.OnJumpPressed();

        PlaySound(_jumpSound);
        VibrateIfEnabled();

        _isJumping = true;
        _isGrounded = false;
        _animator.SetBool("IsJump", true);

        Vector3 jumpDir = GetInputDirection();
        if (jumpDir == Vector3.zero)
            jumpDir = transform.forward;

        rb.AddForce((jumpDir + Vector3.up) * jumpForce, ForceMode.Impulse);
        StartCoroutine(ResetJump());
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1f);
        _animator.SetBool("IsJump", false);
        _isJumping = false;
    }

    #endregion

    #region Damage

    public void TakeDamage(int damage)
    {
        if (_isTakingDamage || _isGameOver) return;

        _isTakingDamage = true;
        _currentHP -= damage;

        PlaySound(_damageSound);
        UpdateHealthBar();

        _effectStan?.Play();

        if (_currentHP <= 0)
            Die();
        else
            StartCoroutine(ResetDamage());
    }

    private IEnumerator ResetDamage()
    {
        _animator.SetBool("IsDamage", true);
        yield return new WaitForSeconds(1f);
        _animator.SetBool("IsDamage", false);
        _isTakingDamage = false;
    }

    #endregion

    #region Death & Win

    private void Die()
    {
        _isGameOver = true;
        _canvasHP.transform.DOScale(Vector3.zero, 0.3f);
        _animator.SetBool("IsLose", true);
        MechanicManager.Instance.Lose();
        
        foreach (var enemy in FindObjectsOfType<EnemyController>())
            enemy.OnPlayerLose();
    }

    public void TriggerWin()
    {
        _animator.SetBool("IsWin", true);
        MechanicManager.Instance.Win();
    }

    #endregion

    #region Utils

    private void UpdateHealthBar()
    {
        if (_healthBar != null)
        {
            float targetFill = Mathf.Clamp01((float)_currentHP / maxHP);
            _healthBar.DOFillAmount(targetFill, 0.3f);
        }
    }

    private void RotateHealthBar()
    {
        if (_canvasHP != null && Camera.main != null)
        {
            _canvasHP.transform.LookAt(Camera.main.transform);
            _canvasHP.transform.Rotate(0, 180f, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            SoundManager.GetInstance().PlaySound(clip);
    }

    private void VibrateIfEnabled()
    {
        if (PlayerPrefs.GetInt("VibrationEnabled", 1) == 1)
            Handheld.Vibrate();
    }

    #endregion
}
