using UnityEngine;

public class Key : Interactble
{
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private Exit _exitObject;

    private void Start()
    {
        _exitObject = GameObject.FindGameObjectWithTag("Exit").GetComponent<Exit>();
    }
    private void OpenDoor()
    {
        _exitObject.SetCanExit(true);
    }

    public override void MakeInteraction(PlayerController player)
    {
        SFXManager.Instance.PlaySoundOnce(_pickUpSound);
        OpenDoor();
        LevelFlagsManager.Instance.DecreaseFlags();
        transform.parent?.gameObject.SetActive(false);
    }
}
