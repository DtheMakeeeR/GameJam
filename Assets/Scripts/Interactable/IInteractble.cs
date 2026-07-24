using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactble: MonoBehaviour
{
    public abstract void MakeInteraction(PlayerController player);

}
