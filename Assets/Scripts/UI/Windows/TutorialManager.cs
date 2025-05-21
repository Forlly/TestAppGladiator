using System.Collections;
using DG.Tweening;
using Project;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI References")]
    [SerializeField] private CanvasGroup[] tutorialPanels; 
    [SerializeField] private Button attackButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private ViewUI _view;

    private bool _joystickUsed;
    private bool _attackUsed;
    private bool _jumpUsed;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTutor()
    {
        _view.Show();
        ResetFlags();
        StartCoroutine(RunTutorialSequence());
    }

    public void Close()
    {
        _view.InstantHide();
    }

    private IEnumerator RunTutorialSequence()
    {
        yield return ShowStep(0, 2f);     // Step 1: Intro
        yield return WaitForJoystick();   // Step 2: Joystick
        yield return WaitForButton(2, attackButton, () => _attackUsed); // Step 3: Attack
        yield return WaitForButton(3, jumpButton, () => _jumpUsed);     // Step 4: Jump
    }

    private void ResetFlags()
    {
        _joystickUsed = false;
        _attackUsed = false;
        _jumpUsed = false;
    }

    private IEnumerator ShowStep(int index, float delay = 0f)
    {
        yield return ShowPanel(index, delay);
        HidePanel(index);
    }

    private IEnumerator WaitForJoystick()
    {
        yield return ShowPanel(1);
        yield return new WaitUntil(() => _joystickUsed);
        HidePanel(1);
    }

    private IEnumerator WaitForButton(int panelIndex, Button button, System.Func<bool> condition)
    {
        yield return ShowPanel(panelIndex);
        BounceButton(button);
        yield return new WaitUntil(condition);
        StopBounce(button);
        HidePanel(panelIndex);
    }

    private IEnumerator ShowPanel(int index, float delay = 0)
    {
        if (!IsValidPanel(index)) yield break;

        yield return new WaitForSeconds(delay);

        var panel = tutorialPanels[index];
        panel.alpha = 0;
        panel.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad);
        panel.blocksRaycasts = true;
    }

    private void HidePanel(int index)
    {
        if (!IsValidPanel(index)) return;

        var panel = tutorialPanels[index];
        panel.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad);
        panel.blocksRaycasts = false;
    }

    private void BounceButton(Button button)
    {
        if (button != null)
            button.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopBounce(Button button)
    {
        if (button == null) return;

        button.transform.DOKill();
        button.transform.localScale = Vector3.one;
    }

    private bool IsValidPanel(int index)
    {
        return tutorialPanels != null && index >= 0 && index < tutorialPanels.Length;
    }

    #region Event Listeners

    public void OnJoystickUsed()
    {
        if (!_joystickUsed)
        {
            _joystickUsed = true;
        }
    }

    public void OnAttackPressed()
    {
        if (!_attackUsed)
        {
            _attackUsed = true;
        }
    }

    public void OnJumpPressed()
    {
        if (!_jumpUsed)
        {
            _jumpUsed = true;
        }
    }

    #endregion
}
