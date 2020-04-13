using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ForceFieldAbility")]
public class ForceFieldAbility : Ability
{
    public float forceFieldRange;
    public float hitForce;
    public float forceFieldDuration;
    public Vector3 forceFieldMaxScale;
    
    private ForceFieldTriggerable forceField;

    public override void Initialize(GameObject obj)
    {
        forceField = obj.GetComponent<ForceFieldTriggerable>();
        forceField.Initialize();
        
        forceField.forceFieldRange = this.forceFieldRange;
        forceField.hitForce = this.hitForce;
        forceField.forceFieldDuration = this.forceFieldDuration;
        forceField.forceFieldMaxScale = this.forceFieldMaxScale;
        forceField.forceField_sr.sprite = this.abilityEffectSprite;
        forceField.forceField_sr.color = this.abilityColor;
    }

    public override void TriggerAbility()
    {
        forceField.Trigger();
        forceField.audioSource.PlayOneShot(abilitySound);
    }
}
