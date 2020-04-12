using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldTriggerable : MonoBehaviour
{
    [HideInInspector] private float forceFieldRange = 1.0f;
    [HideInInspector] private float hitForce = 250f;
    private WaitForSeconds forceFieldDuration = new WaitForSeconds(0.5f);
    private int targetLayer = 1 << 8;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        var enemiesInRange = EnemiesInRange();

        if (enemiesInRange.Length <= 0) return;

        foreach (var enemy in enemiesInRange)
        {
            Debug.Log(enemy.name);
            var dir = enemy.transform.position - transform.position;

            var rb = enemy.GetComponent<Rigidbody>();
            rb.AddForce(dir * hitForce);
        }
    }

    private Collider[] EnemiesInRange()
    {
        return Physics.OverlapSphere(transform.position, forceFieldRange, targetLayer);
    }
}
