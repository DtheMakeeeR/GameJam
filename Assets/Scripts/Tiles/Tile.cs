using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool[] _walls = new bool[4];
    
    [SerializeField] private GameObject[] _wallVisuals = new GameObject[4]; 

    public void Setup(int[] cellWalls)
    {
        for (int i = 0; i < 4; i++)
        {
            _walls[i] = cellWalls[i] == 1;
            
            if (_wallVisuals.Length > i && _wallVisuals[i] != null)
            {
                _wallVisuals[i].SetActive(_walls[i]);
            }
        }
    }
    public bool CanEnter(Vector3 startPos)
    {
        if(startPos.y > transform.position.y)
        {
            if(_walls[0])
            {
                Debug.Log("Cannot enter from top");
                return false;
            }
            else
            {
                Debug.Log("Can enter from top");
                return true;
            }
        }
        else if(startPos.y < transform.position.y)
        {
            if(_walls[2])
            {
                Debug.Log("Cannot enter from bottom");
                return false;
            }
            else
            {
                Debug.Log("Can enter from bottom");
                return true;
            }
        }
        else if(startPos.x > transform.position.x)
        {
            if(_walls[1])
            {
                Debug.Log("Cannot enter from right");
                return false;
            }
            else
            {
                Debug.Log("Can enter from right");
                return true;
            }
        }
        else
        {
            if(_walls[3])
            {
                Debug.Log("Cannot enter from left");
                return false;
            }
            else
            {
                Debug.Log("Can enter from left");
                return true;
            }
        }
    }

    public void ChangeWall(int wallIndex, bool flag)
    {
        _walls[wallIndex] = flag;
    }
}
