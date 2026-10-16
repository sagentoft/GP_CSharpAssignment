using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector3 FacingDirection { get; private set; } = Vector3.forward;
    
    private InputActions inputActions;
    
    [SerializeField]float moveSpeed = 100.0f;
    [SerializeField] private PauseMenu pauseMenu;


    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        
        inputActions.Player.Restart.performed += RestartActionOnperformed;
        inputActions.Player.Pause.performed += PauseActionOnPerformed;
    }
    
    private void OnDisable()
    {
        inputActions.Player.Restart.performed -= RestartActionOnperformed;
        inputActions.Player.Pause.performed -= PauseActionOnPerformed;

        inputActions.Player.Disable();
    }

    private void RestartActionOnperformed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.IsGameOver)
        {
            GameManager.Instance.RestartGame();
        }
    }
    
    private void PauseActionOnPerformed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.IsGameOver)
            return;

        pauseMenu.TogglePause();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        
        if (movement.sqrMagnitude > 0)
        {
            FacingDirection = movement.normalized;
        }
        
        transform.Translate(movement * (moveSpeed * Time.deltaTime), Space.World);
        
    }

    public void ModifyMoveSpeed(float multiplier)
    {
        moveSpeed *= multiplier;
    }
}
