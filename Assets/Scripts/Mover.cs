
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.3f;
    [SerializeField] private float rotateSpeed = 20f;


    private NavMeshAgent navMeshAgent;
    private bool isRunning =false;
    

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 destination)
    {   isRunning = true;

        Vector3 moveDirection = navMeshAgent.velocity.normalized;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        navMeshAgent.destination = destination;
        navMeshAgent.speed = maxSpeed;
        navMeshAgent.isStopped = false;
    }

    public void CancelMoveAction()
    {
        isRunning = false;
        navMeshAgent.isStopped = true;
    }

}
