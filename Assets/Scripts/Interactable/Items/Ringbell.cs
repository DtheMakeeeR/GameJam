using UnityEngine;

public class Ringbell : Interactble
{
    public override void MakeInteraction(PlayerController player)
    {
        StepsManager.Instance.RunEnemies();
        transform.parent?.gameObject.SetActive(false);
    }
}
