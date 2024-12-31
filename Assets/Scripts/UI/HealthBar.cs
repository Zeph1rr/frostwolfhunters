using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        Player.Instance.OnHealthChanged += OnPlayerHealthChanged;
    }

    private void OnDisable() {
        Player.Instance.OnHealthChanged -= OnPlayerHealthChanged;
    }
    

    public void OnPlayerHealthChanged(object sender, HealthChangedArgs e) {
        _slider.maxValue = e.MaxHealth;
        _slider.value = e.CurrentHealth;
    }
}
