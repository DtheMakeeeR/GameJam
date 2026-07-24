using System.Linq;
using UnityEngine;

public class Skeleton : Interactble, IMob
{
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private int _steps = 1;
    public int Steps => _steps;

    private bool _direction = true; // true = right, false = left
    private Vector3[] _directions = new Vector3[4] {Vector3.up,Vector3.right, -Vector3.up, -Vector3.right};
    private int _dirIndex = 0;
    private int DirIndex
    {
        get => _dirIndex;
        set
        {
            if (value < 0)
            {
                _dirIndex = _directions.Length - 1;
            }
            else if (value >= _directions.Length)
            {
                _dirIndex = 0;
            }
            else
            {
                _dirIndex = value;
            }
        }
    }
    public override void MakeInteraction(PlayerController player)
    {
        Debug.Log("Skeleton attacks player");
        SFXManager.Instance.PlaySoundOnce(_attackSound);
        player.Die();
    }

    public void MakeStep()
    {
        if (_direction)
        {
            var dir = _directions[DirIndex];
            if (TilesManager.Instance.CanEnterTile(transform.parent.position, transform.parent.position + dir))
            {
                Debug.Log($"Skeleton cav move in {dir} direction");
                transform.parent.position += dir;
                SFXManager.Instance.PlaySoundOnce(_walkSound);
                return;
            }
            DirIndex++;
            if (DirIndex == 0)
            {
                _direction = false;
            }
        }
        else
        {
            var dir = _directions[DirIndex];
            if (TilesManager.Instance.CanEnterTile(transform.parent.position, transform.parent.position + dir))
            {
                Debug.Log($"Skeleton cav move in {dir} direction");
                transform.parent.position += dir;
                SFXManager.Instance.PlaySoundOnce(_walkSound);
                return;
            }
            DirIndex--;
            if (DirIndex == _directions.Length-1)
            {
                _direction = true;
            }
        }
    }
}
