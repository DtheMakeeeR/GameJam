using UnityEngine;

public class Key : Interactble
{
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private Tile _door;

    private void Start()
    {
        if( _door == null )
        {
            Debug.LogWarning($"Warning! {gameObject.name} has no door reference!");
        }
    }
    private void OpenDoor()
    {
        for(int i = 0; i < 4; i++)
        {
            _door.ChangeWall(i, false);
        }
    }

    public override void MakeInteraction(PlayerController player)
    {
        SFXManager.Instance.PlaySoundOnce(_pickUpSound);
        OpenDoor();
        transform.parent?.gameObject.SetActive(false);
    }
}
