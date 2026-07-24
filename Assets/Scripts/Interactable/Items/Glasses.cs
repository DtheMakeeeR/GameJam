using UnityEngine;

public class Glasses : Interactble
{
    public override void MakeInteraction(PlayerController player)
    {
        player.TurnGlasses(true);
        transform.parent?.gameObject.SetActive(false);
    }
}
