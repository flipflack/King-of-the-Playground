using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer;

    private Vector2 movementInput;

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = movementInput.normalized * moveSpeed;
    }
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        if (movementInput.x <0)
        {
            spriteRenderer.flipX = true;
        }
        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
