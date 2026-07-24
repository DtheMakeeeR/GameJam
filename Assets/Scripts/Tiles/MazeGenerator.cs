using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

// Константы направлений
public static class Constants
{
    public const int UP = 0;
    public const int RIGHT = 1;
    public const int DOWN = 2;
    public const int LEFT = 3;
}

public class Cell
{
    public (int X, int Y) Coordinates { get; set; }
    public int[] Walls { get; set; }

    /// <summary>
    /// Одна ячейка лабиринта
    /// coordinates - кортеж координат x и y ячейки
    /// walls - массив из 4 элементов, содержащий 0 или 1. Элемент массива показывает наличие или отсутствие стены в направлении.
    /// Схема расположений стен: [^, >, v, <]
    /// </summary>
    public Cell((int X, int Y) coordinates, int[] walls)
    {
        Coordinates = coordinates;
        Walls = walls;
    }

    // Эквивалент Python метода __iadd__ для числа
    public Cell Add(int direction)
    {
        Walls[direction] = 1;
        return this;
    }

    // Эквивалент Python метода __iadd__ для другой ячейки
    public Cell Add(Cell other)
    {
        int x1 = Coordinates.X, y1 = Coordinates.Y;
        int x2 = other.Coordinates.X, y2 = other.Coordinates.Y;

        if (Math.Abs(x2 - x1) == 1 && Math.Abs(y2 - y1) == 0)
        {
            if (x1 > x2)
            {
                Walls[Constants.LEFT] = 1;
                other.Walls[Constants.RIGHT] = 1;
            }
            else
            {
                Walls[Constants.RIGHT] = 1;
                other.Walls[Constants.LEFT] = 1;
            }
        }
        else if (Math.Abs(y2 - y1) == 1 && Math.Abs(x2 - x1) == 0)
        {
            if (y1 > y2)
            {
                Walls[Constants.UP] = 1;
                other.Walls[Constants.DOWN] = 1;
            }
            else
            {
                Walls[Constants.DOWN] = 1;
                other.Walls[Constants.UP] = 1;
            }
        }

        return this;
    }

    // Эквивалент Python метода __isub__ для числа
    public Cell Sub(int direction)
    {
        Walls[direction] = 0;
        return this;
    }

    // Эквивалент Python метода __isub__ для другой ячейки
    public Cell Sub(Cell other)
    {
        int x1 = Coordinates.X, y1 = Coordinates.Y;
        int x2 = other.Coordinates.X, y2 = other.Coordinates.Y;

        if (Math.Abs(x2 - x1) == 1 && Math.Abs(y2 - y1) == 0)
        {
            if (x1 > x2)
            {
                Walls[Constants.LEFT] = 0;
                other.Walls[Constants.RIGHT] = 0;
            }
            else
            {
                Walls[Constants.RIGHT] = 0;
                other.Walls[Constants.LEFT] = 0;
            }
        }
        else if (Math.Abs(y2 - y1) == 1 && Math.Abs(x2 - x1) == 0)
        {
            if (y1 > y2)
            {
                Walls[Constants.UP] = 0;
                other.Walls[Constants.DOWN] = 0;
            }
            else
            {
                Walls[Constants.DOWN] = 0;
                other.Walls[Constants.UP] = 0;
            }
        }

        return this;
    }
}

