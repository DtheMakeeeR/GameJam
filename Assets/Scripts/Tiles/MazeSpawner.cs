using UnityEngine;
using System.Collections.Generic;

public class MazeSpawner : MonoBehaviour
{
    [Header("Настройки лабиринта")]
    public int width = 10;
    public int height = 10;
    private const float TileSize = 1f; // Расстояние между центрами тайлов

    [Header("Префаб")]
    public Tile tilePrefab;

    private void Start()
    {
        GenerateAndSpawnMaze();
    }

    private void GenerateAndSpawnMaze()
    {
        MazeGrid grid = new MazeGrid(width, height);
        EllerGenerator generator = new EllerGenerator();
        generator.Generate(grid);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x < width - 1)
                {
                    bool hasWall = grid.Maze[y][x].Walls[Constants.RIGHT] == 1 || 
                                   grid.Maze[y][x + 1].Walls[Constants.LEFT] == 1;
                                   
                    grid.Maze[y][x].Walls[Constants.RIGHT] = hasWall ? 1 : 0;
                    grid.Maze[y][x + 1].Walls[Constants.LEFT] = hasWall ? 1 : 0;
                }
                
                if (y < height - 1)
                {
                    bool hasWall = grid.Maze[y][x].Walls[Constants.DOWN] == 1 || 
                                   grid.Maze[y + 1][x].Walls[Constants.UP] == 1;
                                   
                    grid.Maze[y][x].Walls[Constants.DOWN] = hasWall ? 1 : 0;
                    grid.Maze[y + 1][x].Walls[Constants.UP] = hasWall ? 1 : 0;
                }
            }
        }

        List<Tile> spawnedTiles = new List<Tile>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell logicCell = grid.Maze[y][x];
                
                float spawnY = (height - 1 - y) * TileSize;
                Vector3 spawnPos = new Vector3(x * TileSize - width/2, spawnY - height/2, 0);
                
                Tile newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                newTile.Setup(logicCell.Walls);
                
                newTile.gameObject.name = $"Tile_{spawnPos.x}_{spawnPos.y}";
                
                spawnedTiles.Add(newTile);
            }
        }

        if (TilesManager.Instance != null)
        {
            TilesManager.Instance.InitDynamically(spawnedTiles.ToArray());
        }
    }
}