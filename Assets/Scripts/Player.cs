using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Singleton instance of the Player class
    public static Player Instance { get; private set; }

    // Event that is triggered when the selected counter changes
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        // Custom event arguments class for the selected counter change event (no additional data needed)
    }

    // Event that is triggered when the player's state changes
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    // Speed at which the player moves
    [SerializeField] private float moveSpeed = 7f;

    // Speed at which the player rotates
    [SerializeField] float rotateSpeed = 20f;

    // Reference to the GameInput script for receiving player input
    [SerializeField] private GameInput gameInput;

    // Player's capsule collider radius
    [SerializeField] private float playerRadius = 0.4f;

    // Player's capsule collider height
    [SerializeField] private float playerHeight = 1.0f;

    // Flag to track if the player is running
    private bool isRunning = false;

    // Current state of the player (Idle, Running, Attacking)
    private State state;

    // Reference to the Archer component attached to the player
    private Archer archer;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Singleton pattern: Ensure there is only one instance of Player in the scene
        if (Instance != null)
        {
            Debug.Log("There is more than one Player instance");
        }
        Instance = this;
    }

    // Called before the first frame update
    private void Start()
    {
        // Get the Archer component attached to the player
        archer = GetComponent<Archer>();

        // Find the GameInput script in the scene
        gameInput = FindObjectOfType<GameInput>();
    }

    // Called once per frame
    private void Update()
    {
        // If the player is dead, do not process any input
        if (GetComponent<Health>().IsDead())
        {
            return;
        }

        // Handle player movement, shooting, and rotation
        HandleMovement();
        HandleShooting();
        HandleRotation();
    }

    // Handle player rotation based on the current state
    private void HandleRotation()
    {
        switch (state)
        {
            case State.Idle:
                // If there is a current target for the Archer, rotate towards the target's position
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
                // If there is a current target for the Archer, rotate towards the target's position
                if (archer.GetCurrentTarget() != null)
                {
                    Vector3 baseLook = archer.GetCurrentTarget().transform.position - transform.position;
                    Vector3 lookDirection = new Vector3(baseLook.x, 0, baseLook.z);
                    transform.forward = Vector3.Slerp(transform.forward, lookDirection, Time.deltaTime * rotateSpeed);
                }
                break;
            default:
                break;
        }
    }

    // Handle player shooting based on the current state
    private void HandleShooting()
    {
        // If the player is in the Idle state, change to Attacking state and trigger the state change event
        if (state == State.Idle)
        {
            state = State.Atacking;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }
    }

    // Check if the player is walking (running)
    public bool IsWalking()
    {
        return isRunning;
    }

    // Handle player movement
    private void HandleMovement()
    {
        // Get the input vector for movement from the GameInput script
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        // Calculate the move distance based on the move speed and the elapsed time
        float moveDistance = Time.deltaTime * moveSpeed;

        // Check if the player can move in the desired direction without colliding with obstacles
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // If the player cannot move in the desired direction, attempt movement along the X and Z axes separately
        if (!canMove)
        {
            // Attempt only X Movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            // If the player can move only along the X-axis, update the move direction
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X-axis, attempt only Z movement
                Debug.Log("Can't move along X-axis");

                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                // If the player can move only along the Z-axis, update the move direction
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    Debug.Log("Can't move in any direction");
                    // Cannot move in any direction due to obstacles
                }
            }
        }

        // If the player can move, update the position based on the move direction and distance
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // Update the isRunning flag based on the move direction
        isRunning = moveDir != Vector3.zero;

        // If the player is running, change the state to Running and trigger the state change event
        if (isRunning)
        {
            state = State.Running;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }
        // If the player is not running and not attacking, change the state to Idle and trigger the state change event
        else if (state != State.Atacking)
        {
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
        }

        // Smoothly rotate the player towards the move direction
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
