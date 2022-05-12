using System;
using UnityEngine;

[CreateAssetMenu(menuName = "DifficultyManager")]
internal class DifficultyManager : ScriptableObject
{
    [SerializeField] private int _currentDifficulty;
    public int CurrentDifficulty => _currentDifficulty;

    public int IncreaseDifficulty() => _currentDifficulty++;
}
