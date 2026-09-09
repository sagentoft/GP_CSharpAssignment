using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction restartAction;
    
    [SerializeField]float moveSpeed = 100.0f;


    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        restartAction = InputSystem.actions.FindAction("Restart");
    }

    private void OnEnable()
    {
        restartAction.performed += RestartActionOnperformed;
    }

    private void OnDisable()
    {
        restartAction.performed -= RestartActionOnperformed;
    }

    private void RestartActionOnperformed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.IsGameOver)
        {
            GameManager.Instance.RestartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        
        transform.Translate(movement * (moveSpeed * Time.deltaTime));
        
    }
}
