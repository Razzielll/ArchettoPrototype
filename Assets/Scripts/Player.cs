
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs: EventArgs
    {
    }


    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] float rotateSpeed = 20f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float playerRadius = 0.4f;
    [SerializeField] private float playerHeight = 1.0f;

    private bool isRunning = false;
    private State state;
    private Archer archer;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There is more than one Player instance");
        }
        Instance = this;
    }
    private void Start()
    {
        archer = GetComponent<Archer>();
        gameInput = FindObjectOfType<GameInput>();
    }

    

    private void Update()
    {
        if (GetComponent<Health>().IsDead())
        {
            return;
        }
        HandleMovement();
        HandleShooting();
        HandleRotation();
    }

    private void HandleRotation()
    {
        switch (state)
        {
            case State.Idle:
                if (archer.GetCurrentTarget() != null)
                {
                    Vector3 baseLook = archer.GetCurrentTarget().transform.position - transform.position;
                    Vector3 lookDirection = new Vector3(baseLook.x, 0, baseLook.z);
                    transform.forward = Vector3.Slerp(transform.forward, lookDirection, Time.deltaTime * rotateSpeed);
                }
                break;
            case State.Running:
                break;

            case State.Atacking:
                if(archer.GetCurrentTarget() != null)
                {
                    Vector3 baseLook = archer.GetCurrentTarget().transform.position - transform.position;
                    Vector3 lookDirection = new Vector3(baseLook.x,0, baseLook.z);
                    transform.forward = Vector3.Slerp(transform.forward, lookDirection, Time.deltaTime * rotateSpeed);
                }
                
                break;
            default:
                break;
        }
    }

    private void HandleShooting()
    {
        if(state == State.Idle)
        {
            state = State.Atacking;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {state = state
            });
        }
        
    }

    public bool IsWalking()
    {
        return isRunning;
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDistance = Time.deltaTime * moveSpeed;
        
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //Attempt only X Movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                //Can move only on the X
                moveDir = moveDirX;
                
            }
            else
            {
                //Cannot move only on the X
                Debug.Log("cant move x");
                // Attempt obly Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //Can move only on the X

                    moveDir = moveDirZ;
                    
                }
                else
                {
                    Debug.Log("cant move anyd");
                    //Cannot move in any Direction
                }

            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }


        isRunning = moveDir != Vector3.zero;
        if (isRunning)
        {
            state = State.Running;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }
        else if(state != State.Atacking)
        {
            state=State.Idle; 
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

}
