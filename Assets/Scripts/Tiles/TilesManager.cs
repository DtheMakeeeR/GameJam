using UnityEngine;
using System.Linq;

public class TilesManager : MonoBehaviour
{
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private float _tilesLevel;
    public static TilesManager Instance { get; private set; }

    
    private void Awake()
    {
        Instance = this;
    }
    
    private Tile GetTileAtPosition(Vector3 position)
    {
        foreach (var tile in _tiles)
        {
            Debug.Log($"Compare {tile.transform.position} with {position} ");
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
    
    public void InitDynamically(Tile[] newTiles)
    {
        _tiles = newTiles;
    }
}
