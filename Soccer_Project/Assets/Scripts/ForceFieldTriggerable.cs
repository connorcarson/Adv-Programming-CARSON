using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldTriggerable : MonoBehaviour
{
    [HideInInspector] public float forceFieldRange, hitForce, forceFieldDuration;
    [HideInInspector] public Vector3 forceFieldMaxScale;
    [HideInInspector] public SpriteRenderer forceField_sr;
    [HideInInspector] public AudioSource audioSource;
    
    private Vector3 forceFieldDefaultScale;
    private float timeElapsed;
    private int targetLayer = 1 << 8;

    public void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        forceField_sr = GetComponentInChildren<SpriteRenderer>();
        forceFieldDefaultScale = forceField_sr.transform.localScale;
    }

    public void Trigger()
    {
        StartCoroutine(FieldEffect());

        var enemiesInRange = EnemiesInRange();

        if (enemiesInRange.Length <= 0) return;

        foreach (var enemy in enemiesInRange)
        {
            Debug.Log(enemy.name);
            var dir = enemy.transform.position - transform.position;

            var rb = enemy.GetComponent<Rigidbody>();
            rb.AddForce(dir.normalized * hitForce);
        }
    }

    private Collider[] EnemiesInRange()
    {
        return Physics.OverlapSphere(transform.position, forceFieldRange, targetLayer);
    }
    
    private IEnumerator FieldEffect()
    {
        while (timeElapsed < forceFieldDuration)
        {
            forceField_sr.transform.localScale = Vector3.Lerp(forceField_sr.transform.localScale, forceFieldMaxScale, timeElapsed / forceFieldDuration);

            yield return null;
            
            timeElapsed += Time.deltaTime;
        }

        timeElapsed = 0;
        forceField_sr.transform.localScale = forceFieldDefaultScale;
    }
}
