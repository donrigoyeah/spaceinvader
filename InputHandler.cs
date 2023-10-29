using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Player Inputs")]
    public bool shootFlag;
    public bool enterFlag;
    public bool exitFlag;
    public float horizontal;
    public float vertical;
    public float moveAmount;

    PlayerControls inputActions;

    Vector2 movementInput;
    Vector2 cameraInput;

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerInput.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            //inputActions.PlayerMovement.Camera.performed += inputActions => cameraInput = inputActions.ReadValue<Vector2>();
            //inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleShootInput(delta);
        HandleEscInput(delta);
        HandleEnterInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        //cameraX = cameraInput.x;
        //cameraY = cameraInput.y;
    }



    private void HandleShootInput(float delta)
    {
        shootFlag = inputActions.PlayerInput.Shoot.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

    }

    private void HandleEnterInput(float delta)
    {
        enterFlag = inputActions.PlayerInput.Enter.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

    }
    private void HandleEscInput(float delta)
    {
        exitFlag = inputActions.PlayerInput.Exit.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
    }
}
