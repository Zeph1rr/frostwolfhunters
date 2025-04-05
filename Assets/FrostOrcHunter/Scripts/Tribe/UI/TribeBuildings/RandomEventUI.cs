using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrostOrcHunter.Scripts.Tribe.UI.TribeBuildings
{
    public class RandomEventUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Button _button;
        
        public void Initialize(string title, string description)
        {
            _title.text = title;
            _description.text = description;
            _button.onClick.AddListener(() => Destroy(gameObject));
        }
    }
}