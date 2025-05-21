using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    [RequireComponent(typeof(Button))]
    public class VolumeButton : MonoBehaviour
    {
        [SerializeField] private bool _isMusicButton = false;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _inactiveSprite;

        private Button _button;

        private const string MusicPrefKey = "MusicIsActive";
        private const string SoundPrefKey = "SoundsIsActive";

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            UpdateIcon();
        }

        private void OnClick()
        {
            if (_isMusicButton)
                SoundManager.GetInstance().ChangeMusicState();
            else
                SoundManager.GetInstance().ChangeSoundsState();

            UpdateIcon();
        }

        private void UpdateIcon()
        {
            if (_iconImage == null) return;

            bool isActive = PlayerPrefs.GetInt(_isMusicButton ? MusicPrefKey : SoundPrefKey, 1) == 1;
            _iconImage.sprite = isActive ? _activeSprite : _inactiveSprite;
        }
    }
}