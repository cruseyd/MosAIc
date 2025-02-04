using UnityEngine;
using UnityEngine.EventSystems;

public interface IDoubleClickable
{
    public abstract void OnDoubleClick(PointerEventData eventData);
}
