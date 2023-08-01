
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAI : MonoBehaviour
{
     

    [SerializeField] private float moveSpeed;
    [SerializeField] private float enemyHeigh;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float enemyRadius;
    [SerializeField] LayerMask obstaclesLayerMask;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    protected Transform playerTransform;
    private State state;
    private bool isRunning;

    protected virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        state = State.Running;
    }

    protected virtual void Update()
    {
        if (GetComponent<Health>().IsDead())
        {
            return;
        }
        HandleMovement(playerTransform.position - transform.position, 0f);
        HandleShooting();
        HandleRotation();
    }

    protected virtual void HandleRotation()
    {
        switch (state)
        {
            case State.Idle:
                if (playerTransform != null)
                {
                    Vector3 baseLook = playerTransform.position - transform.position;
                    Vector3 lookDirection = new Vector3(baseLook.x, 0, baseLook.z);
                    transform.forward = Vector3.Slerp(transform.forward, lookDirection, Time.deltaTime * rotateSpeed);
                }
                break;
            case State.Running:
                break;

            case State.Atacking:
                if (playerTransform != null)
                {
                    Vector3 baseLook = playerTransform.position - transform.position;
                    Vector3 lookDirection = new Vector3(baseLook.x, 0, baseLook.z);
                    transform.forward = Vector3.Slerp(transform.forward, lookDirection, Time.deltaTime * rotateSpeed);
                }

                break;
            default:
                break;
        }
    }

    protected virtual void HandleShooting()
    {
        if (state == State.Idle)
        {
            state = State.Atacking;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }

    }

    public void SetState(State state)
    {
        this.state = state;
    }

    public bool IsWalking()
    {
        return isRunning;
    }
    protected virtual void HandleMovement(Vector3 direction, float floatingHeight)
    {
        if(state == State.Idle || state == State.Atacking)
        {
            return;
        }
        // Vector2 inputVector = new Vector2();
        Vector3 moveDir = direction.normalized;
        float moveDistance = Time.deltaTime * moveSpeed;

        bool canMove = !Physics.CapsuleCast(transform.position + Vector3.up * floatingHeight, transform.position + Vector3.up * enemyHeigh, enemyRadius, moveDir, moveDistance, obstaclesLayerMask);

        if (!canMove)
        {
            //Attempt only X Movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position + Vector3.up * floatingHeight, transform.position + Vector3.up * enemyHeigh, enemyRadius, moveDirX, moveDistance, obstaclesLayerMask);
            if (canMove)
            {
                //Can move only on the X
                moveDir = moveDirX;
                UpdateMoveDirection();
            }
            else
            {
                //Cannot move only on the X
               // Debug.Log("cant move x");
                // Attempt obly Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position + Vector3.up * floatingHeight, transform.position + Vector3.up * enemyHeigh, enemyRadius, moveDirZ, moveDistance, obstaclesLayerMask);
                if (canMove)
                {
                    //Can move only on the Z

                    moveDir = moveDirZ;
                    UpdateMoveDirection();
                }
                else
                {
                   // Debug.Log("cant move anyd");
                    //Cannot move in any Direction
                    UpdateMoveDirection();
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
        else if (state != State.Atacking)
        {
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    protected virtual void UpdateMoveDirection()
    {
        
    }
}
