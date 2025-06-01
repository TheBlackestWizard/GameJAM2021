using Mono.Cecil;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;
    public InputActionReference move;
    public InputActionReference fire;

    public static bool fired = false;

    Vector2 movement;


    private void OnEnable()
    {
        //playerControls.Enable();
        fire.action.started += Fire;
        
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        fired = true;
    }

    private void OnDisable()
    {
        //playerControls.Disable();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = move.action.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2 (movement.x * moveSpeed * Time.fixedDeltaTime, movement.y * moveSpeed * Time.fixedDeltaTime);
    }

}
