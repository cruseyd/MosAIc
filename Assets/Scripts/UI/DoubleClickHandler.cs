using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float threshold = 0.3f;
    private float _lastClick= -1.0f;
    private IDoubleClickable component;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - _lastClick < threshold)
        {
            component?.OnDoubleClick(eventData);
            _lastClick = -1.0f;
        } else {
            _lastClick = Time.time;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        component = GetComponent<IDoubleClickable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
