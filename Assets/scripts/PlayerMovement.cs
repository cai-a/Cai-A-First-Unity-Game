using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 desiredVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;

        float currentSpeed = speed;

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            currentSpeed = sprintSpeed;
        }

        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            movement.y += 1;
        }

        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            movement.y -= 1;
        }

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            movement.x -= 1;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            movement.x += 1;
        }

        desiredVelocity = movement.normalized * currentSpeed;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = desiredVelocity;
    }

    private void OnDisable()
    {
        desiredVelocity = Vector2.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}