using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class VibrationToggleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _vibrationOnSprite;
        [SerializeField] private Sprite _vibrationOffSprite;
    
        private const string VibrationKey = "VibrationEnabled";
        private bool _isVibrationEnabled;

        private void Start()
        {
            _isVibrationEnabled = PlayerPrefs.GetInt(VibrationKey, 1) == 1;
            UpdateButtonState();
            _button.onClick.AddListener(ToggleVibration);
        }

        private void ToggleVibration()
        {
            _isVibrationEnabled = !_isVibrationEnabled;
            PlayerPrefs.SetInt(VibrationKey, _isVibrationEnabled ? 1 : 0);
            PlayerPrefs.Save();

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            _icon.sprite = _isVibrationEnabled ? _vibrationOnSprite : _vibrationOffSprite;
        }
    }

}