using Zeph1rr.Core.Resources;
using TMPro;
using UnityEngine;
using Zeph1rr.Core.Stats;
using UnityEngine.UI;
using System;

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
        try
        {
            float nextValue = _stat.GetNextValue();
            _resourceName.text = $"{LocalizationSystem.Translate(_stat.Name)} ({_stat.Value} -> {_stat.GetNextValue()})";
        } catch (ArgumentOutOfRangeException e) {
            Debug.Log(e);
            _resourceName.text = $"{LocalizationSystem.Translate(_stat.Name)} ({_stat.Value} MAX)";
            _resourceName.color = Color.red;
            _button.interactable = false;
        }
        _upgradeName.text = LocalizationSystem.Translate($"{_stat.Name}_upgrade");
        

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
        _stat.Upgrade();
        _resourceStorage.SpendResource(_resourceType.ToString(), _stat.GetNextValueCost());
    }
}
