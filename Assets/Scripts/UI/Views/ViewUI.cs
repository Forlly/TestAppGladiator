using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class ViewUI : ViewBase
    {
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private GameObject _gameObject;
        public override void Awake()
        {
            base.Awake();
            
            if (IsShowOnStart())
            {
                InstantShow();
            }
            else
            {
                if (_gameObject != null)
                {
                    _gameObject.SetActive(false);
                }
                
                InstantHide();
            }
        }
        
        public override void Show(bool withBackground = false)
        {
            if (_gameObject != null)
            {
                _gameObject.SetActive(true);
            }
            
            base.Show();
            
            if (_scroll != null)
            {
                _scroll.movementType = ScrollRect.MovementType.Elastic;
            }
        }

        public override void Hide()
        {
            base.Hide();
            if (_scroll != null)
            {
                _scroll.movementType = ScrollRect.MovementType.Unrestricted;
            }
            
            if (_gameObject != null)
            {
                _gameObject.SetActive(false);
            }
        }
    }
}