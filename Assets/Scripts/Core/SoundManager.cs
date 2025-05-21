using UnityEngine;

namespace Project
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _firstAudioSource;
        [SerializeField] private AudioSource _secondAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _backgroundMusicAudioSource;

        private static SoundManager s_instance;
        public static SoundManager GetInstance() => s_instance;

        private const string MusicPrefKey = "MusicIsActive";
        private const string SoundPrefKey = "SoundsIsActive";

        private void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
            ApplyMusicSettings();
            ApplySoundSettings();
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip == null || !IsSoundEnabled()) return;

            if (!_firstAudioSource.isPlaying)
            {
                _firstAudioSource.clip = clip;
                _firstAudioSource.Play();
            }
            else if (!_secondAudioSource.isPlaying)
            {
                _secondAudioSource.clip = clip;
                _secondAudioSource.Play();
            }
            else
            {
                _firstAudioSource.Stop();
                _firstAudioSource.clip = clip;
                _firstAudioSource.Play();
            }
        }

        public void SwitchMusic()
        {
            if (!IsMusicEnabled()) return;

            if (_musicAudioSource.isPlaying)
            {
                _musicAudioSource.Pause();
                _backgroundMusicAudioSource.Play();
            }
            else
            {
                _backgroundMusicAudioSource.Pause();
                _musicAudioSource.Play();
            }
        }

        public void ChangeSoundsState()
        {
            bool enabled = !IsSoundEnabled();
            PlayerPrefs.SetInt(SoundPrefKey, enabled ? 1 : 0);
            ApplySoundSettings();
        }

        public void ChangeMusicState(bool? forceState = null)
        {
            bool targetState = forceState ?? !IsMusicEnabled();
            PlayerPrefs.SetInt(MusicPrefKey, targetState ? 1 : 0);
            ApplyMusicSettings();
        }

        private void ApplySoundSettings()
        {
            bool enabled = IsSoundEnabled();
            _firstAudioSource.enabled = enabled;
            _secondAudioSource.enabled = enabled;
        }

        private void ApplyMusicSettings()
        {
            bool enabled = IsMusicEnabled();

            if (enabled)
                _backgroundMusicAudioSource.Play();
            else
                _backgroundMusicAudioSource.Pause();
        }

        private bool IsSoundEnabled() => PlayerPrefs.GetInt(SoundPrefKey, 1) == 1;
        private bool IsMusicEnabled() => PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
    }
}
