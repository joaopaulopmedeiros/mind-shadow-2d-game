using UnityEngine;

public class FrogController : Possessable
{
    public float moveSpeed = 3f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isPossessed) return;

        // Movimento horizontal
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // Pulo
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Liberar posse
        if (Input.GetKeyDown(KeyCode.R))
        {
            ghost.ReleasePossession();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    public override void OnPossessed(PlayerController controller)
    {
        base.OnPossessed(controller);
        // Aqui voc� pode trocar sprite, som, etc.
    }

    public override void OnReleased()
    {
        base.OnReleased();
        // Volta para estado "neutro"
    }
}
