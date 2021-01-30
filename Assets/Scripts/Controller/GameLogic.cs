using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private int distinctButterflies = 7;
    [SerializeField] private int cellNumber = 140;
    [SerializeField] private GridView gridView;
    [Space]
    [SerializeField] List<Cell> cells;

    private List<int> _values = new List<int>();
    private List<List<Cell>> _map;

    private static GameLogic _instance;
    public static GameLogic Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameLogic>();
            }
            return _instance;
        }
    }

    private Cell selected;

    public void SelectCell(Cell cell)
    {
        Debug.Log(cell.ID);
        if(selected == null)
        {
            selected = cell;
        }
        else
        {
            Debug.Log($"Selcted x[{selected.Position.x}],y[{selected.Position.y}] ");
            Debug.Log($"Cell x[{cell.Position.x}],y[{cell.Position.y}] ");
            if (cell == selected || cell.ID != selected.ID)
            {
                Debug.LogError("id not match or you selected the same cell");
                selected = null;
                return;
            }
            if ((cell.Position.x == selected.Position.x - 1) && (cell.Position.y == selected.Position.y)//control stanga
                || (cell.Position.x == selected.Position.x + 1) && (cell.Position.y == selected.Position.y)//control dreapta
                || (cell.Position.x == selected.Position.x) && (cell.Position.y == selected.Position.y - 1)//control jos
                || (cell.Position.x == selected.Position.x) && (cell.Position.y == selected.Position.y + 1))//control sus
            {
                Destroy(cell.GetComponent<Image>());
                Destroy(selected.GetComponent<Image>());
                selected = null;
                return;
            }
            else
            {
                Debug.LogError("cells not in range");
                selected = null;
                return;
            }

        }
        
    }

    void Start()
    {
        Randomize();
        Shuffle();
        Map();
        Debug.Log(_map.Count);
        InitializeCells();    
    }



    private void InitializeCells()
    {
        var index = 0;
        for (var row = 0; row < _map.Count; row++)
        {

            for (var column = 0; column < _map[0].Count; column++)
            {
                var current = _map[row][column];
                current.ID = _values[index];
                current.Position = new Vector2(row, column);
                gridView.ColorCell(current.ID, cells[index].GetComponent<Image>());
                Cell cell = cells[index];
               
                gridView.SetXYText(cell, row, column);
                index++;
            }

        }
    }

    private void Randomize()
    {
        for(int i = 0; i < cellNumber / 2; i++)
        {
            var random = Random.Range(0, 7);
            _values.Add(random);
            _values.Add(random);
        }
    }

    private void Shuffle()
    {
        int count = _values.Count;
        for (var i = 0; i < count; i++)
        {
            var randomIndex = Random.Range(0, count);
            var temp = _values[i];
            _values[i] = _values[randomIndex];
            _values[randomIndex] = temp;
        }
    }

    private void Map()
    {
       _map = new List<List<Cell>>();
        var index = 0;

        for(var row = 0; row < 7; row++)
        {
            var col = new List<Cell>();
            for (var column = 0; column < 20; column++)
            {
                
                col.Add(cells[index]);
                index++;
            }
            _map.Add(col);

        }
    }
}
