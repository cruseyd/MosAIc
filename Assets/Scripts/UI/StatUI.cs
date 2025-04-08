using UnityEngine;
using TMPro;
using UnityEngine.UIElements.Experimental;

public class StatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] public StatName statName;
    public int statValue
    {
        set {
            _text.text = value.ToString();
        }
    }
}
