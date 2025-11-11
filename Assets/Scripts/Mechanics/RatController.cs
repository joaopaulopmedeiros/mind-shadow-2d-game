using UnityEngine;

public class RatController : Possessable
{
    public float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isPossessed) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveX));

        if (moveX > 0)
            sr.flipX = false;
        else if (moveX < 0)
            sr.flipX = true;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ghost.ReleasePossession();
        }
    }

    public override void OnPossessed(PlayerController controller)
    {
        base.OnPossessed(controller);
    }

    public override void OnReleased()
    {
        base.OnReleased();
    }
}
