using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    [SerializeField] private readonly float speed = 5f; 

    public CircleCollider2D possessRange;
    private bool isPossessing = false;
    private GameObject currentPossession;
    private List<Possessable> nearbyPossessables = new List<Possessable>();
    private Vector3 lastPossessedPosition;


    private Vector2 moveInput;


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); 

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Ghost"),
            LayerMask.NameToLayer("Possessable"),
            true
        );
    }

    private void Update()
    {
        if (isPossessing) return;

        Move();

        if (Input.GetKeyDown(KeyCode.E))
        {
            PossessClosest();
        }
    }

    void FixedUpdate()
    {
        if (!isPossessing)
        {
            rigidbody2D.linearVelocity = moveInput * speed;
        }
        else
        {
            rigidbody2D.linearVelocity = Vector2.zero;
        }
         
        if (isPossessing && Input.GetKeyDown(KeyCode.R))
        {
            ReleasePossession();
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); 
        moveInput = new Vector2(moveX, 0).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Possessable p = other.GetComponent<Possessable>();
        if (p != null && !nearbyPossessables.Contains(p))
        {
            nearbyPossessables.Add(p);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Possessable p = other.GetComponent<Possessable>();
        if (p != null && nearbyPossessables.Contains(p))
        {
            nearbyPossessables.Remove(p);
        }
    }

    private void PossessClosest()
    {
        if (nearbyPossessables.Count == 0) return;

        Possessable closest = null;
        float minDist = Mathf.Infinity;

        foreach (var p in nearbyPossessables)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p;
            }
        }

        if (closest != null)
        {
            currentPossession = closest.gameObject;
            closest.OnPossessed(this); 
            gameObject.SetActive(false);
            isPossessing = true;
        }
    }

    public void ReleasePossession()
    {
        if (currentPossession == null) return;
         
        lastPossessedPosition = currentPossession.transform.position;
         
        currentPossession.GetComponent<Possessable>().OnReleased();
         
        transform.position = lastPossessedPosition;

        gameObject.SetActive(true);
         
        isPossessing = false;
        currentPossession = null;
    }
}
