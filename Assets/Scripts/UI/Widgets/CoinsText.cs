using TMPro;
using UnityEngine;
using Project;

namespace View
{
    [RequireComponent(typeof(TMP_Text))]
    public class CoinsText : MonoBehaviour
    {
        [SerializeField] private bool useAbbreviatedFormat = false;

        private TMP_Text _text;

        private const string CoinsKey = "Coins";

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            Refresh();
        }

        private void OnEnable()
        {
            if (MechanicManager.Instance != null)
                MechanicManager.Instance.OnCoinChanged += Refresh;
        }

        private void OnDisable()
        {
            if (MechanicManager.Instance != null)
                MechanicManager.Instance.OnCoinChanged -= Refresh;
        }

        public void Refresh()
        {
            int coins = PlayerPrefs.GetInt(CoinsKey, 0);
            _text.text = useAbbreviatedFormat ? FormatCoins(coins) : coins.ToString();
        }

        private string FormatCoins(int amount)
        {
            if (amount >= 10_000)
                return (amount / 1000f).ToString("0.#") + "K";

            return amount.ToString();
        }
    }
}