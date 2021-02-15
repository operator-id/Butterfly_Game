using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image childImage;
    [SerializeField] private Image parentImage;

    public Image ChildImage => childImage;

    public int ID { get; set; }
    public bool Active { get; set; } = true;

    public Vector2Int Position { get; set; }
    private Material _defaultMaterial;
    private Color _defaultColor;

    private void Start()
    {
        _defaultMaterial = GetComponent<Image>().material;
        _defaultColor = parentImage.color;
    }

    
    public void Setup(Sprite sprite)
    {
        childImage.sprite = sprite;
        //parentImage.color = color;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!Active) return;
        GameLogic.Instance.SelectCell(this);
    }

    public void OnMatch()
    {
        Highlight(false);
        Active = false;
        parentImage.color = Color.white;
        childImage.enabled = false;
    }

    public void Highlight(bool isActive)
    {
        GetComponent<Image>().material = isActive ? GameLogic.Instance.GridView.Highlight : _defaultMaterial;
    }

    public void Visit(bool justChecking)
    {
        StartCoroutine(VisitCoroutine(justChecking));
    }

    private IEnumerator VisitCoroutine(bool justChecking)
    {
        parentImage.color = justChecking ? new Color(1f, .9f, .1f, 1f) : new Color(0, 0, 0);
        //GetComponent<Image>().color = new Color(0, 0, 0);
        yield return new WaitForSeconds(GameLogic.Instance.SimulationSpeed);
        
        parentImage.color = Active ? _defaultColor : new Color(1, 1, 1);
    }
}
