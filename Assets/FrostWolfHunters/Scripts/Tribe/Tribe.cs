using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tribe : MonoBehaviour, ISceeneRoot
{
    [SerializeField] private GameObject _alertPrefab;
    [SerializeField] private GameObject _resourcePrefab;
    private Alert _alert;
    private GameData _gameData;
    public void StartScene(GameData gameData)
    {
        _gameData = gameData;
        Debug.Log("Tribe");
        _gameData.ResourceStorage.PrintResources();
        InitializeResources();
        if (_gameData.IsDead) SendAlert("Dead");
        if (_gameData.IsLeaved) SendAlert("Leave");
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
        foreach (string resourceType in _gameData.ResourceStorage.Resources.Keys)
        {
            GameObject newResource = Instantiate(_resourcePrefab, resourcesContainer.transform);
            newResource.name = resourceType;

            TextMeshProUGUI text = newResource.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = _gameData.ResourceStorage.Resources[resourceType].ToString();
            }

            Image image = newResource.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = Resources.Load<Sprite>($"Resources/{resourceType}");
            }
        }
    }
}
