using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ForceFieldAbility")]
public class ForceFieldAbility : Ability
{
    public float forceFieldRange = 1.0f;
    public float hitForce = 10f;
    public Color forceFieldColor = Color.white;

    public override void Initialize(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerAbility()
    {
        throw new System.NotImplementedException();
    }
}
