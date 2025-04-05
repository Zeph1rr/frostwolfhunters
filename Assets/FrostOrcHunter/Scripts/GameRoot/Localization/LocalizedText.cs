using TMPro;
using UnityEngine;

namespace FrostOrcHunter.Scripts.GameRoot.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;
        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            UpdateText();
        }

        private void OnEnable()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            if (_text != null)
            {
                _text.text = LocalizationSystem.Translate(_key);
            }
        }
    }
}
