using UnityEngine;
using System.Collections.Generic;

public class MazeSpawner : MonoBehaviour
{
    [Header("Настройки лабиринта")] 
    public int width = 10;
    public int height = 10;
    public const float TileSize = 1f;
    [SerializeField] private bool _hasExit = false;

    [Header("Префаб")]
    public Tile tilePrefab;
    
    [Header("Сущности")]
    [SerializeField] private GameObject[] _entities;

    public static MazeSpawner Instance { get; private set; }
    public MazeGrid Grid { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        GenerateAndSpawnMaze();
    }

    private void GenerateAndSpawnMaze()
    {
        Grid = new MazeGrid(width, height);
        bool[,] entitiesPosition = new bool[width, height];
        EllerGenerator generator = new EllerGenerator();
        generator.Generate(Grid);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x < width - 1)
                {
                    bool hasWall = Grid.Maze[y][x].Walls[Constants.RIGHT] == 1 || 
                                   Grid.Maze[y][x + 1].Walls[Constants.LEFT] == 1;
                                   
                    Grid.Maze[y][x].Walls[Constants.RIGHT] = hasWall ? 1 : 0;
                    Grid.Maze[y][x + 1].Walls[Constants.LEFT] = hasWall ? 1 : 0;
                }
                
                if (y < height - 1)
                {
                    bool hasWall = Grid.Maze[y][x].Walls[Constants.DOWN] == 1 || 
                                   Grid.Maze[y + 1][x].Walls[Constants.UP] == 1;
                                   
                    Grid.Maze[y][x].Walls[Constants.DOWN] = hasWall ? 1 : 0;
                    Grid.Maze[y + 1][x].Walls[Constants.UP] = hasWall ? 1 : 0;
                }
            }
        }

        List<Tile> spawnedTiles = new List<Tile>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell logicCell = Grid.Maze[y][x];
                
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
            
            GenerateExit(spawnedTiles);
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                entitiesPosition[i, j] = false;
            }
        }
        
        if (_entities == null) return;
        
        foreach (var entity in _entities)
        {
             int x = Random.Range(0, width);
             int y = Random.Range(0, height);

             while (((x == 0 || x == width - 1) && (y == 0 || y == height - 1)) ||
                    (Mathf.Abs(x - width / 2) < 2 && Mathf.Abs(y - height / 2) < 2) ||
                    entitiesPosition[x, y] == true)
             {
                 x  = Random.Range(0, width);
                 y = Random.Range(0, height);
             }
             
             float spawnY = (height - 1 - y) * TileSize;
             Vector3 spawnPos = new Vector3(x * TileSize - width/2, spawnY - height/2, 0);
     
             Instantiate(entity, spawnPos, Quaternion.identity);
             entitiesPosition[x, y] = true;
        }
    }

    private void GenerateExit(List<Tile> spawnedTiles)
    {
        if (!_hasExit) return;

        int[] cornerX = { 0, width - 1 };
        int[] cornerY = { 0, height - 1 };

        int x = cornerX[Random.Range(0, 2)];
        int y = cornerY[Random.Range(0, 2)];

        int wallIndexToReplace = (y == 0) ? 0 : 2; 

        int tileIndex = y * width + x;
        Tile cornerTile = spawnedTiles[tileIndex];

        cornerTile.SetupExit(wallIndexToReplace);
    }  
}