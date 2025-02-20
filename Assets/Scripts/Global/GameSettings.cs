using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Game/Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private int _currentResolutionIndex;
    [SerializeField] private bool _isFullscreen;
    [SerializeField][Range(0,1)] private float _volume;

    public int CurrentResolutionIndex => _currentResolutionIndex;
    public bool IsFullscreen => _isFullscreen;
    public float Volume => _volume;

    public void Initialize(int currentResolutionIndex, bool isFullscreen, float volume)
    {
        _currentResolutionIndex = currentResolutionIndex;
        _isFullscreen = isFullscreen;
        _volume = volume;
    }

    public void SetVolume(float value)
    {
        if (value < 0 || value > 1) throw new ArgumentOutOfRangeException();
        _volume = value;
    }

    public void SetResolution(int resolutionIndex) {
        _currentResolutionIndex = resolutionIndex;
    }

    public void setFullscreen(bool fullscreen)
    {
        _isFullscreen = fullscreen;
    }
}
