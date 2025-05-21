using System;
using UnityEngine;

public class CoinManager
{
    public Action OnCoinChanged;

    public void Add(int amount)
    {
        var coins = PlayerPrefs.GetInt("Coins", 0) + amount;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        OnCoinChanged?.Invoke();
    }

    public void Subtract(int amount)
    {
        var coins = PlayerPrefs.GetInt("Coins", 0) - amount;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        OnCoinChanged?.Invoke();
    }

    public int Get() => PlayerPrefs.GetInt("Coins", 0);
}