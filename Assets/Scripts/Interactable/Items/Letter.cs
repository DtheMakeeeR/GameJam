using UnityEngine;

public class Letter : Interactble
{
    [SerializeField] private GameObject _panel;

    public override void MakeInteraction(PlayerController player)
    {
        _panel.SetActive(true);
        player.SpecialFlag = false;
        transform.parent?.gameObject.SetActive(false);
    }
}
