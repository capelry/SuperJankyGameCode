using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components and object properties
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Animator animator;

    // inputs
    [SerializeField] private string right;
    [SerializeField] private string left;
    [SerializeField] private string up;
    [SerializeField] private string down;

    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;
    [SerializeField] private float knockBack;
    private bool secondJump;
    private bool onGround;
    private float horizontalInput;
    private bool notStunned;
    private float stunCooldown;
    private float fallForceScalar;
    private float smallJumpScalar;

    private void Awake()
    {
        // get component references
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        notStunned = true;
        stunCooldown = 0f;
        secondJump = true;
        onGround = false;
        horizontalInput = 0;
        fallForceScalar = 2.2f;
        smallJumpScalar = 2f;
    }

    // Update is called once per frame
    private void Update()
    {
        if(stunCooldown > 0f)
        {
            notStunned = false;
            stunCooldown -= Time.deltaTime;
        }
        else
        {
            notStunned = true;
        }

        // get movement input from keys     
        if(Input.GetKey(right) && notStunned)
        {
            horizontalInput = 1;
        }
        else if (Input.GetKey(left) && notStunned)
        {
            horizontalInput = -1;
        }
        else
        {
            horizontalInput = 0;
        }

        //TODO: fully replace usage of this: horizontalInput = Input.GetAxis("Horizontal");
        onGround = OnGround();

        // Running
        // Set animator parameters
        animator.SetBool("run", horizontalInput != 0);
        // Apply run input to rigidbody
        rigidBody.velocity = new Vector2(horizontalInput * speed, rigidBody.velocity.y);

        // In air
        animator.SetBool("grounded", onGround);

        // Flip sprite when changing direction
        if (rigidBody.velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(5, 5, 1);
        }
        else if (rigidBody.velocity.x < -0.01f)
        {
            //TODO: change this and above so scale is consistent from editor
            transform.localScale = new Vector3(-5, 5, 1);
        }

        // Jump Logic
        if(onGround)
        {
            secondJump = true;
        }
        // trigger jump
        if (Input.GetKeyDown(up) && notStunned)
        {
            Jump();
        }
        // faster fall
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += new Vector2(0, Physics2D.gravity.y * (fallForceScalar - 1) * Time.deltaTime);
        }
        // player increase fall speed
        if (rigidBody.velocity.y != 0 && Input.GetKey(down))
        {
            rigidBody.velocity += new Vector2(0, Physics2D.gravity.y * (fallForceScalar - 1) * Time.deltaTime);
        }
        // smaller jump when button off
        else if (rigidBody.velocity.y > 0 && !Input.GetKey(up))
        {
            rigidBody.velocity += new Vector2(0, Physics2D.gravity.y * (smallJumpScalar - 1) * Time.deltaTime);
        }

        // Super janky quick function
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void Jump()
    {
        if (onGround)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
        }
        else if (OnWall())
        {
            rigidBody.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
            // cannot double jump from wall
            secondJump = false;
        }
        else if (secondJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
            // no more jumps 
            secondJump = false;
        }

    }

    private bool OnGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return true;
    }

    public void TakeStun(float _stun, float knockBackDirection)
    {
        stunCooldown = _stun;
        rigidBody.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * knockBackDirection * knockBack, 0));
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}