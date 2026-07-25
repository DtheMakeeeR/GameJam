using System.Collections.Generic;

public class Pathfinding
{
    public static List<Cell> FindPathBFS(MazeGrid grid, Cell startCell, Cell targetCell)
    {
        Queue<Cell> queue = new Queue<Cell>();
        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

        queue.Enqueue(startCell);
        cameFrom[startCell] = null;

        while (queue.Count > 0)
        {
            Cell current = queue.Dequeue();

            if (current == targetCell)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (Cell neighbor in grid.GetNeighbours(current))
            {
                if (CanMove(current, neighbor) && !cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return null;
    }

    private static bool CanMove(Cell current, Cell neighbor)
    {
        int dx = neighbor.Coordinates.X - current.Coordinates.X;
        int dy = neighbor.Coordinates.Y - current.Coordinates.Y;

        if (dy == -1 && dx == 0) return current.Walls[Constants.UP] == 0;
        if (dx == 1 && dy == 0) return current.Walls[Constants.RIGHT] == 0;
        if (dy == 1 && dx == 0) return current.Walls[Constants.DOWN] == 0;
        if (dx == -1 && dy == 0) return current.Walls[Constants.LEFT] == 0;

        return false;
    }

    private static List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell current)
    {
        List<Cell> path = new List<Cell>();
        while (current != null)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
}