using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Toy : Interactble
{
    [SerializeField] private AudioClip _meetSound;
    public override void MakeInteraction(PlayerController player)
    {
        Debug.Log("Player finds Toy");
        LevelFlagsManager.Instance.DecreaseFlags();
        GameObject newPlace = GameObject.FindWithTag("Item_Place");
        transform.parent.transform.SetParent(newPlace.transform);
        transform.parent.transform.localPosition = Vector3.zero;
        SFXManager.Instance.PlaySoundOnce(_meetSound);
    }
}
