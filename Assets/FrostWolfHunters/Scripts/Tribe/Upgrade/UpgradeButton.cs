using Zeph1rr.Core.Resources;
using TMPro;
using UnityEngine;
using Zeph1rr.Core.Stats;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _upgradeName;
    [SerializeField] private TextMeshProUGUI _resourceName;
    [SerializeField] private GameObject _resource;
    private Button _button;
    private Stat _stat;
    private ResourceType _resourceType;
    private ResourceStorage _resourceStorage;
    public void Initialize(Stat stat, ResourceType resourceType, ResourceStorage resourceStorage)
    {
        _resourceType = resourceType;
        _resourceStorage = resourceStorage;
        _button = GetComponent<Button>();
        _stat = stat;
        _upgradeName.text = LocalizationSystem.Translate($"{_stat.Name}_upgrade");
        _resourceName.text = $"{LocalizationSystem.Translate(_stat.Name)} ({_stat.Value} -> {_stat.GetNextValue()})";

        TextMeshProUGUI resourceText = _resource.GetComponentInChildren<TextMeshProUGUI>();
        resourceText.text = _stat.GetNextValueCost().ToString();

        Image resourceImage = _resource.GetComponentInChildren<Image>();
        resourceImage.sprite = Resources.Load<Sprite>($"Resources/{resourceType}");
        
        _button.onClick.AddListener(() => Upgrade());

        if (_resourceStorage.GetResourceValueByName(_resourceType.ToString()) < _stat.GetNextValueCost())
        {
            _button.interactable = false;
            resourceText.color = Color.red;
        }
    }

    public void Upgrade()
    {
        _resourceStorage.SpendResource(_resourceType.ToString(), _stat.GetNextValueCost());
        _stat.Upgrade();
        Initialize(_stat, _resourceType, _resourceStorage);
    }
}
