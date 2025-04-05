using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrostOrcHunter.Scripts.GameRoot.UI.Resource
{
    public class ResourceView: MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;

        public void Initialize(Data.Resource.Resource resource)
        {
            _image.sprite = Resources.Load<Sprite>("Images/Resource/" + resource.Name);
            _text.text = resource.Value.ToString();
        }

        public void SetTextColor(Color color)
        {
            _text.color = color;
        }
    }
}