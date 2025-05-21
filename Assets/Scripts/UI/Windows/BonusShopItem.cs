using Project;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusShopItem : MonoBehaviour
{
    [Header("Bonus Config")]
    [SerializeField] private string bonusKey;
    [SerializeField] private int price = 10;
    [SerializeField] private int amountToAdd = 1;

    [Header("UI Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    [Header("Feedback Views")]
    [SerializeField] private ViewUI purchaseView;
    [SerializeField] private ViewUI insufficientFundsView;

    private void Start()
    {
        InitUI();
        buyButton.onClick.AddListener(TryPurchase);
    }

    private void InitUI()
    {
        if (priceText != null)
            priceText.text = price.ToString();

        if (icon != null && iconSprite != null)
            icon.sprite = iconSprite;
    }

    private void TryPurchase()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);

        if (currentCoins >= price)
        {
            ProcessPurchase();
        }
        else
        {
            insufficientFundsView?.Show();
        }
    }

    private void ProcessPurchase()
    {
        MechanicManager.Instance.SubtractCoins(price);

        int currentAmount = PlayerPrefs.GetInt(bonusKey, 0);
        PlayerPrefs.SetInt(bonusKey, currentAmount + amountToAdd);
        PlayerPrefs.Save();

        purchaseView?.Show();
    }
}