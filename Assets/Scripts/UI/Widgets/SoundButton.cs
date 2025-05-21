using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickSound;
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            SoundManager.GetInstance().PlaySound(_clickSound);
        } 
    }
}