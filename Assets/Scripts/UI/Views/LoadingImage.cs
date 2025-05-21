using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LoadingImage : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _loadImage;
        [SerializeField] private LoadTypeImg typeImg;
        [SerializeField] private float _time;
        
        private void Awake()
        {
            if (typeImg == LoadTypeImg.Rotate)
            {
                StartCoroutine(RotateLoadRoutine());
            }
            else
            {
                StartCoroutine(LoadRoutine());
            }
        }
        private IEnumerator RotateLoadRoutine()
        {
            float elapsed = 0;  
            while (true)
            {
                _loadImage.transform.Rotate(0, 0, (-360f * Time.deltaTime) * (1 / _time));
                elapsed += Time.deltaTime;
                
                if (elapsed >= _time)
                {
                    _canvasGroup.DOFade(0, 0.4f).OnComplete(() =>
                    {
                        _canvasGroup.DOFade(0, 0.3f).OnComplete(() => _canvasGroup.blocksRaycasts = false);
                        StopAllCoroutines();
                    });
                }
                yield return null;
            }
        }
        private IEnumerator LoadRoutine()
        {
            float currentTime = 0f;
            while (currentTime < _time)
            {
                currentTime += Time.deltaTime;
                _loadImage.fillAmount = Mathf.Lerp(0f, 1f, currentTime / _time);
                yield return null;
            }
        
            _loadImage.fillAmount = 1f;
            _canvasGroup.DOFade(0, 0.3f).OnComplete(() => _canvasGroup.blocksRaycasts = false);
        }
    }

    public enum LoadTypeImg
    {
        Rotate,
        Field
    }
}