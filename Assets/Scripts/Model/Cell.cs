using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public int ID { get; set; }
    public Vector2 Position { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameLogic.Instance.SelectCell(this);
    }
}
