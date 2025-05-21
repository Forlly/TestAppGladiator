using System.Collections.Generic;
using Leaderboard;
using TMPro;
using UnityEngine;

public class LeaderboardManager
{
    private const int MaxEntries = 10;
    private const string KeyPrefix = "Leaderboard_";

    private Transform _container;
    private GameObject _cellPrefab;
    private TMP_Text _emptyText;

    public LeaderboardManager() { }

    public void InjectUI(Transform container, GameObject cellPrefab, TMP_Text emptyText)
    {
        _container = container;
        _cellPrefab = cellPrefab;
        _emptyText = emptyText;
    }

    public void Display()
    {
        var scores = Load();

        if (scores.Count == 0)
        {
            _emptyText.gameObject.SetActive(true);
            return;
        }

        _emptyText.gameObject.SetActive(false);

        for (int i = 0; i < scores.Count; i++)
        {
            var cell = UnityEngine.Object.Instantiate(_cellPrefab, _container).GetComponent<LeaderboardCell>();
            cell.SetNum(i + 1);
            cell.SetTime(scores[i]);
        }
    }

    public void SaveScore(int score)
    {
        var scores = Load();
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a));
        if (scores.Count > MaxEntries) scores = scores.GetRange(0, MaxEntries);
        Save(scores);
    }

    private void Save(List<int> scores)
    {
        for (int i = 0; i < scores.Count; i++)
            PlayerPrefs.SetInt($"{KeyPrefix}{i}", scores[i]);
        PlayerPrefs.Save();
    }

    private List<int> Load()
    {
        var list = new List<int>();
        for (int i = 0; i < MaxEntries; i++)
        {
            if (!PlayerPrefs.HasKey($"{KeyPrefix}{i}")) continue;
            var val = PlayerPrefs.GetInt($"{KeyPrefix}{i}", -1);
            if (val >= 0) list.Add(val);
        }
        return list;
    }
    
}