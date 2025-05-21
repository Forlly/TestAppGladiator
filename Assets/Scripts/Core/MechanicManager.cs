using System;
using DG.Tweening;
using GameProject.Levels;
using Project;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MechanicManager : GameManagerBase
{
    [SerializeField] private ViewUI _loseView;
    [SerializeField] private ViewUI _winView;
    [SerializeField] private CanvasGroup _menuCanvas;
    [SerializeField] private CanvasGroup _gameCanvas;
    [SerializeField] private AudioClip _loseSound;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private Button _leaderboardButton;
    
    [Header("LeaderboardUI")]
    [SerializeField] private Transform leaderboardContainer;
    [SerializeField] private GameObject leaderboardCellPrefab;
    [SerializeField] private TMP_Text leaderboardEmptyText;
    
    public static MechanicManager Instance;

    public Action OnGameWin;
    public Action OnCoinChanged;

    private LeaderboardManager _leaderboard;
    private CoinManager _coins;

    public override void OnInitialized()
    {
        Instance = this;
        Application.targetFrameRate = 60;

        _leaderboard = new LeaderboardManager();
        _leaderboard.InjectUI(leaderboardContainer, leaderboardCellPrefab, leaderboardEmptyText);
        _coins = new CoinManager();
        _coins.OnCoinChanged += () => OnCoinChanged?.Invoke();

        _leaderboardButton.onClick.AddListener(() => _leaderboard.Display());
        _leaderboard.Display();
    }

    public void IncreaseCoins(int amount)
    {
        _coins.Add(amount);
    }

    public void SubtractCoins(int amount)
    {
        _coins.Subtract(amount);
    }

    public void Win()
    {
        _winView.Show();
        IncreaseCoins(50);
        SoundManager.GetInstance().PlaySound(_winSound);
        OnGameWin?.Invoke();
    }

    public void Lose()
    {
        _loseView.Show();
        _leaderboard.SaveScore(Mathf.FloorToInt(LevelManager.Instance.GetWaveCount()));
        SoundManager.GetInstance().PlaySound(_loseSound);
    }

    public void ActivateCanvasLevel()
    {
        _menuCanvas.DOFade(0, 0);
        _menuCanvas.blocksRaycasts = false;

        _gameCanvas.DOFade(1, 0.3f);
        _gameCanvas.blocksRaycasts = true;

        SoundManager.GetInstance().SwitchMusic();
    }

    public void ActivateCanvasBack()
    {
        _menuCanvas.DOFade(1, 0);
        _menuCanvas.blocksRaycasts = true;

        _gameCanvas.DOFade(0, 0.3f);
        _gameCanvas.blocksRaycasts = false;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
