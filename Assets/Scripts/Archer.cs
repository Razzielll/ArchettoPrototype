
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private float searchRadius;
    [SerializeField] private Health currentTarget;
    [SerializeField] private float updateFrequency = 0.2f;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float bowPositionHeight = 1.5f;
    [SerializeField] private float shootingFrequency = 1f;
    private Player player;
    private float targetUpdateTimer = 0;
    private float shootingTimer = 0;
    private bool isShooting = false;
    private void Start()
    {
        player = GetComponent<Player>();
        player.OnStateChanged += Player_OnStateChanged;
    }

    private void Player_OnStateChanged(object sender, Player.OnStateChangedEventArgs e)
    {
        if(e.state == State.Atacking)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }
    }

    void Update()
    {
        if (targetUpdateTimer > updateFrequency)
        {
            UpdateTarget();
            targetUpdateTimer = 0;
        }

        if (shootingTimer > 1f / shootingFrequency && isShooting)
        {

            Shoot();


            shootingTimer = 0;
        }

        UpdateTimers();
    }

    private void UpdateTimers()
    {
        targetUpdateTimer += Time.deltaTime;
        shootingTimer += Time.deltaTime;
    }

    private void Shoot()
    {
        if (currentTarget == null)
        {
            return;
        }
        GameObject arrowGO = Instantiate(arrowPrefab, transform.position + bowPositionHeight * Vector3.up, Quaternion.identity);

        arrowGO.transform.forward = currentTarget.transform.position - transform.position;
    }

    void UpdateTarget()
    {
        Health enemyHealth = FindNewTargetInRange(searchRadius);
        if (enemyHealth == null)
        {
            return;
        }
        currentTarget = enemyHealth;
    }

    private Health FindNewTargetInRange(float searchRadius)
    {
        Health best = null;
        float bestDistance = Mathf.Infinity;
        foreach (var candidate in FindAllTargetsInRange(searchRadius))
        {
            float candidateDistance = Vector3.Distance(transform.position, candidate.transform.position);
            if (candidateDistance < bestDistance)
            {
                best = candidate;
                bestDistance = candidateDistance;
            }
        }
        return best;
    }

    private IEnumerable<Health> FindAllTargetsInRange(float searchRadius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, searchRadius, Vector3.up, 0);

        foreach (var hit in hits)
        {
            Health target = hit.transform.GetComponent<Health>();

            if (target == null)
            {
                continue;
            }
            if (target.gameObject != this.gameObject)
            {
                yield return target;
            }
        }
    }

    public Health GetCurrentTarget()
    {
        return currentTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
