
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Mover mover;
    [SerializeField] float timeToShoot = 3f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float attackHeight = 1.5f;
    [SerializeField] Transform player;

    private float shotTimer =0f;
    private EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.OnStateChanged += EnemyAI_OnStateChanged;
    }

    private void EnemyAI_OnStateChanged(object sender, EnemyAI.OnStateChangedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
        shotTimer += Time.deltaTime;
    }

    private void HandleMovement()
    {
        if (enemyAI.GetState() == State.Running)
        {
            mover.MoveTo(transform.position + transform.forward);
        }
        else
        {
            mover.CancelMoveAction();
        }
        
    }

    private void HandleAttack()
    {
        float randNumber = Random.Range(0, timeToShoot);
        if(shotTimer > randNumber)
        {
            shotTimer = 0f;
            enemyAI.SetState(State.Idle);
            StartCoroutine(ShootRoutine());
            
        }
    }

    private void Shoot()
    {
        if (player == null)
        {
            return;
        }
        GameObject arrowGO = Instantiate(projectilePrefab, transform.position + attackHeight * Vector3.up, Quaternion.identity);

        arrowGO.transform.forward = player.transform.position - transform.position;
    }

    IEnumerator ShootingAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            Shoot();
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ShootRoutine()
    {
        enemyAI.SetState(State.Atacking);
        yield return new WaitForSeconds(0.2f);
        

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(ShootingAttack());

        yield return new WaitForSeconds(0.2f);

        enemyAI.SetState(State.Running);
    }
    private bool CanMoveToDestination(Vector3 destination)
    {
        // Calculate the path from the current position to the destination
        NavMeshPath path = new NavMeshPath();
        bool canMove = navMeshAgent.CalculatePath(destination, path);

        // If the path status is not "PathComplete," the destination is not reachable
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            canMove = false;
        }

        return canMove;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(-10, 10);
        transform.forward = new Vector3(randomX, 0, randomZ);
    }
}
