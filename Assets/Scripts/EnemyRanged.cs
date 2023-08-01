using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyRanged : EnemyAI
{
    // Time to wait before starting the shooting attack
    [SerializeField] private float secondsToWait = 0.6f;

    // Time between consecutive shots during the shooting attack
    [SerializeField] private float secondsBetweenAttackShots = 0.2f;



    // Maximum random time for the enemy to shoot in a given interval
    [SerializeField] float maxRandomTimeToShoot = 3f;

    // Prefab of the projectile the enemy shoots
    [SerializeField] GameObject projectilePrefab;

    // Height at which the projectiles are spawned for shooting
    [SerializeField] float attackHeight = 1.5f;

    // Timer to track the time since the last shot
    private float shotTimer = 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // If the enemy is dead, return early and do nothing
        if (GetComponent<Health>().IsDead())
        {
            return;
        }

        // Handle shooting attack logic
        HandleShooting();
        // Handle attack state transition
        HandleAttack();
        // Handle rotation to face the player
        HandleRotation();
        // Update the shot timer
        shotTimer += Time.deltaTime;
        // Handle enemy movement
        HandleMovement(transform.forward, 2f);
    }

    // Update the movement direction of the enemy (randomized)
    protected override void UpdateMoveDirection()
    {
        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(-10, 10);
        transform.forward = new Vector3(randomX, 0, randomZ);
    }

    // Handle the enemy's shooting attack
    private void HandleAttack()
    {
        // Generate a random time for the enemy to shoot within the given interval
        float randNumber = Random.Range(0, maxRandomTimeToShoot);

        // If the shot timer exceeds the random time, initiate the shooting attack
        if (shotTimer > randNumber)
        {
            shotTimer = 0f;
            // Set the enemy state to Idle (to prepare for the shooting attack)
            SetState(State.Idle);

            // Start the shooting attack coroutine
            StartCoroutine(ShootRoutine());
        }
    }

    // Create a projectile and shoot it in the direction of the player
    private void Shoot()
    {
        if (playerTransform == null)
        {
            return;
        }

        GameObject arrowGO = Instantiate(projectilePrefab, transform.position + attackHeight * Vector3.up, Quaternion.identity);

        arrowGO.transform.forward = playerTransform.position - transform.position;
    }

    // Coroutine for handling consecutive shooting attack shots
    IEnumerator ShootingAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            Shoot();
            yield return new WaitForSeconds(secondsBetweenAttackShots);
        }
    }

    // Coroutine for handling the complete shooting attack sequence
    IEnumerator ShootRoutine()
    {
        // Set the enemy state to Attacking
        SetState(State.Atacking);
        yield return new WaitForSeconds(secondsToWait);

        // Start the consecutive shooting attack
        StartCoroutine(ShootingAttack());
        yield return new WaitForSeconds(secondsToWait);

        yield return new WaitForSeconds(secondsToWait);

        // Set the enemy state back to Running after the attack is finished
        SetState(State.Running);
    }
}
