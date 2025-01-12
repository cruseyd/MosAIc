using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] public StatName statName;
    protected Stat stat {get; private set; }

    void Update()
    {
        if (stat != null)
        {
            _text.text = stat.value.ToString();
        }
    }

    public void Initialize(int value)
    {
        _text.text = value.ToString();
    }

    public void Initialize(Stat stat)
    {
        this.stat = stat;
    }
}
