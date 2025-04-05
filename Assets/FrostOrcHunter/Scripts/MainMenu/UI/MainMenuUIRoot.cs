using System;
using FrostOrcHunter.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Zeph1rrGameBase.Scripts.Core.DI;

namespace FrostOrcHunter.Scripts.MainMenu.UI
{
    public class MainMenuUIRoot : MonoBehaviour
    {
        [SerializeField] private Settings _settings;
        [SerializeField] private Menu _menu;
        
        private DIContainer _container;
        private InputActions _inputActions;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _inputActions = container.Resolve<InputActions>();
            _inputActions.Enable();
            _inputActions.Global.Escape.performed += ReturnToMenu;
            _menu.Initialize(_container);
            _settings.Initialize(_container);
            ReturnToMenu();
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
            _inputActions.Global.Escape.performed -= ReturnToMenu;
        }

        private void ReturnToMenu(InputAction.CallbackContext obj)
        {
            ReturnToMenu();
        }

        public void ReturnToMenu()
        {
            _menu.gameObject.SetActive(false);
            _menu.gameObject.SetActive(true);
            _settings.gameObject.SetActive(false);
            _menu.ReturnToMainMenu();
        }

        public void OpenSettings()
        {
            _settings.gameObject.SetActive(true);
        }
    }
}