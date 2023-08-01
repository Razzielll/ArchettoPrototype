using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Archer : MonoBehaviour
{
    // The radius within which the archer searches for targets
    [SerializeField] private float searchRadius;

    // The current target (enemy with Health component) the archer is aiming at
    [SerializeField] private Health currentTarget;

    // The frequency at which the target is updated
    [SerializeField] private float updateFrequency = 0.2f;

    // Prefab of the arrow that the archer shoots
    [SerializeField] private GameObject arrowPrefab;

    // Height of the bow position from the archer's position
    [SerializeField] private float bowPositionHeight = 1.5f;

    // Frequency at which the archer shoots arrows
    [SerializeField] private float shootingFrequency = 1f;

    private Player player;

    // Timer for updating the target
    private float targetUpdateTimer = 0;

    // Timer for shooting arrows
    private float shootingTimer = 0;

    // Flag to indicate if the archer is in shooting mode
    private bool isShooting = false;

    private void Start()
    {
        player = GetComponent<Player>();
        player.OnStateChanged += Player_OnStateChanged;
    }

    // Event handler for the player state change event
    private void Player_OnStateChanged(object sender, Player.OnStateChangedEventArgs e)
    {
        // If the player is in attacking state, the archer enters shooting mode
        if (e.state == State.Atacking)
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
        // If the target update timer exceeds the update frequency, update the target
        if (targetUpdateTimer > updateFrequency)
        {
            UpdateTarget();
            targetUpdateTimer = 0;
        }

        // If the shooting timer exceeds the shooting frequency and the archer is in shooting mode, shoot an arrow
        if (shootingTimer > 1f / shootingFrequency && isShooting)
        {
            Shoot();
            shootingTimer = 0;
        }

        UpdateTimers();
    }

    // Update the target update and shooting timers
    private void UpdateTimers()
    {
        targetUpdateTimer += Time.deltaTime;
        shootingTimer += Time.deltaTime;
    }

    // Shoot an arrow at the current target (if any)
    private void Shoot()
    {
        if (currentTarget == null)
        {
            return;
        }

        // Instantiate an arrow and set its forward direction towards the current target
        GameObject arrowGO = Instantiate(arrowPrefab, transform.position + bowPositionHeight * Vector3.up, Quaternion.identity);
        arrowGO.transform.forward = currentTarget.transform.position - transform.position;
    }

    // Update the current target by finding the closest enemy with Health component in the search radius
    void UpdateTarget()
    {
        Health enemyHealth = FindNewTargetInRange(searchRadius);
        if (enemyHealth == null)
        {
            return;
        }
        currentTarget = enemyHealth;
    }

    // Find the closest enemy with Health component within the given search radius
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

    // Find all the enemies with Health component within the given search radius
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

    // Get the current target of the archer
    public Health GetCurrentTarget()
    {
        return currentTarget;
    }

    // Draw a red wire sphere in the scene to represent the search radius of the archer
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
