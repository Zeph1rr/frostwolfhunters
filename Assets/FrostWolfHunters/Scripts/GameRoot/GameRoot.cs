using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameSettings _defaultSettings;
    [SerializeField] private GameObject _sceneRoot;

    private void Awake()
    {
        ISceeneRoot sceeneRoot = _sceneRoot.GetComponent<ISceeneRoot>();
        _gameSettings.Initialize(SaveLoadSystem.LoadSettings(_defaultSettings));
        LocalizationSystem.SetLanguage(_gameSettings.Language);
        Utils.SetResolution(_gameSettings.CurrentResolution);
        Utils.SetFullScreen(_gameSettings.IsFullscreen);
        sceeneRoot.StartScene();
    }
}
