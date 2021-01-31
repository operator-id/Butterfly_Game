using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SearchAlgorithms
{
    private const int GridWidth = 20;
    private const int GridHeight = 7;
    public static IEnumerator DFSVisual(Dictionary<Vector2Int, Cell> map, Cell start, LineRenderer line, Cell searchedCell)
    {
        var visited = new HashSet<Cell>();

        var stack = new Stack<Cell>();

        stack.Push(start);

        //start.Active = false;
        int index = 0;
        line.positionCount = 0;
        bool checkedBorders = false;
        while (stack.Count > 0)
        {
            var cell = stack.Pop(); // start

            if (visited.Contains(cell))
            {
                continue;
            }

            visited.Add(cell);

            yield return new WaitForSeconds(GameLogic.Instance.SimulationSpeed);

            if (!checkedBorders && CellHasExitToBorders(map, cell))
            {
              CheckIfInactiveCellAtBorders(map, searchedCell, stack, null) ;
              checkedBorders = true;
            }
            cell.Visit(false);
            line.positionCount++;
            line.SetPosition(index, cell.transform.position);
            index++;
            if (FoundMatchingCell(map, cell, searchedCell, true)) yield break;
            
            
            CheckInactiveNeighbours(map, cell, visited, stack, null);
        }

    }
    
        public static bool DFS(Dictionary<Vector2Int, Cell> map, Cell start, Cell searchedCell)
        {
            var visited = new HashSet<Cell>();
            var stack = new Stack<Cell>();
    
            stack.Push(start);
            bool checkedBorders = false;
            while (stack.Count > 0)
            {
                var cell = stack.Pop(); // start
    
                if (visited.Contains(cell))
                {
                    continue;
                }
                if (!checkedBorders && CellHasExitToBorders(map, cell))
                {
                    if (CheckIfInactiveCellAtBorders(map, searchedCell, stack, null)) return true;
                    checkedBorders = true;
                }
                if (FoundMatchingCell(map, cell, searchedCell)) return true;
                visited.Add(cell);
                
                CheckInactiveNeighbours(map, cell, visited, stack, null);
            }

            return false;
        }
        
        public static IEnumerator CircularSearchVisual(Dictionary<Vector2Int, Cell> map, Cell start, LineRenderer line, Cell searchedCell)
        {
            var visited = new HashSet<Cell>();

            var queue = new Queue<Cell>();

            queue.Enqueue(start);

            //start.Active = false;
            int index = 0;
            line.positionCount = 0;
            bool checkedBorders = false;
            while (queue.Count > 0)
            {
                var cell = queue.Dequeue(); // start

                if (visited.Contains(cell))
                {
                    continue;
                }

                visited.Add(cell);

                yield return new WaitForSeconds(GameLogic.Instance.SimulationSpeed);

                if (!checkedBorders && CellHasExitToBorders(map, cell))
                {
                    CheckIfInactiveCellAtBorders(map, searchedCell, null, queue);
                    checkedBorders = true;
                }
                cell.Visit(false);
                line.positionCount++;
                line.SetPosition(index, cell.transform.position);
                index++;
                if (FoundMatchingCell(map, cell, searchedCell, true)) yield break;
            
                CheckInactiveNeighbours(map, cell, visited, null, queue);
            }
        }
        
        public static bool CircularSearch(Dictionary<Vector2Int, Cell> map, Cell start, Cell searchedCell)
        {
            var visited = new HashSet<Cell>();
            var queue = new Queue<Cell>();
    
            queue.Enqueue(start);

            bool checkedBorders = false;
            while (queue.Count > 0)
            {
               
                var cell = queue.Dequeue(); // start
    
                if (visited.Contains(cell))
                {
                    continue;
                }

                if (!checkedBorders && CellHasExitToBorders(map, cell))
                {
                    if (CheckIfInactiveCellAtBorders(map, searchedCell, null, queue)) return true;
                    checkedBorders = true;
                }
                
                if (FoundMatchingCell(map, cell, searchedCell)) return true;
                visited.Add(cell);
                
                CheckInactiveNeighbours(map, cell, visited, null, queue);
            }

            return false;
        }


        private static bool FoundMatchingCell(Dictionary<Vector2Int, Cell> map, Cell cell, Cell searchedCell, bool highlight = false)
        {
            var coordinates = ExtractPosition2Coordinates(cell);

            foreach (var coordinate in coordinates)
            {
                if (!map.ContainsKey(coordinate)) continue;
                if (highlight)
                {
                   map[coordinate].Visit(true);
                }

                if (map[coordinate] == searchedCell) return true;

                
            }

            return false;
        }
        
        private static void CheckInactiveNeighbours(Dictionary<Vector2Int, Cell> map,
            Cell cell, ICollection<Cell> visitedCollection,[CanBeNull] Stack<Cell> stack, [CanBeNull] Queue<Cell> queue)
        {
            var coordinates = ExtractPosition2Coordinates(cell);

            foreach (var coordinate in coordinates)
            {
                if (!map.ContainsKey(coordinate)) continue;

                var currentCell = map[coordinate];
                if(currentCell.Active) continue;

                if (visitedCollection.Contains(currentCell)) continue;

                stack?.Push(currentCell);
                queue?.Enqueue(currentCell);
            }
        }

        private static List<Vector2Int> ExtractPosition2Coordinates(Cell cell)
        {
            var x = cell.Position.x;
            var y = cell.Position.y;
            var up = new Vector2Int(x - 1, y);
            var down = new Vector2Int(x + 1, y);
            var left = new Vector2Int(x, y - 1);
            var right = new Vector2Int(x, y + 1);

            var coordinates = new List<Vector2Int> {up, down, left, right};
            return coordinates;
        }

        private static bool CheckIfInactiveCellAtBorders(Dictionary<Vector2Int, Cell> map, Cell searchedCell, [CanBeNull] Stack<Cell> stack, [CanBeNull] Queue<Cell> queue)
        {
            

            bool FoundBorderCell(int index, int edge,  bool vertical = false)
            {
                var row = vertical ? new Vector2Int( index , edge) : new Vector2Int(edge, index);
                if (map[row] == searchedCell)
                {
                    return true;
                }

                if (!map[row].Active)
                {

                    stack?.Push(map[row]);
                    queue?.Enqueue(map[row]);
                }

                return false;
            }
            
            int currentEdge = 0;
            for (int i = 0; i < GridWidth; i++)
            {
                if (FoundBorderCell(i, currentEdge)) return true;
            }

            currentEdge = GridHeight - 1;
            
            for (int i = 0; i < GridWidth; i++)
            {
                if (FoundBorderCell(i, currentEdge)) return true;
            }

            currentEdge = 0;
            for (int i = 0; i < GridHeight; i++)
            {
                if (FoundBorderCell(i, currentEdge, true)) return true;
            }

            currentEdge = GridWidth - 1;
            
            for (int i = 0; i < GridHeight; i++)
            {
                if (FoundBorderCell(i, currentEdge, true)) return true;
            }

            return false;
        }

        private static bool CellHasExitToBorders(Dictionary<Vector2Int, Cell> map, Cell cell)
        {
            var coordinates = ExtractPosition2Coordinates(cell);

            foreach (var coordinate in coordinates)
            {
                if (!map.ContainsKey(coordinate)) return true;
            }

            return false;
        }
}
