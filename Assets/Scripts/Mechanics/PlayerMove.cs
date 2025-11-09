using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    [SerializeField] private readonly float speed = 10f;

    private Vector2 inputVector = new(0, 0);

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReceiveInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReceiveInput()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Debug.Log(inputVector);
    }

    private void Move()
    {
        rigidbody2D.linearVelocity = inputVector.normalized * speed;
    }
}
