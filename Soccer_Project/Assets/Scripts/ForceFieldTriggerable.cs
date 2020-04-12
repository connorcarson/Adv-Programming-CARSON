using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldTriggerable : MonoBehaviour
{
    [HideInInspector] public float forceFieldRange = 1.0f;
    [HideInInspector] public float hitForce = 500f;
    
    private SpriteRenderer forceField_sr;
    private Vector3 forceFieldDefaultScale, forceFieldMaxScale;
    private float forceFieldDuration = 0.2f;
    private float timeElapsed;
    private int targetLayer = 1 << 8;


    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        forceField_sr = GetComponentInChildren<SpriteRenderer>();
        forceFieldMaxScale = new Vector3(3.0f, 3.0f, 3.0f);
        forceFieldDefaultScale = forceField_sr.transform.localScale;
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Trigger();
        }
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
