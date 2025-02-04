using UnityEngine;
using UnityEngine.EventSystems;

public interface IRightClickable
{
    public abstract void OnRightClick(PointerEventData eventData);
}
