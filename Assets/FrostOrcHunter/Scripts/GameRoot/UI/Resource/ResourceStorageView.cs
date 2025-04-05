using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.Data.Resource;
using UnityEngine;

namespace FrostOrcHunter.Scripts.GameRoot.UI.Resource
{
    public class ResourceStorageView : MonoBehaviour
    {
        [SerializeField] private ResourceView _resourceViewPrefab;

        private ResourceStorage _resourceStorage;
        public void Initialize(ResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
            _resourceStorage.OnResourcesChanged += DrawResources;
            DrawResources();
        }

        private void OnDestroy()
        {
            _resourceStorage.OnResourcesChanged -= DrawResources;
        }

        private void DrawResources()
        {
            ClearResources();
            foreach (var resource in _resourceStorage.Resources)
            {
                var resourceView = Instantiate(_resourceViewPrefab, transform);
                resourceView.Initialize(resource);
            }
        }

        private void ClearResources()
        {
            var childCount = transform.childCount;
            for (var i = childCount - 1; i >= 0; i--) 
            {
                var child = transform.GetChild(i); 
                Destroy(child.gameObject); 
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                _resourceStorage.AddResource(ResourceType.Fur.ToString(), 2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                _resourceStorage.AddResource(ResourceType.Eat.ToString(), 2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                _resourceStorage.AddResource(ResourceType.Wood.ToString(), 2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                _resourceStorage.AddResource(ResourceType.Bones.ToString(), 2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                _resourceStorage.AddResource(ResourceType.Stone.ToString(), 2);
            }
        }
#endif
    }
}