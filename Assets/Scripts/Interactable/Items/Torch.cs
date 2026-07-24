using UnityEngine;

public class Torch : Interactble
{
    [SerializeField] private float _distance = 2f;
    [SerializeField] private AudioClip _pickUpSound;
    public override void MakeInteraction(PlayerController player)
    {
        player.SetViewDistance(_distance);
        SFXManager.Instance.PlaySoundOnce(_pickUpSound);
        transform.parent?.gameObject.SetActive(false);
    }
}
