
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyRanged : EnemyAI
{
    [SerializeField] private float secondsToWait = 0.6f;
    [SerializeField] private float secondsBetweenAttackShots = 0.2f;
   // [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Mover mover;
    
    [SerializeField] float timeToShoot = 3f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float attackHeight = 1.5f;

    private float shotTimer =0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       // enemyAI = GetComponent<EnemyAI>();
        
    }



    // Update is called once per frame
    protected override void Update()
    {
        if (GetComponent<Health>().IsDead())
        {
            return;
        }
        


        HandleShooting();
        HandleAttack();
        HandleRotation();
        shotTimer += Time.deltaTime;
        HandleMovement(transform.forward, 2f);
    }

    protected override void UpdateMoveDirection()
    {
        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(-10, 10);
        transform.forward = new Vector3(randomX, 0, randomZ);
    }

    private void HandleAttack()
    {
        float randNumber = Random.Range(0, timeToShoot);
        if(shotTimer > randNumber)
        {
          //  Debug.Log("Attack");
            shotTimer = 0f;
           SetState(State.Idle);
            
           StartCoroutine(ShootRoutine());
            
        }
    }

    private void Shoot()
    {
        if (playerTransform == null)
        {
            return;
        }

        //Debug.Log("Shhoot");
        GameObject arrowGO = Instantiate(projectilePrefab, transform.position + attackHeight * Vector3.up, Quaternion.identity);

        arrowGO.transform.forward = playerTransform.position - transform.position;
    }

    IEnumerator ShootingAttack()
    {
        for (int i = 0; i < 3; i++)
        {
           // Debug.Log("Shhooting");
            Shoot();
            yield return new WaitForSeconds(secondsBetweenAttackShots);
        }
    }

    IEnumerator ShootRoutine()
    {
        SetState(State.Atacking);
        yield return new WaitForSeconds(secondsToWait);
        
        StartCoroutine(ShootingAttack());
        yield return new WaitForSeconds(secondsToWait);
        

        yield return new WaitForSeconds(secondsToWait);
        
        SetState(State.Running);
    }

    
}
