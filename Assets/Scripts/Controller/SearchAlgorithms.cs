using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SearchAlgorithms 
{
    public static IEnumerator DFSVisual(Dictionary<Vector2Int, Cell> map, Cell start, LineRenderer line, Cell searchedCell)
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
            if (CheckActiveNeighbours(map, cell, searchedCell, true)) yield break;
            
            CheckInactiveNeighbours(map, cell, visited, stack, null);
        }

    }
    
        public static bool DFS(Dictionary<Vector2Int, Cell> map, Cell start, Cell searchedCell)
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
                if (CheckActiveNeighbours(map, cell, searchedCell)) return true;
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
            while (queue.Count > 0)
            {
                var cell = queue.Dequeue(); // start

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
                if (CheckActiveNeighbours(map, cell, searchedCell, true)) yield break;
            
                CheckInactiveNeighbours(map, cell, visited, null, queue);
            }
        }
        
        public static bool CircularSearch(Dictionary<Vector2Int, Cell> map, Cell start, Cell searchedCell)
        {
            var visited = new HashSet<Cell>();
            var queue = new Queue<Cell>();
    
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var cell = queue.Dequeue(); // start
    
                if (visited.Contains(cell))
                {
                    continue;
                }
                if (CheckActiveNeighbours(map, cell, searchedCell)) return true;
                visited.Add(cell);
                
                CheckInactiveNeighbours(map, cell, visited, null, queue);
            }

            return false;
        }


        private static bool CheckActiveNeighbours(Dictionary<Vector2Int, Cell> map, Cell cell, Cell searchedCell, bool highlight = false)
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
            Cell cell, ICollection<Cell> structureToCompareVisited,[CanBeNull] Stack<Cell> stack, [CanBeNull] Queue<Cell> queue)
        {
            var coordinates = ExtractPosition2Coordinates(cell);

            foreach (var coordinate in coordinates)
            {
                if (!map.ContainsKey(coordinate)) continue;

                var currentCell = map[coordinate];
                if(currentCell.Active) continue;

                if (structureToCompareVisited.Contains(currentCell)) continue;

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
}
