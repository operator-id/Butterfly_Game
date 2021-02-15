using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    [SerializeField] private Material highlight;
    [SerializeField] List<ButterflyData> butterflies;

    public Material Highlight => highlight;

    // public Color ColorCell(int colorNumber)
    // {
    //     return colors[colorNumber];
    // }
    public Sprite GetButterflySprite(int index)
    {
        return butterflies[index].sprite;
    }
    public void SetXYText(Cell cell, int x, int y)
    {
        cell.GetComponentInChildren<Text>().text = $"X[{x}] Y[{y}]";
    }
}
