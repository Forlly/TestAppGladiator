using GameProject.Levels;
using Project;
using UnityEngine;
using UnityEngine.UI;

namespace GameProject.Scripts.Views
{
    public class LoaderButton : MonoBehaviour
    {
        [SerializeField] private bool _isReloadMechanic;
        [SerializeField] private bool _isNextButton;
        [SerializeField] private LevelData _levelData;
        [SerializeField] private AudioClip _audioClip;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            SoundManager.GetInstance().PlaySound(_audioClip);
            if (_isNextButton)
            {
                LevelManager.Instance.GoToNextLevel();
                return;
            }
            LoadScene();
        }

        private  void LoadScene()
        {
            Debug.Log($"LoadScene");
        
            if (_isReloadMechanic)
            {
                LevelManager.Instance.ReloadCurrentLevel();
            }
        }
    }
}