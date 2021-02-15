using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private int shuffleCount = 10;
    [Space]
    [SerializeField] private int distinctButterflies = 7;
    [SerializeField] private int cellNumber = 140;
    [SerializeField] private GridView gridView;
    [Space]
    [SerializeField] private List<Cell> cells;
    [SerializeField] private float simulationSpeed = .2f;
    
    [Space] 
    [SerializeField] private SearchType howToSearch;

    [Header("Optional for visuals")]
    [SerializeField] private bool showVisuals;
    [SerializeField] private LineRenderer line;
    
    
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }

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

    private void Start()
    {
        Randomize();
        for (int i = 0; i < shuffleCount; i++)
        {
            Shuffle();
        }
        
        Map();
        InitializeCells();
        GridWidth = _listMap[0].Count;
        GridHeight = _listMap.Count;

    }
    
    public void SelectCell(Cell cell)
    {
        if (_selectedCell == null)
        {
            _selectedCell = cell;
            _selectedCell.Highlight(true);

        }
        else
        {
            if (cell == _selectedCell || cell.ID != _selectedCell.ID)
            {
                _selectedCell.Highlight(false);
                _selectedCell = null;
                Debug.Log("<color=red>id not match or you selected the same cell</color>");
                return;
            }

            _selectedCell.Active = false;
            bool found = false;
            switch (howToSearch)
            {
                case SearchType.Circular:
                {
                    found = SearchAlgorithms.CircularSearch(_map, _selectedCell, cell);
                    
                    if(!showVisuals) break;
                    StartCoroutine(SearchAlgorithms.CircularSearchVisual(_map, _selectedCell, line, cell));
                    break;
                }
                case SearchType.DFS:
                {
                    found = SearchAlgorithms.DFS(_map, _selectedCell, cell);
                    
                    if(!showVisuals) break;
                    StartCoroutine(SearchAlgorithms.DFSVisual(_map, _selectedCell, line, cell));
                    break;
                    
                }
            }
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
        cell.OnMatch();
        _selectedCell.OnMatch();
        _selectedCell = null;
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
                current.Setup(gridView.GetButterflySprite(current.ID));
                
                // Cell cell = cells[index];
                //gridView.SetXYText(cell, row, column);
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
