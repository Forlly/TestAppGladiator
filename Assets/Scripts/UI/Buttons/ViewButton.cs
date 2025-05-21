using System.Collections.Generic;
using GameProject.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    [RequireComponent(typeof(Button))]
    public class ViewButton : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _isShowButton = true;
        [SerializeField] private bool _isBackButton = false;

        [Header("View References")]
        [SerializeField] private ViewBase _view;
        [SerializeField] private List<ViewBase> _closedViews = new();

        [Header("Sound")]
        [SerializeField] private AudioClip _sound;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            if (_view != null)
            {
                if (_isShowButton)
                    _view.Show();
                else
                    _view.Hide();
            }

            HideAllViews();

            if (_isBackButton)
            {
                SoundManager.GetInstance().SwitchMusic();
                MechanicManager.Instance.ActivateCanvasBack();
                LevelManager.Instance.Clear();
            }

            PlaySound();
        }

        private void HideAllViews()
        {
            foreach (var view in _closedViews)
            {
                if (view != null)
                    view.Hide();
            }
        }

        private void PlaySound()
        {
            if (_sound != null)
                SoundManager.GetInstance().PlaySound(_sound);
        }

        public ViewBase GetView() => _view;
    }
}