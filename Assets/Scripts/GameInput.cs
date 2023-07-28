
using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction; 
    public event EventHandler OnInteractAlternateAction;
    [SerializeField] FloatingJoystick joystick;

    private Vector3 rawInput;
   // private PlayerInputActions playerInputActions;
    private void Awake()
    {
        

    }


    public Vector2 GetMovementVectorNormalized()
    {
        // Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        rawInput = new Vector2(-joystick.Horizontal, -joystick.Vertical);
       // inputVector = inputVector.normalized;
        return rawInput;
    }
}
