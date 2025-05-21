using UnityEngine;

namespace Project
{
    public class GameManagerBase : MonoBehaviour
    {
        private void Awake()
        {
            OnInitialized();
        }

        public virtual void OnInitialized()
        {
            
        }
    }
}