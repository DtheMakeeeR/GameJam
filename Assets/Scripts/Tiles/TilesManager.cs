using UnityEngine;
using System.Linq;

public class TilesManager : MonoBehaviour
{
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private float _tilesLevel;
    public static TilesManager Instance { get; private set; }
    
    private void Awake()
    {
        // if(Instance == null)
        // {
        //     Instance = this;
        // }
        // else
        // {
        //     Destroy(this);
        // }
        Instance = this;
    }
    private void Start()
    {
        SortAndRenameTiles();
    }

    private void SortAndRenameTiles()
    {
        var sortedTiles = from t in _tiles orderby (t.transform.position.y, t.transform.position.x) select t;
        _tiles = sortedTiles.ToArray();
        foreach (var tile in _tiles)
        {
            tile.gameObject.name = $"Tile_{tile.transform.position.x}_{tile.transform.position.y}";
        }
    }
    private Tile GetTileAtPosition(Vector3 position)
    {
        foreach (var tile in _tiles)
        {
            if (tile.transform.position == position)
            {
                return tile;
            }
        }
        return null;
    }
    public bool CanEnterTile(Vector3 startPos, Vector3 targetPos)
    {
        Tile targetTile = GetTileAtPosition(targetPos.With(z: _tilesLevel));
        if (targetTile == null)
        {
            Debug.Log("Target tile not found");
            return false;
        }
        return targetTile.CanEnter(startPos);
    }
}
