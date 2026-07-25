using UnityEngine;

public class Exit : Interactble
{
    [SerializeField] private AudioClip _exitSound;
    [SerializeField] private bool _canExit;
    public void SetCanExit(bool canExit) => _canExit = canExit;
    public override void MakeInteraction(PlayerController player)
    {
        if(_canExit) LevelFlagsManager.Instance.DecreaseFlags();
    }
}
