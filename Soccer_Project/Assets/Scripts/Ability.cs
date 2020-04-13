using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName = "New Ability";
    public Sprite abilityEffectSprite;
    public Sprite abilityIcon;
    public AudioClip abilitySound;
    public float baseCoolDown = 1f;
    public float baseCost = 1f;
    public Color abilityColor;

    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();
}