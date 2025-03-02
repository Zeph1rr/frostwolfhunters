using TMPro;
using UnityEngine;

public class TribeBuildingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;


    public void Initialize(string title)
    {
        _title.text = title;
    }
}
