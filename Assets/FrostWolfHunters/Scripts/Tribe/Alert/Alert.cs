using System;
using TMPro;
using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _alert;
    public event EventHandler OnUnderstood;

    public void Initialize(string title, string alert)
    {
        _title.text = title;
        _alert.text = alert;
    }

    public void Understood()
    {
        OnUnderstood?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
