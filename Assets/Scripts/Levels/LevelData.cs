using UnityEngine;

namespace GameProject.Levels
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        public int LevelIndex = 0;
        public Sprite Frame;
        public Sprite DeactiveFrame;
        public Sprite CurrentFrame;
        public bool IsEndless;
        public LevelBoard Board;
    }
}