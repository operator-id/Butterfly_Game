using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    [SerializeField] private Material highlight;
    [SerializeField] List<Color> colors;

    public Material Highlight => highlight;

    public Color ColorCell(int colorNumber)
    {
        return colors[colorNumber];
    }
    public void SetXYText(Cell cell, int x, int y)
    {
        cell.GetComponentInChildren<Text>().text = $"X[{x}] Y[{y}]";
    }
}
