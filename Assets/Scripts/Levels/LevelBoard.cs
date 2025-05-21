using System.Collections.Generic;
using UnityEngine;

namespace GameProject.Levels
{
    public class LevelBoard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] public CharacterController player;
        [SerializeField] private List<EnemySpawner> _spawners;

        private int _enemiesRemaining;
        private int _killCount;
        private bool _isEndlessMode;
        private bool _isWinTriggered;

        private void Start()
        {
            InitializeCounters();
            UpdateCounter();
        }

        private void InitializeCounters()
        {
            _isWinTriggered = false;
            _enemiesRemaining = 0;

            foreach (var spawner in _spawners)
            {
                if (!spawner.IsInfinite)
                    _enemiesRemaining += spawner.TotalSpawnCount;
            }
        }

        public void EnableEndlessMode()
        {
            _isEndlessMode = true;
            _killCount = 0;
            UpdateCounter();
        }

        public void EnemyDefeated()
        {
            if (_isEndlessMode)
            {
                _killCount++;
                UpdateCounter();
                return;
            }

            if (_isWinTriggered || _enemiesRemaining <= 0)
                return;

            _enemiesRemaining--;
            UpdateCounter();

            if (_enemiesRemaining <= 0)
                TriggerWin();
        }

        private void UpdateCounter()
        {
            int displayValue = _isEndlessMode ? _killCount : _enemiesRemaining;
            LevelManager.Instance.UpdateEnemyCounter(displayValue);
        }

        private void TriggerWin()
        {
            _isWinTriggered = true;
            player?.TriggerWin();
        }
    }
}