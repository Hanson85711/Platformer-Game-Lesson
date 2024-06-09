using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Forces")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    [Header("Collision Info")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private bool isGrounded;

    private Animator animator; 
    private bool canDoubleJump;
    private Rigidbody2D rb;
    private float xMovementInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = rb.velocity.x != 0; 

        animator.SetBool("isMoving", isMoving);

        JumpButton();
        CollisionCheck();
        xMovementInput = Input.GetAxis("Horizontal");
        
        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }


    void FixedUpdate()
    {
        Move();
    }


    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed * xMovementInput, rb.velocity.y);
    }

    private void JumpButton()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            canDoubleJump = false;
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
