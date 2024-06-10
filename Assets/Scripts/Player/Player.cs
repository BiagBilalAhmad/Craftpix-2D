using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public float health = 50f;
    public Image healthImg;
    private float maxHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isFacingRight = true;
    private float moveInput;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool wasGrounded = true;

    [Header("Double Jump Settings")]
    public bool canDoubleJump = false;
    public int maxJumps = 2;
    private int jumpCount;
    private bool isJumping;

    [Header("Attack")]
    public GameObject arrowPrefab;
    public Transform shootPoint;


    [HideInInspector] public Rigidbody2D _rigidBody;
    [HideInInspector] public Animator _animator;

    // STATIC STRINGS
    private const string IDLE_ANIM = "Idle";
    private const string RUN_ANIM = "Run";
    private const string JUMP_ANIM = "Jump";
    private const string FALL_ANIM = "Fall";
    private const string LAND_ANIM = "Land";
    private const string SHOOT_ANIM = "Shoot";
    private const string MELEE1_ANIM = "Melee 1";
    private const string MELEE2_ANIM = "Melee 2";
    private const string TAKE_DAMAGE_ANIM = "Take Damage";
    private const string DEATH_ANIM = "Death";

    void Start()
    {
        maxHealth = health;

        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get horizontal movement input
        moveInput = Input.GetAxis("Horizontal");

        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Flip player
        if (moveInput > 0) // Facing Right
        {
            isFacingRight = false;
            Flip();
        }
        else if (moveInput < 0) // Facing Left
        {
            isFacingRight = true;
            Flip();
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumps))
        {
            isJumping = true;
        }

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        _rigidBody.velocity = new Vector2(moveInput * moveSpeed, _rigidBody.velocity.y);

        // Apply jump force
        if (isJumping)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);
            jumpCount++;
            isJumping = false;
        }
    }

    public void ShootArrow()
    {

        Arrow arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.Fire(!isFacingRight);
    }

    public void ShootFireball()
    {
        PlayerFireball fireball = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity).GetComponent<PlayerFireball>();
        fireball.Fire(!isFacingRight);
    }

    void UpdateAnimations()
    {
        _animator.SetBool(JUMP_ANIM, false);
        _animator.SetBool(FALL_ANIM, false);
        _animator.SetBool(IDLE_ANIM, false);
        _animator.SetBool(RUN_ANIM, false);

        if (!isGrounded)
        {
            if (_rigidBody.velocity.y > 0)
            {
                _animator.SetBool(JUMP_ANIM, true);
            }
            else
            {

                _animator.SetBool(FALL_ANIM, true);
            }
        }
        else if (Mathf.Abs(moveInput) > 0.1f)
        {
            _animator.SetBool(RUN_ANIM, true);
        }
        else
        {
            _animator.SetBool(IDLE_ANIM, true);
        }

        //if (isGrounded && !wasGrounded)
        //{
        //    _animator.SetTrigger(LAND_ANIM);
        //}

        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger(SHOOT_ANIM);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            int randAtk = UnityEngine.Random.Range(1, 3);

            if(randAtk == 1)
                _animator.SetTrigger(MELEE1_ANIM);
            else
                _animator.SetTrigger(MELEE2_ANIM);
        }

        //wasGrounded = isGrounded;
    }

    void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (isFacingRight)
            rotation.y = 180f;
        else
            rotation.y = 0f;

        transform.eulerAngles = rotation;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        healthImg.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            this.enabled = false;
            _rigidBody.isKinematic = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
            _animator.SetTrigger(DEATH_ANIM);
            StartCoroutine(CallDeath(3f));
            return;
        }

        _animator.SetTrigger(TAKE_DAMAGE_ANIM);
    }

    private IEnumerator CallDeath(float delay)
    {
        this.enabled = false;
        yield return new WaitForSeconds(delay/2);
        GameManager.Instance.ShowGameOver();
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(10f);
        }

        if (collision.gameObject.CompareTag("SpikeBall"))
        {
            StartCoroutine(CallDeath(3f));
        }
    }

    void OnDrawGizmos()
    {
        // Draw the ground check radius in the editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
