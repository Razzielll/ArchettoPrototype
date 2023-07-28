
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 rawInput;
    
    private NavMeshAgent navMeshAgent;
    [SerializeField] float maxSpeed =5f;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] float stoppingDistance = 0.3f;
    [SerializeField] float cuttingRange = 0.3f;
    
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] float moveRange = 10f;
    [SerializeField] GameObject RestartMenu;

    bool inBase = false;

    public event Action<bool> OnAttackPatternChange;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        
        rawInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        
        Vector3 moveInput = new Vector3(transform.position.x + moveRange* rawInput.x, 0, transform.position.z + moveRange* rawInput.y);
        Vector3 moveDirection = new Vector3(moveRange * rawInput.x, 0, moveRange * rawInput.y);
        if (moveDirection.magnitude >0)
        {            
            transform.forward = moveDirection;
        }

        MoveTo(moveInput);      
    }

    public float GetCuttingRange()
    {
        return cuttingRange;
    }

    
    public void MoveTo(Vector3 destination)
    {
        if(Vector3.Distance(destination,transform.position) > 0.5f)
        {
            transform.LookAt(destination);
        }        
        navMeshAgent.destination = destination;
        navMeshAgent.speed = maxSpeed;
        navMeshAgent.isStopped = false;
    }

    public void CancelMoveAction()
    {
        navMeshAgent.isStopped = true;
    }


    public Vector2 GetPointer()
    {
         return Camera.main.ScreenToWorldPoint(playerInput.actions["Look"].ReadValue<Vector2>());
    }
    public Ray GetPointerPosition()
    {
         return Camera.main.ScreenPointToRay(playerInput.actions["Look"].ReadValue<Vector2>());
    }

    public Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyZone"))
        {
            inBase = false;
            OnAttackPatternChange?.Invoke(inBase);
        }

        if (other.CompareTag("BaseZone"))
        {
            
            inBase = true;

            OnAttackPatternChange?.Invoke(inBase);
        }
    }

    public void SetInBase()
    {
        inBase = true;
        OnAttackPatternChange?.Invoke(inBase);
    }


    public bool GetInAttackMode()
    {
        return inBase;
    }

   public void OnMenu()
    {
        RestartMenu.SetActive(!RestartMenu.activeInHierarchy);
    }
}
