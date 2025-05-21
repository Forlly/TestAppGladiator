using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text _num;
        [SerializeField] private TMP_Text _time;

        public void SetNum(int num)
        {
            _num.text = $"#{num.ToString()}";
        }
        public void SetTime(float num)
        {
            _time.text = $"{num/100} points";
        }
    }
}