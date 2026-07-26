using UnityEngine;

public class Exit : Interactble
{
    [SerializeField] private AudioClip _exitSound;
    [SerializeField] private bool _canExit;
    [SerializeField] private bool _hasBoy = true;
    public void SetCanExit(bool canExit) => _canExit = canExit;
    public void SetHasBoy(bool hasBoy) => _hasBoy = hasBoy;
    public override void MakeInteraction(PlayerController player)
    {
        if(_canExit && _hasBoy) LevelFlagsManager.Instance.DecreaseFlags();
    }
}
