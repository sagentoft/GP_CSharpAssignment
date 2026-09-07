using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    
    [SerializeField]float moveSpeed = 100.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        transform.Translate(move.x * moveSpeed * Time.deltaTime, 0, move.y * moveSpeed * Time.deltaTime);
        
    }
}
