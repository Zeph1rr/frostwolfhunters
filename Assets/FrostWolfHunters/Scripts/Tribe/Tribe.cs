using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FrostWolfHunters.Scripts.Game.Data;
using FrostWolfHunters.Scripts.Game.Data.Enums;

public class Tribe : MonoBehaviour, ISceneCompositeRoot
{
    [SerializeField] private GameObject _alertPrefab;
    [SerializeField] private GameObject _resourcePrefab;
    [SerializeField] private GameObject _tribeBuildingPrefab;
    [SerializeField] private Button _saveAndLeaveButton;
    [SerializeField] private Button _startHuntButton;
    
    private Alert _alert;
    private GameData _gameData;
    public void StartScene(GameData gameData)
    {
        _gameData = gameData;
        InitializeResources();
        if (_gameData.IsDead) SendAlert("Dead");
        if (_gameData.IsLeaved) SendAlert("Leave");
        _gameData.ResourceStorage.OnResourcesChanged += HandleResourcesChanged;
        _saveAndLeaveButton.onClick.AddListener(() => GameRoot.Instance.SaveAndLeaveToMainMenu());
        _startHuntButton.onClick.AddListener(() => GameRoot.Instance.ChangeScene("Gameplay"));
    }

    public void OpenTribeBuilding(string name)
    {
        TribeBuildingUI tribeBuilding = Instantiate(_tribeBuildingPrefab, transform). GetComponent<TribeBuildingUI>();
        tribeBuilding.Initialize(name, _gameData);
    }

    private void OnApplicationQuit()
    {
        GameRoot.Instance.SaveGame();
    }

    private void OnDestroy()
    {
        _gameData.ResourceStorage.OnResourcesChanged -= HandleResourcesChanged;
    }

    private void SendAlert(string key)
    {
        GameObject alertInstance = Instantiate(_alertPrefab, transform);
        _alert = alertInstance.GetComponent<Alert>();
        Dictionary<string, string> alert = AlertSystem.GetAlert(key);
        if (alert != null) _alert.Initialize(alert["title"], alert["alert"]);
        _alert.OnUnderstood += HandleOnUnderstood;
    }

    private void HandleOnUnderstood(object sender, EventArgs e)
    {
        _alert.OnUnderstood -= HandleOnUnderstood;
        _gameData.ResetPlayerConditions();
    }

    private void InitializeResources()
    {
        GridLayoutGroup resourcesContainer = GetComponentInChildren<GridLayoutGroup>();
        foreach (Transform child in resourcesContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (string resourceType in Enum.GetNames(typeof(ResourceType)))
        {
            GameObject newResource = Instantiate(_resourcePrefab, resourcesContainer.transform);
            newResource.name = resourceType;

            TextMeshProUGUI text = newResource.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = _gameData.ResourceStorage.GetResourceValueByName(resourceType).ToString();
            }

            Image image = newResource.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = Resources.Load<Sprite>($"Resources/{resourceType}");
            }
        }
    }

    private void HandleResourcesChanged(object sender, EventArgs e)
    {
        InitializeResources();
    }
}
