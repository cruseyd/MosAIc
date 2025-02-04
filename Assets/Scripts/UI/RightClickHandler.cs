using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler
{
    private IRightClickable _component;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _component?.OnRightClick(eventData);
        }
    }

    void Start()
    {
        _component = GetComponent<IRightClickable>();
    }
}
