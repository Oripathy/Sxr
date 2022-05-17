using UnityEngine;

[CreateAssetMenu(menuName = "DifficultyManager")]
internal class DifficultyManager : ScriptableObject
{
    [SerializeField] private int _currentDifficulty;
    public int CurrentDifficulty => _currentDifficulty;

    public void IncreaseDifficulty() => _currentDifficulty++;
    public void SetDifficulty(int value)
    {
        if (value > _currentDifficulty)
            _currentDifficulty = value;
    }
}
