using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Views
{
    public class AnimateImage : MonoBehaviour
    {
        public enum AnimationType
        {
            None,
            Bounce,
            Rotation,
            MoveUpDown
        }
        
        [Header("Select Animation Type")]
        public AnimationType selectedAnimation = AnimationType.None;
        
        [Header("Bounce Settings")]
        public float bounceScale = 1.2f;
        public float bounceDuration = 0.5f;

        [Header("Rotation Settings")]
        public float rotationAngle = 360f;
        public float rotationDuration = 2f;

        [Header("Movement Settings")]
        public float moveDistance = 2f;
        public float moveDuration = 1f;

        private void Start()
        {
            switch (selectedAnimation)
            {
                case AnimationType.Bounce:
                    StartBounceAnimation();
                    break;
                case AnimationType.Rotation:
                    StartRotationAnimation();
                    break;
                case AnimationType.MoveUpDown:
                    StartMoveUpDownAnimation();
                    break;
                case AnimationType.None:
                    // Do nothing
                    break;
            }
        }

        private void StartBounceAnimation()
        {
            transform.DOScale(bounceScale, bounceDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }

        private void StartRotationAnimation()
        {
            transform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StartMoveUpDownAnimation()
        {
            transform.DOMoveY(transform.position.y + moveDistance, moveDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }
}