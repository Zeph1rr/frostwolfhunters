using UnityEngine;

namespace FrostWolfHunters.Scripts.Game.GameRoot
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Transform _uiSceneContainer;

        private void Awake()
        {
            HideLoadingScreen();
        }

        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            clearSceneUI();
            
            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void clearSceneUI()
        {
            var childCount = _uiSceneContainer.childCount;
            for (var i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}