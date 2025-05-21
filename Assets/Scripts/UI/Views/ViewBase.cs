using DG.Tweening;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(RectTransform))]
    public class ViewBase : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private AnimationType _animationType = AnimationType.Default;
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _showOnStart = false;

        private RectTransform _rectTransform;

        public virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_showOnStart)
                Show();
            else
                InstantHide();
        }

        public virtual void Show(bool withBackground = false)
        {
            switch (_animationType)
            {
                case AnimationType.SlideFromLeft:
                    SlideFrom(-Screen.width, Axis.X);
                    break;
                case AnimationType.SlideFromBottom:
                    SlideFrom(-Screen.height, Axis.Y);
                    break;
                case AnimationType.SlideFromTop:
                    SlideFrom(Screen.height, Axis.Y);
                    break;
                case AnimationType.WithFade:
                    ShowWithFade();
                    break;
                case AnimationType.Default:
                default:
                    DefaultShow();
                    break;
            }
        }


        public virtual void Hide()
        {
            switch (_animationType)
            {
                case AnimationType.SlideFromLeft:
                    SlideTo(-Screen.width, Axis.X);
                    break;
                case AnimationType.SlideFromBottom:
                    SlideTo(-Screen.height, Axis.Y);
                    break;
                case AnimationType.SlideFromTop:
                    SlideTo(Screen.height, Axis.Y);
                    break;
                case AnimationType.WithFade:
                    HideWithFade();
                    break;
                case AnimationType.Default:
                default:
                    DefaultHide();
                    break;
            }
        }

        public void InstantShow()
        {
            _rectTransform.localScale = Vector3.one;
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
            }
        }

        public void InstantHide()
        {
            if (_animationType == AnimationType.WithFade)
                InstantHideFade();
            else
                _rectTransform.localScale = Vector3.zero;
        }

        private void DefaultShow()
        {
            _rectTransform.DOScale(Vector3.one, _fadeDuration).SetEase(Ease.OutBack);
        }

        private void DefaultHide()
        {
            _rectTransform.DOScale(Vector3.zero, _fadeDuration).SetEase(Ease.InBack);
        }

        private void ShowWithFade()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(1f, _fadeDuration);
            _canvasGroup.blocksRaycasts = true;
        }

        private void HideWithFade()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(0f, _fadeDuration);
        }

        private void InstantHideFade()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        private enum Axis { X, Y }

        private void SlideFrom(float offset, Axis axis)
        {
            _rectTransform.localScale = Vector3.one;
            var pos = _rectTransform.localPosition;

            if (axis == Axis.X)
                _rectTransform.localPosition = new Vector3(offset, pos.y, pos.z);
            else
                _rectTransform.localPosition = new Vector3(pos.x, offset, pos.z);

            _rectTransform.DOLocalMove(Vector3.zero, _fadeDuration).SetEase(Ease.OutCubic);
        }

        private void SlideTo(float offset, Axis axis)
        {
            var pos = _rectTransform.localPosition;
            Vector3 target;

            if (axis == Axis.X)
                target = new Vector3(offset, pos.y, pos.z);
            else
                target = new Vector3(pos.x, offset, pos.z);

            _rectTransform.DOLocalMove(target, _fadeDuration).SetEase(Ease.InCubic)
                .OnComplete(() => _rectTransform.localScale = Vector3.zero);
        }

        public bool IsShowOnStart() => _showOnStart;
    }

    public enum AnimationType
    {
        Default,
        SlideFromLeft,
        WithFade,
        SlideFromBottom,
        SlideFromTop
    }
}
