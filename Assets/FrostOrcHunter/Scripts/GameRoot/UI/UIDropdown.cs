using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace FrostOrcHunter.Scripts.GameRoot.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class UIDropdown : MonoBehaviour
    {
        public void Initialize(List<string> options, string currentOption, UnityAction<int> onValueChanged)
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.SetValueWithoutNotify(options.IndexOf(currentOption));
            dropdown.RefreshShownValue();
            dropdown.onValueChanged.AddListener(onValueChanged);
        }
    }
}