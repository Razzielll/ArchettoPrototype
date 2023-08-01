using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAI : MonoBehaviour
{
    // Speed at which the enemy moves
    [SerializeField] private float moveSpeed;

    // Height of the enemy (used for movement)
    [SerializeField] private float enemyHeigh;

    // Speed at which the enemy rotates
    [SerializeField] private float rotateSpeed;

    // Radius of the enemy's collider
    [SerializeField] private float enemyRadius;

    // Layer mask for obstacles to detect while moving
    [SerializeField] LayerMask obstaclesLayerMask;

    // Event that is triggered when the state of the enemy changes
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    // Custom event arguments class for the state change event
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    // Transform of the player (the target)
    protected Transform playerTransform;

    // Current state of the enemy
    private State state;

    // Flag to indicate if the enemy is running (moving)
    private bool isRunning;

    // Called before the first frame update
    protected virtual void Start()
    {
        // Find the player object and set the initial state to Running
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.Running;
    }

    // Called once per frame
    protected virtual void Update()
    {
        // If the enemy is dead, return early and do nothing
        if (GetComponent<Health>().IsDead())
        {
            return;
        }

        // Handle enemy movement towards the player
        HandleMovement(playerTransform.position - transform.position, 0f);
        // Handle enemy shooting
        HandleShooting();
        // Handle enemy rotation to face the player
        HandleRotation();
    }

    // Handle the rotation of the enemy based on its state
    protected virtual void HandleRotation()
    {
        switch (state)
        {
            case State.Idle:
                // If the player exists, rotate the enemy to face the player
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
                // If the player exists, rotate the enemy to face the player
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

    // Handle the enemy's shooting logic
    protected virtual void HandleShooting()
    {
        if (state == State.Idle)
        {
            // Change the state to Attacking if the enemy starts shooting
            state = State.Atacking;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }
    }

    // Set the state of the enemy
    public void SetState(State state)
    {
        this.state = state;
    }

    // Check if the enemy is walking (moving)
    public bool IsWalking()
    {
        return isRunning;
    }

    // Handle the enemy's movement based on the given direction and floating height
    protected virtual void HandleMovement(Vector3 direction, float floatingHeight)
    {
        // If the enemy is in Idle or Attacking state, return early and do not move
        if (state == State.Idle || state == State.Atacking)
        {
            return;
        }

        // Normalize the movement direction and calculate the move distance based on the moveSpeed and Time.deltaTime
        Vector3 moveDir = direction.normalized;
        float moveDistance = Time.deltaTime * moveSpeed;

        // Perform a capsule cast to check for obstacles in the movement direction and determine if the enemy can move
        bool canMove = !Physics.CapsuleCast(transform.position + Vector3.up * floatingHeight, transform.position + Vector3.up * enemyHeigh, enemyRadius, moveDir, moveDistance, obstaclesLayerMask);

        // If the enemy cannot move in the current direction, attempt to move only in the X or Z direction
        if (!canMove)
        {
            // Attempt only X Movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position + Vector3.up * floatingHeight, transform.position + Vector3.up * enemyHeigh, enemyRadius, moveDirX, moveDistance, obstaclesLayerMask);

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
                UpdateMoveDirection();
            }
            else
            {
                // Cannot move only on the X
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position + Vector3.up * floatingHeight, transform.position + Vector3.up * enemyHeigh, enemyRadius, moveDirZ, moveDistance, obstaclesLayerMask);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                    UpdateMoveDirection();
                }
                else
                {
                    // Cannot move in any Direction
                    UpdateMoveDirection();
                }
            }
        }

        // If the enemy can move, update its position
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // Update the isRunning flag based on the movement direction (non-zero indicates running)
        isRunning = moveDir != Vector3.zero;

        // If the enemy is running, set the state to Running and invoke the state change event
        if (isRunning)
        {
            state = State.Running;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }
        // If the enemy is not running and not in Attacking state, set the state to Idle and invoke the state change event
        else if (state != State.Atacking)
        {
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }

        // Smoothly rotate the enemy's forward direction towards the move direction
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Method to be overridden by child classes to update the move direction
    protected virtual void UpdateMoveDirection()
    {
        // Implement in child classes if needed
    }
}
