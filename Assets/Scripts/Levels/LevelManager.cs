using System;
using System.Collections.Generic;
using Cinemachine;
using Project;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameProject.Levels
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public event Action OnBackToMenu;
        public event Action OnLevelReload;
        public event Action OnLevelLoad;

        [Header("UI & Camera")]
        [SerializeField] private TMP_Text _enemyCounterText;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private CinemachineVirtualCamera _camera;
        public VariableJoystick variableJoystick;

        [Header("Levels")]
        [SerializeField] private List<LevelData> _levels;
        [SerializeField] private Transform _boardTransform;
        [SerializeField] private int _currentLevel;
        private int _currentUnlockedLevel;

        [Header("Runtime")]
        [SerializeField] private LevelBoard _currentBoard;

        [Header("Debug")]
        [SerializeField] private GameObject _enemiesPanel;

        private const string CurrentLevelKey = "CurrentLevel";
        private const string CurrentUnlockedLevelKey = "CurrentUnlockedLevel";

        private int _enemyKillCount;

        public int BeginnerBetSum { get; private set; }
        public (int, int) ExperiencedBetValues { get; private set; }

        private void Awake()
        {
            Instance = this;
            LoadCurrentLevelIndex();
        }

        private void Start()
        {
            LoadCurrentLevelIndex();
        }

        public TMP_Text EnemyCounterText => _enemyCounterText;

        public int GetWaveCount() => _enemyKillCount;

        public void IncreaseEnemies() => _enemyKillCount++;

        public void ReloadCurrentLevel() => CreateBoard(GetCurrentLevel());

        public void Clear()
        {
            Debug.Log($"Clear called, _currentBoard = {_currentBoard}");

            DestroyBoard();
            OnBackToMenu?.Invoke();
            MechanicManager.Instance.ActivateCanvasBack();
        }

        public LevelBoard GetBoard() => _currentBoard;

        public int GetCurrentUnlockedLevel()
        {
            LoadCurrentLevelIndex();
            return _currentUnlockedLevel;
        }

        public LevelData GetCurrentUnlockedLevelData()
        {
            LoadCurrentLevelIndex();
            return _levels[_currentUnlockedLevel];
        }

        public LevelData GetCurrentLevel() => _levels[_currentLevel % _levels.Count];

        public void GoToNextLevel()
        {
            if (_currentLevel < _levels.Count - 1)
            {
                _currentLevel++;
                _currentUnlockedLevel = Mathf.Max(_currentUnlockedLevel, _currentLevel);
                PlayerPrefs.SetInt(CurrentUnlockedLevelKey, _currentUnlockedLevel);
                SaveCurrentLevel();
            }

            CreateBoard(GetCurrentLevel());
        }

        public void LoadLevel(int levelIndex)
        {
            if (levelIndex >= _levels.Count)
            {
                Debug.LogWarning($"Invalid level index: {levelIndex}");
                return;
            }

            _currentLevel = levelIndex;

            if (_currentLevel == 10)
                TutorialManager.Instance.StartTutor();
            else
                TutorialManager.Instance.Close();

            CreateBoard(_levels[levelIndex]);

            if (_levels[levelIndex].IsEndless)
                _currentBoard.EnableEndlessMode();

            MechanicManager.Instance.ActivateCanvasLevel();
        }

        private void CreateBoard(LevelData data)
        {
            DestroyBoard();

            _enemyKillCount = 0;

            _currentBoard = Instantiate(data.Board, _boardTransform);
            _currentBoard.player.Init(_attackButton, _jumpButton, variableJoystick);

            _camera.Follow = _currentBoard.player.transform;
            _camera.LookAt = _currentBoard.player.transform;
        }
        public void UpdateEnemyCounter(int value)
        {
            if (_enemyCounterText != null)
                _enemyCounterText.text = value.ToString();
        }

        private void DestroyBoard()
        {
            if (_currentBoard != null)
            {
                Destroy(_currentBoard.gameObject);
                _currentBoard = null;
            }
        }

        private void LoadCurrentLevelIndex()
        {
            _currentUnlockedLevel = PlayerPrefs.GetInt(CurrentUnlockedLevelKey, 0);
        }

        private void SaveCurrentLevel()
        {
            PlayerPrefs.SetInt(CurrentLevelKey, _currentLevel);
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            SaveCurrentLevel();
        }
    }
}
