using UnityEngine;

namespace FrostOrcHunter.Scripts.GameRoot.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Transform _uiSceneContainer;

        public void AttachSceneUI(GameObject uiScene)
        {
            ClearSceneUI();

            var canvas = GetComponentInChildren<Canvas>();
            canvas.worldCamera = Camera.main;
            
            uiScene.transform.SetParent(_uiSceneContainer, false);
        }
        
        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }
        
        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }
            
        private void Awake()
        {
            HideLoadingScreen();
        }

        public void ClearSceneUI()
        {
            var childCount = _uiSceneContainer.childCount;
            for (var i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}