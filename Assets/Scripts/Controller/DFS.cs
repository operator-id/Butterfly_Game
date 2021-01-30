using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DFS 
{
    public static IEnumerator SearchVisual(Dictionary<Vector2Int, Cell> map, Cell start, LineRenderer line, Cell searchedCell)
    {
        var visited = new HashSet<Cell>();

        var stack = new Stack<Cell>();

        stack.Push(start);

        //start.Active = false;
        int index = 0;
        line.positionCount = 0;
        while (stack.Count > 0)
        {
            var cell = stack.Pop(); // start

            if (visited.Contains(cell))
            {
                continue;
            }

            visited.Add(cell);

            yield return new WaitForSeconds(GameLogic.Instance.SimulationSpeed);

            cell.Visit(false);
            line.positionCount++;
            line.SetPosition(index, cell.transform.position);
            index++;
            if (CheckNeighbours(map, cell, searchedCell, true)) yield break;
            
            var x = cell.Position.x;
            var y = cell.Position.y;
            var up = new Vector2Int(x - 1, y);
            var down = new Vector2Int(x + 1, y);
            var left = new Vector2Int(x, y - 1);
            var right = new Vector2Int(x, y + 1);

            Cell empty;
            if (map.ContainsKey(up) && !map[up].Active)
            {
                empty = map[up];
                if (!visited.Contains(empty)) stack.Push(empty);
            }

            if (map.ContainsKey(down) && !map[down].Active)
            {
                empty = map[down];
                if (!visited.Contains(empty)) stack.Push(empty);
            }

            if (map.ContainsKey(left) && !map[left].Active)
            {
                empty = map[left];
                if (!visited.Contains(empty)) stack.Push(empty);
            }

            if (map.ContainsKey(right) && !map[right].Active)
            {
                empty = map[right];
                if (!visited.Contains(empty)) stack.Push(empty);
            }
        }

    }
    
        public static bool Search(Dictionary<Vector2Int, Cell> map, Cell start, Cell searchedCell)
        {
            var visited = new HashSet<Cell>();
    
            var stack = new Stack<Cell>();
    
            stack.Push(start);

            while (stack.Count > 0)
            {
                var cell = stack.Pop(); // start
    
                if (visited.Contains(cell))
                {
                    continue;
                }
                if (CheckNeighbours(map, cell, searchedCell)) return true;
                visited.Add(cell);

                var x = cell.Position.x;
                var y = cell.Position.y;
                var up = new Vector2Int(x - 1, y);
                var down = new Vector2Int(x + 1, y);
                var left = new Vector2Int(x, y - 1);
                var right = new Vector2Int(x, y + 1);
    
                Cell search;
                if (map.ContainsKey(up) && !map[up].Active)
                {
                    search = map[up];
                    
                    if (!visited.Contains(search)) stack.Push(search);
                }
    
                if (map.ContainsKey(down) && !map[down].Active)
                {
                    search = map[down];
                    if (!visited.Contains(search)) stack.Push(search);
                }
    
                if (map.ContainsKey(left) && !map[left].Active)
                {
                    search = map[left];
                    if (!visited.Contains(search)) stack.Push(search);
                }
    
                if (map.ContainsKey(right) && !map[right].Active)
                {
                    search = map[right];
                    if (!visited.Contains(search)) stack.Push(search);
                }
            }

            return false;
        }

        private static bool CheckNeighbours(Dictionary<Vector2Int, Cell> map, Cell cell, Cell searchedCell, bool highlight = false)
        {
            var x = cell.Position.x;
            var y = cell.Position.y;
            var up = new Vector2Int(x - 1, y);
            var down = new Vector2Int(x + 1, y);
            var left = new Vector2Int(x, y - 1);
            var right = new Vector2Int(x, y + 1);

            var coordinates = new List<Vector2Int> {up, down, left, right};

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
        
}
