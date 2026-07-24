using System;
using UnityEngine;

public class Boots : Interactble
{
    [SerializeField] private int _stepsIncrease = 1;
    public override void MakeInteraction(PlayerController player)
    {
        player.AddSteps(_stepsIncrease);
        player.IncreaseStepsPerCycle(_stepsIncrease);
        transform.parent?.gameObject.SetActive(false);
    }
}