public class MazeGrid
{
    public Cell[][] Maze { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public MazeGrid(int width, int height)
    {
        Width = width;
        Height = height;
        ClearMaze();
    }

    public void ClearMaze()
    {
        Maze = new Cell[Height][];
        for (int y = 0; y < Height; y++)
        {
            Maze[y] = new Cell[Width];
            for (int x = 0; x < Width; x++)
            {
                Maze[y][x] = new Cell((x, y), new int[] { 0, 0, 0, 0 });
            }
        }
    }

    public void FullMaze()
    {
        Maze = new Cell[Height][];
        for (int y = 0; y < Height; y++)
        {
            Maze[y] = new Cell[Width];
            for (int x = 0; x < Width; x++)
            {
                Maze[y][x] = new Cell((x, y), new int[] { 1, 1, 1, 1 });
            }
        }
    }

    public void BuildBorders()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (y == 0)
                    Maze[y][x].Add(Constants.UP);
                if (y == Height - 1)
                    Maze[y][x].Add(Constants.DOWN);

                if (x == 0)
                    Maze[y][x].Add(Constants.LEFT);
                if (x == Width - 1)
                    Maze[y][x].Add(Constants.RIGHT);
            }
        }
    }

    public List<Cell> GetNeighbours(Cell cell)
    {
        int xCell = cell.Coordinates.X;
        int yCell = cell.Coordinates.Y;
        var neighbours = new List<Cell>();

        var directions = new (int dy, int dx)[]
        {
            (-1, 0), (0, 1), (0, -1), (1, 0)
        };

        foreach (var (dy, dx) in directions)
        {
            int newY = yCell + dy;
            int newX = xCell + dx;

            if (newY >= 0 && newY < Height && newX >= 0 && newX < Width)
            {
                neighbours.Add(Maze[newY][newX]);
            }
        }

        return neighbours;
    }

    public void PrintMaze()
    {
        Console.WriteLine(" " + string.Concat(Enumerable.Repeat("__ ", Width)));

        for (int y = 0; y < Height; y++)
        {
            string line = "|";

            for (int x = 0; x < Width; x++)
            {
                Cell cell = Maze[y][x];

                if (cell.Walls[Constants.DOWN] == 1)
                    line += "__";
                else
                    line += "  ";

                if (cell.Walls[Constants.RIGHT] == 1)
                    line += "|";
                else
                    line += " ";
            }

            Console.WriteLine(line);
        }
    }
}

public abstract class MazeGenerator
{
    protected int RandomState;
    protected Random Random;

    public MazeGenerator(int? randomState = null)
    {
        if (randomState == null)
        {
            Random tempRandom = new Random();
            randomState = tempRandom.Next(1_000_000);
        }

        RandomState = randomState.Value;
        Random = new Random(RandomState);
    }

    public abstract void Generate(MazeGrid maze);

    public void SetRandomState(int randomState)
    {
        RandomState = randomState;
        Random = new Random(randomState);
    }
}

public class EllerGenerator : MazeGenerator
{
    public EllerGenerator(int? randomState = null) : base(randomState) { }

    public override void Generate(MazeGrid maze)
    {
        maze.ClearMaze();
        maze.BuildBorders();

        List<int> setOfCells = Enumerable.Range(0, maze.Width).ToList();
        int i = maze.Width;

        for (int y = 0; y < maze.Height; y++)
        {
            if (y != 0)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    if (maze.Maze[y - 1][x].Walls[Constants.DOWN] == 1)
                    {
                        setOfCells[x] = i;
                        i++;
                    }
                }
            }

            for (int x = 0; x < maze.Width - 1; x++)
            {
                if (setOfCells[x] == setOfCells[x + 1])
                {
                    maze.Maze[y][x].Add(Constants.RIGHT);
                }
                else
                {
                    if (Random.Next(2) == 0) // True или False
                    {
                        maze.Maze[y][x].Add(Constants.RIGHT);
                    }
                    else
                    {
                        int oldSet = setOfCells[x + 1];
                        int newSet = setOfCells[x];

                        for (int j = 0; j < setOfCells.Count; j++)
                        {
                            if (setOfCells[j] == oldSet)
                            {
                                setOfCells[j] = newSet;
                            }
                        }
                    }
                }
            }

            var sets = new Dictionary<int, List<int>>();

            for (int x = 0; x < maze.Width; x++)
            {
                int cellSet = setOfCells[x];
                if (!sets.ContainsKey(cellSet))
                {
                    sets[cellSet] = new List<int>();
                }
                sets[cellSet].Add(x);
            }

            foreach (var cellSet in sets.Keys)
            {
                var indices = sets[cellSet];
                int openCell = indices[Random.Next(indices.Count)];

                foreach (int x in indices)
                {
                    if (x == openCell)
                        continue;
                        
                    if (Random.Next(2) == 0)
                    {
                        maze.Maze[y][x].Add(Constants.DOWN);
                    }
                }
            }

            if (y == maze.Height - 1)
            {
                for (int x = 0; x < maze.Width - 1; x++)
                {
                    if (setOfCells[x] != setOfCells[x + 1])
                    {
                        maze.Maze[y][x].Sub(Constants.RIGHT);

                        int oldSet = setOfCells[x + 1];
                        int newSet = setOfCells[x];

                        for (int j = 0; j < setOfCells.Count; j++)
                        {
                            if (setOfCells[j] == oldSet)
                            {
                                setOfCells[j] = newSet;
                            }
                        }
                    }
                }
            }
        }
    }
}