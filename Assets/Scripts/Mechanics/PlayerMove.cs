using System.IO;
using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    [SerializeField] private readonly float speed = 5f;
    [SerializeField] private float jumpForce = 8f;

    private float direcao;
    private bool isJumping;

    private Vector2 inputVector = new(0, 0);

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        isJumping = false;
    }

    private void Update()
    {
        Move();
    }

    //private void FixedUpdate()
    //{
    //    Move();
    //}

    private void ReceiveInput()
    {
        

        Debug.Log(inputVector);
    }

    private void Move()
    {
        //inputVector = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rigidbody2D.linearVelocity.y);

        //rigidbody2D.linearVelocity = inputVector.normalized;


        direcao = Input.GetAxisRaw("Horizontal");
        rigidbody2D.linearVelocity = new Vector2(direcao * speed, rigidbody2D.linearVelocityY);
        


        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rigidbody2D.linearVelocityY = jumpForce;
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            isJumping = false;
        }
    }
}
