using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    [SerializeField] List<Color> colors;

    public void ColorCell(int colorNumber, Image image)
    {
       image.color = colors[colorNumber];
    }
    public void SetXYText(Cell cell, int x, int y)
    {
        cell.GetComponentInChildren<Text>().text = $"X[{x}] Y[{y}]";
    }
}
