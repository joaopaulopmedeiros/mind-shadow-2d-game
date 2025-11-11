using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;
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
    public CinemachineCamera virtualCamera;
    private SpriteRenderer sr;


    private Vector2 moveInput;


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Ghost"),
            LayerMask.NameToLayer("Possessable"),
            true
        );

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Possessable"),
            LayerMask.NameToLayer("Possessable"),
            true
        );

        if (virtualCamera != null)
            virtualCamera.Follow = transform;
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

        if (moveX > 0)
            sr.flipX = true;
        else if (moveX < 0)
            sr.flipX = false;
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
                Debug.Log("Chegou aqui!");
            DeathCharacter death;
            if (closest.TryGetComponent<DeathCharacter>(out death) && death.preventPossession)
            {
                Debug.Log("Chegou aqui! 3");

                death.TriggerDialogue(this);
                return;
            }
            currentPossession = closest.gameObject;
            if (virtualCamera != null)
                virtualCamera.Follow = closest.transform;
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

        if (virtualCamera != null)
            virtualCamera.Follow = transform;
    }
}
