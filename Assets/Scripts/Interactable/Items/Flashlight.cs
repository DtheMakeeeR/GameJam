using UnityEngine;

public class Flashlight : Interactble
{
    public override void MakeInteraction(PlayerController player)
    {
        player.TurnFlashlight(true);
        transform.parent?.gameObject.SetActive(false);
    }
}
