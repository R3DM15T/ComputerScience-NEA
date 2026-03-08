using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] bool canDoubleJump = false;
    [SerializeField] bool hasJumped = false;
    [SerializeField] bool hasDoubleJumped = false;
    [SerializeField] float spikeDamage;
    [SerializeField] private LayerMask passThroughLayers = LayerMask.GetMask();

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f;
    private float dashCounter, dashCoolCounter;
    private bool isDashingThroughObjects = false;

    Vector2 moveInput;
    Rigidbody2D rigidBody;
    Animator animation;
    private Camera cam;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    [SerializeField] GameObject doublejumpParticles;
    [SerializeField] GameObject dashParticles;
    [SerializeField] GameObject feet;
    [SerializeField] GameObject dashParticlesArea;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        activeMoveSpeed = runSpeed;
        cam = Camera.main;
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.freezeRotation = true;
        animation = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();


    }

    void FixedUpdate()
    {
        SpriteRun();
        SpriteFlip();
        IsGrounded();
        PreventWallStick();
        Hazards();


        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                DisableDashPassThrough();
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    bool IsGrounded()
    {
        float rayLength = 0.1f;
        Vector2 rayOrigin = feetCollider.bounds.center;
        Vector2 rayDirection = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, LayerMask.GetMask("Platforms"));
        return hit.collider != null;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            bool isFeetGrounded = IsGrounded();
            if (isFeetGrounded)
            {
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpSpeed);
                hasJumped = true;
                hasDoubleJumped = false;

            }
            else if (canDoubleJump && hasJumped && !hasDoubleJumped)
            {
                Instantiate(doublejumpParticles, feet.transform.position, feet.transform.rotation);
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpSpeed);
                hasDoubleJumped = true;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (dashCounter <= 0 && dashCoolCounter <= 0)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 dashDirection = (mousePos - transform.position).normalized;
            Instantiate(dashParticles, dashParticlesArea.transform.position, dashParticlesArea.transform.rotation);
            //give invinsibility when added health system
            //turnTransparent(); will be visual feedback when i do it
            EnableDashPassThrough();
            rigidBody.linearVelocity = dashDirection * dashSpeed; //apply force

            dashCounter = dashLength;
        }

    }

    void SpriteRun()
    {
        if (dashCounter <= 0)
        {
            Vector2 playerVelocity = new Vector2(moveInput.x * activeMoveSpeed, rigidBody.linearVelocity.y);
            rigidBody.linearVelocity = playerVelocity;
        }
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.linearVelocity.x) > Mathf.Epsilon;
        animation.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void SpriteFlip()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(1.8f * Mathf.Sign(rigidBody.linearVelocity.x), 1.8f);

        }
    }

    void Hazards()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            //take damage

        }
    }


    void PreventWallStick()
    {
        bool isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isTouchingWall = bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isGrounded && isTouchingWall && Mathf.Abs(moveInput.x) > 0.1f)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, -Mathf.Abs(jumpSpeed / 2));
        }
    }


    private void EnableDashPassThrough()
    {
        isDashingThroughObjects = true;


        for (int i = 0; i < 32; i++) //getting all layers assigned in the array
        {
            if ((passThroughLayers.value & (1 << i)) != 0)
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, i, true);
            }
        }
    }

    private void DisableDashPassThrough()
    {
        if (!isDashingThroughObjects) return;

        isDashingThroughObjects = false;


        for (int i = 0; i < 32; i++)
        {
            if ((passThroughLayers.value & (1 << i)) != 0)
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, i, false);
            }
        }
    }

}

