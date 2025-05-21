using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float turnSpeed = 900f;
    public float moveSpeed = 15f;
    private Vector3 input;
    private Rigidbody rb;

    [Header("Options")]
    public bool canMove = true;
    public bool isMovementIsometric;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (canMove)
        {
            // Get input from the old Input system (WASD / arrow keys)
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            input = new Vector3(h, 0, v).normalized;
        }
        else
        {
            input = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Look();
            Move();
        }
    }

    void Look()
    {
        if (input != Vector3.zero)
        {
            int moveDegrees = isMovementIsometric ? 45 : 0;
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, moveDegrees, 0));
            var skewedInput = matrix.MultiplyPoint3x4(input);

            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    void Move()
    {
        if (input.magnitude > 0)
        {
            rb.MovePosition(transform.position + (transform.forward * input.magnitude * moveSpeed * Time.deltaTime));
        }
    }

    void EnableMovement()
    {
        canMove = true;
    }

    public void Knockback(Vector3 direction, float force)
    {
        canMove = false; // freeze movement during knockback
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Invoke(nameof(EnableMovement), 0.2f); // resume after delay
    }
}
