using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameProject.Levels
{
    public class LevelsButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Image _frame;
        [SerializeField] private GameObject _stars;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _index;
        [SerializeField] private Image _icon;

        [Header("Runtime")]
        [SerializeField] public bool IsLastUnlockedLevel;

        private void Start()
        {
            if (_index != null) _index.text = "";

            if (IsLastUnlockedLevel)
                _levelData = LevelManager.Instance.GetCurrentUnlockedLevelData();

            UpdateLevelButton();

            _button.onClick.AddListener(LoadLevel);
            LevelManager.Instance.OnBackToMenu += UpdateLevelButton;
        }

        private void UpdateLevelButton()
        {
            int unlockedLevel = LevelManager.Instance.GetCurrentUnlockedLevel();

            if (IsLastUnlockedLevel)
            {
                _levelData = LevelManager.Instance.GetCurrentUnlockedLevelData();
                return;
            }

            if (_levelData == null) return;

            bool isUnlocked = _levelData.LevelIndex <= unlockedLevel;
            bool isCurrent = _levelData.LevelIndex == unlockedLevel;

            if (_levelData.IsEndless)
            {
                bool available = unlockedLevel >= 1;
                ApplyState(available, _levelData.Frame, _levelData.DeactiveFrame, showStars: available);
                return;
            }

            if (isUnlocked)
            {
                var frame = isCurrent ? _levelData.CurrentFrame : _levelData.Frame;
                ApplyState(true, frame, null, showStars: !isCurrent);
            }
            else
            {
                ApplyState(false, _levelData.DeactiveFrame, null, showStars: false);
            }

            if (_index != null)
                _index.text = (_levelData.LevelIndex + 1).ToString();
        }

        private void ApplyState(bool interactable, Sprite frameSprite, Sprite fallbackIcon, bool showStars)
        {
            _button.enabled = interactable;

            if (_frame != null)
                _frame.sprite = frameSprite;

            if (_icon != null && frameSprite != null)
                _icon.sprite = frameSprite;
            else if (_icon != null && fallbackIcon != null)
                _icon.sprite = fallbackIcon;

            if (_stars != null)
                _stars.SetActive(showStars);
        }

        private void LoadLevel()
        {
            if (_levelData == null) return;

            int unlocked = LevelManager.Instance.GetCurrentUnlockedLevel();

            if (_levelData.IsEndless || _levelData.LevelIndex <= unlocked)
            {
                LevelManager.Instance.LoadLevel(_levelData.LevelIndex);
            }
        }

        private void OnDestroy()
        {
            if (LevelManager.Instance != null)
                LevelManager.Instance.OnBackToMenu -= UpdateLevelButton;
        }
    }
}
