using UnityEngine;

public class HiFiPlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("LeftJoystickHorizontal"), Input.GetAxisRaw("LeftJoystickVertical"));
        moveVelocity = moveInput.normalized * speed;

        print(moveInput);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}

