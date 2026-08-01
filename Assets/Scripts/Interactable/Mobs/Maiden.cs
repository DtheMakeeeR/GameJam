using System.Linq;
using UnityEngine;

public class Maiden : Interactble, IMob
{
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private int _runsCount = 2;
    [SerializeField] private int _steps = 1;
    public int Steps => _steps;
    private bool CanMove = true;
    private bool _direction = true; // true = right, false = left
    private Vector3[] _directions = new Vector3[4] { Vector3.up, Vector3.right, -Vector3.up, -Vector3.right };
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
        Debug.Log("Maidene attacks player");
        StepsManager.Instance.RunEnemies(_runsCount);
        SFXManager.Instance.PlaySoundOnce(_attackSound);
        transform.parent?.gameObject.SetActive(false);
        CanMove = false;
    }

    public void MakeStep()
    {
        if (!CanMove) { return; }
        if (_direction)
        {
            for (int i = 0; i < 3; i++, DirIndex++)
            {
                var dir = _directions[DirIndex];
                if (TilesManager.Instance.CanEnterTile(transform.parent.position, transform.parent.position + dir))
                {
                    Debug.Log($"Maiden can move in {dir} direction");
                    transform.parent.position += dir;
                    SFXManager.Instance.PlaySoundOnce(_walkSound);
                    return;
                }
            }
            _direction = false;
        }
        else
        {
            for (int i = 0; i < 3; i++, DirIndex--)
            {
                var dir = _directions[DirIndex];
                if (TilesManager.Instance.CanEnterTile(transform.parent.position, transform.parent.position + dir))
                {
                    Debug.Log($"Maiden can move in {dir} direction");
                    transform.parent.position += dir;
                    SFXManager.Instance.PlaySoundOnce(_walkSound);
                    return;
                }
            }
            _direction = true;
        }
    }
}
