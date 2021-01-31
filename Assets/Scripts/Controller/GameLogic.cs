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

    [SerializeField] private float simulationSpeed = .2f;
    

    [SerializeField] private LineRenderer line;

    private List<int> _values = new List<int>();
    private List<List<Cell>> _listMap;
    private readonly Dictionary<Vector2Int, Cell> _map = new Dictionary<Vector2Int, Cell>();
    
    public GridView GridView => gridView;

    public float SimulationSpeed => simulationSpeed;


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

    private Cell _selectedCell;
    private SearchAlgorithms _searchAlgorithms;

    public void SelectCell(Cell cell)
    {

        //Debug.Log(cell.ID);
        if (_selectedCell == null)
        {
            _selectedCell = cell;
            _selectedCell.Highlight(true);
            //Debug.Log($"Selected x[{_selectedCell.Position.x}],y[{_selectedCell.Position.y}] ");
        }
        else
        {
            //Debug.Log($"Selected x[{_selectedCell.Position.x}],y[{_selectedCell.Position.y}] ");
            //Debug.Log($"Cell x[{cell.Position.x}],y[{cell.Position.y}] ");
            
            if (cell == _selectedCell || cell.ID != _selectedCell.ID)
            {
                _selectedCell.Highlight(false);
                _selectedCell = null;
                Debug.Log("<color=red>id not match or you selected the same cell</color>");
                return;
            }

            _selectedCell.Active = false;
            //StartCoroutine(SearchAlgorithms.DFSVisual(_map, _selectedCell, line, cell));
            StartCoroutine(SearchAlgorithms.CircularSearchVisual(_map, _selectedCell, line, cell));
            //bool found = SearchAlgorithms.DFS(_map, _selectedCell, cell);
            bool found = SearchAlgorithms.CircularSearch(_map, _selectedCell, cell);
            _selectedCell.Active = true;
            Debug.Log($"Search returned {found}");
            if (found)
            {
                ResetCells(cell);
            }
            _selectedCell = null;
        }
    }
    
    private void ResetCells(Cell cell)
    {
      
        cell.Active = false;
        _selectedCell.Active = false;
        _selectedCell.Highlight(false);
        _selectedCell.GetComponent<Image>().color = new Color(1, 1, 1);
        cell.GetComponent<Image>().color = new Color(1, 1, 1);
        _selectedCell = null;
    }

    void Start()
    {
        _searchAlgorithms = new SearchAlgorithms();
        Randomize();
        Shuffle();
        Map();
//        Debug.Log(_map.Count);
        InitializeCells();    
    }



    private void InitializeCells()
    {
        var index = 0;
        for (var row = 0; row < _listMap.Count; row++)
        {

            for (var column = 0; column < _listMap[0].Count; column++)
            {
                var current = _listMap[row][column];
                current.ID = _values[index];
                current.Position = new Vector2Int(row, column);
                current.Setup(gridView.ColorCell(current.ID));
                Cell cell = cells[index];
               
                gridView.SetXYText(cell, row, column);
                _map.Add(current.Position, current);
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
       _listMap = new List<List<Cell>>();
        var index = 0;

        for(var row = 0; row < 7; row++)
        {
            var col = new List<Cell>();
            for (var column = 0; column < 20; column++)
            {
                col.Add(cells[index]);
                index++;
                
            }
            _listMap.Add(col);

        }
    }

}
