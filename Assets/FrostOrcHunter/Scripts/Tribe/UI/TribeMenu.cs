using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.GameRoot;
using UnityEngine;
using UnityEngine.InputSystem;
using Zeph1rrGameBase.Scripts.Core.DI;
using Zeph1rrGameBase.Scripts.Core.SaveLoadSystem;
using Zeph1rrGameBase.Scripts.Core.Utils;

namespace FrostOrcHunter.Scripts.Tribe.UI
{
    public class TribeMenu : MonoBehaviour
    {
        private PlayerPrefsSaveLoadSystem _saveLoadSystem;
        private GameData _gameData;

        public void Initialize(DIContainer container)
        {
            _saveLoadSystem = container.Resolve<PlayerPrefsSaveLoadSystem>();
            _gameData = container.Resolve<GameData>();
            Hide();
            Debug.Log("TribeMenu initialized");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void QuitToMainMenu()
        {
            Coroutines.StartRoutine(GameEntryPoint.Instance.LoadScene(Scenes.MAIN_MENU));
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SaveGame()
        {
            _saveLoadSystem.Save("hunter", _gameData);
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}