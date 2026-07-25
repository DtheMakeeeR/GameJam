using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Boy : Interactble
{
    [SerializeField] private AudioClip _meetSound;
    [SerializeField] private int _steps = 1;
    public override void MakeInteraction(PlayerController player)
    {
        Debug.Log("Boy meets player");
        GameObject newPlace = GameObject.FindWithTag("NPC_Place");
        transform.parent.transform.SetParent(newPlace.transform);
        transform.parent.transform.localPosition = Vector3.zero;
        SFXManager.Instance.PlaySoundOnce(_meetSound);
    }
}
