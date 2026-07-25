using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour, IMob
{
    [Header("Настройка звуков")]
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private AudioClip _spwanSound;
    [SerializeField] private AudioClip _killSound;

    [SerializeField] private int _steps = 1;
    [SerializeField] private int _timeOut = 3;
    
    public int Steps => _steps;
    private List<Vector3> _directions;
    private string _mobTag = "Boy";
    GameObject _target;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Invoke(nameof(SpawnOnTheCenter), 0.3f);
        _spriteRenderer.enabled = false;        
    }

    private void SpawnOnTheCenter()
    {
        transform.parent.position = new Vector3(0, 0, 0);
        FindTargetPath();
    }
    
    private void FindTargetPath()
    {
        _target = GameObject.FindGameObjectWithTag(_mobTag);

        MazeGrid grid = MazeSpawner.Instance.Grid;
        int width = MazeSpawner.Instance.Grid.Width;
        int height = MazeSpawner.Instance.Grid.Height;
        
        (int startX, int startY) = WorldToGrid(transform.parent.position, width, height);
        (int targetX, int targetY) = WorldToGrid(_target.transform.position, width, height);
        
        Cell startCell = grid.Maze[startY][startX];
        Cell targetCell = grid.Maze[targetY][targetX];
        
        List<Cell> wayToTarget = Pathfinding.FindPathBFS(grid, startCell, targetCell);
        
        _directions = ConvertPathToDirections(wayToTarget);
    }

    public void MakeStep()
    {
        if (_timeOut > 0)
        {
            _timeOut--;

            if (_timeOut <= 0)
            {
                _spriteRenderer.enabled = true;
                SFXManager.Instance.PlaySoundOnce(_spwanSound);
            }
            
            return;
        }

        if (_directions.Count > 0)
        {
            Debug.Log($"Shadow moves in {_directions[0]} direction");
            transform.parent.position += _directions[0];
            _directions.RemoveAt(0);
            SFXManager.Instance.PlaySoundOnce(_walkSound);
        }
    }

    private (int x, int y) WorldToGrid(Vector3 worldPos, int width, int height)
    {
        int x = Mathf.RoundToInt(worldPos.x + width / 2);
        float spawnY = worldPos.y + height / 2;
        int y = height - 1 - Mathf.RoundToInt(spawnY);
        return (x, y);
    }
    
    private List<Vector3> ConvertPathToDirections(List<Cell> path)
    {
        List<Vector3> directions = new List<Vector3>();

        if (path == null || path.Count < 2)
            return directions;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Cell currentCell = path[i];
            Cell nextCell = path[i + 1];

            int dx = nextCell.Coordinates.X - currentCell.Coordinates.X;
            int dy = nextCell.Coordinates.Y - currentCell.Coordinates.Y;

            if (dx == 1 && dy == 0)
            {
                directions.Add(Vector3.right);
            }
            else if (dx == -1 && dy == 0)
            {
                directions.Add(Vector3.left);
            }
            else if (dx == 0 && dy == -1)
            {
                directions.Add(Vector3.up);
            }
            else if (dx == 0 && dy == 1)
            {
                directions.Add(Vector3.down);
            }
        }

        return directions;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_mobTag))
        {
            SFXManager.Instance.PlaySoundOnce(_killSound);
            
            StartCoroutine(KillWaitAndQuit());
        }
    }

    IEnumerator KillWaitAndQuit()
    {
        yield return new WaitForSeconds(3f);
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
