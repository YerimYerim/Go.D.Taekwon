using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public Animator playerAnimator;
    [SerializeField] BoxCollider2D groundCheckCollider;
    [SerializeField] BoxCollider2D ceilingCheckCollider;

    private float boxCastDistance = 0.01f;

    [Header("Dust Prefabs")]
    [SerializeField] GameObject dustSlidePrefab;
    [SerializeField] GameObject landDustPrefab;
    [SerializeField] GameObject jumpDustPrefab;
    GameObject jumpDustClone;

    private Vector3 dustSlideOffset = new Vector3(-1.2f, -1.04f, 0);
    private Vector3 landDustOffset = new Vector3(-0.06f, -1.3f, 0);
    private Vector3 jumpDustOffset = new Vector3(0, -1.5f, 0);
    private bool slideDust = false;

    [Header("Projectile Prefab")]
    [SerializeField] GameObject projectile;
    public GameObject bullet360;

    public Vector3 projectileOffset;
    public Vector3 projectileOffsetCrouching;

    [Header("Animators")]
    public Animator attackAnimator;
    public Animator chargeEffectAnimator;

    [Header("Movement")]
    public bool canMove = true;
    private Vector2 leftStickInput;
    private Vector2 keyboardDirections;
    public float joystickDeadzone;
    [SerializeField] private float playerGravity;
    [SerializeField] private float runSpeed;
    private bool facingRight = true;

    [Header("Ledges")]
    public bool canLedgeGrab;
    private bool isHanging;
    public Vector2 hangOffset;

    [Header("Wall Slide")]
    public bool canWallSlide;
    private bool isWallSliding;
    public float wallSlideSpeed;

    [Header("Wall Jump")]
    public bool canWallJump;
    private bool isWallJumping;
    public float wallJumpDuration;
    public float wallJumpPowerX;
    public float wallJumpPowerY;

    [Header("Crouch")]
    public bool canCrouch;
    private bool isCrouching = false;
    private bool isRising;
    private float riseWait = 0f;

    [Header("Jump")]
    public bool canDoubleJump = true;
    private bool doubleJump = false;
    private bool enableLandDust = false;
    private bool isJumping;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpForceOnRelease;
    [SerializeField] private float fallSpeedMultiplier;
    public float maxGravityFallValue;
    [HideInInspector] public bool falling = false;

    [Header("Dash")]
    public bool canDash = true;
    [HideInInspector] public bool isDashing = false;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    [Header("Slide")]
    public bool canSlide = true;
    [HideInInspector] public bool isSliding = false;
    [SerializeField] private float slideForce;
    [SerializeField] private float slideDuration;
    [SerializeField] private float slideCooldown;

    [Header("Melee Attack")]
    public bool canAttack;
    public bool canChargeAttack;
    public float chargeTime;
    public bool canBlock;
    [HideInInspector] public bool isBlocking;
    [HideInInspector] public bool attackInitiated = false;
    [HideInInspector] public bool isAttacking = false;


    [Header("Gun Attack")]
    public bool canWeaponSwap;
    public bool canFire;
    public bool canFireCrouching;
    public float projectileSpeed;
    public float fireCooldown;
    public bool canFreeAim;
    public float freeAimBulletSpeed;
    public float freeAimFireRatePerSecond;

    private bool shooting360;

    //Seperated so that keyboard and joypad aiming controls don't interfere with each other (both control schemes can be used interchangeably at run time)
    [HideInInspector] public bool aimingKB;
    [HideInInspector] public bool aimingJoy;

    private PlayerHealth playerHealth;

    [Header("Knockback")]
    public bool knockbackWhenHit;
    [HideInInspector] public bool playerHit;
    public float knockbackStrength;
    public float knockbackDuration;
    private bool isKnockedBack;

    [Header("Collision Checks")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    private float leftTriggerInput;
    [HideInInspector] float rightTriggerInput;
    [HideInInspector] public int weapon;


    private float buttonHoldTime = 0;

    public LaserSight laserSight;


    // Start is called before the first frame update
    void Start()
    {

        ResetGravity();

        playerHealth = gameObject.GetComponent<PlayerHealth>();

        attackAnimator = GameObject.Find("Attack Melee").GetComponent<Animator>();

        weapon = 1;
        facingRight = true;

    }

    public void FlipPlayerX()
    {
        //Player Flip
        Vector2 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        //Dust Flip
        dustSlidePrefab.transform.localScale = currentScale;

        facingRight = !facingRight;

    }


    public bool IsGrounded()
    {
        if (canLedgeGrab)
        {
            return Physics2D.BoxCast(groundCheckCollider.bounds.center, groundCheckCollider.size, 0f, Vector2.down, boxCastDistance, (groundLayer + wallLayer));
        }
        else 
        {
            return false;
        }
    }


    private bool CeilingCheck()
    {

        return Physics2D.BoxCast(ceilingCheckCollider.bounds.center, ceilingCheckCollider.size, 0f, Vector2.up, boxCastDistance, wallLayer);

    }

    public bool LedgeCheck()
    {
        if (!facingRight)
        {
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2f), -Vector2.right, 1.4f, (groundLayer + wallLayer));
        }
        else
        {
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2f), Vector2.right, 1.4f, (groundLayer + wallLayer));
        }
    }

    public bool WallCheck()
    {
        if (!facingRight)
        {
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.9f), -Vector2.right, 1.4f, (groundLayer + wallLayer));
        }
        else 
        {
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.9f), Vector2.right, 1.4f, (groundLayer + wallLayer));
        }
    }

    public void MovementStop()
    {
        canMove = false;
        playerRb.velocity = Vector2.zero;
        isDashing = false;
        isSliding = false;
        playerAnimator.SetBool("Running", false);

    }

    public void MovementResume()
    {
        canMove = true;
    }


    // Update is called once per frame
    void Update()
    {

        if (isDashing)
        {
            return;
        }

        //Directional inputs, prevents keyboard and controller inputs from interfering with each other. Both can be used.
        leftStickInput = new Vector2(Input.GetAxisRaw("HorizontalJ"), Input.GetAxisRaw("VerticalJ"));
        keyboardDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (keyboardDirections.magnitude != 0)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            leftStickInput.x = 0;
            leftStickInput.y = 0;

        }
        else if (leftStickInput.magnitude != 0)
        {
            horizontalInput = Input.GetAxis("HorizontalJ");
            verticalInput = Input.GetAxis("VerticalJ");
        }

        //Deadzone with proper radial area
        if (leftStickInput.magnitude < joystickDeadzone && keyboardDirections.magnitude == 0)
        {
            horizontalInput = 0;
            verticalInput = 0;
            leftStickInput.x = 0;
            leftStickInput.y = 0;
        }

        //Prevents running while trying to attack up but doesn't create an additional deadzone while in free aim
        if (!aimingJoy)
        {
            if (leftStickInput.y > joystickDeadzone && leftStickInput.x < joystickDeadzone)
            {
                horizontalInput = 0;
            }
        }

        //Running Animation
        if ((horizontalInput > 0 || horizontalInput < 0) && canMove)
        {
            playerAnimator.SetBool("Running", true);

        }

        if (horizontalInput == 0)
        {
            playerAnimator.SetBool("Running", false);
        }

        if (canMove && !isAttacking)
        {
            Run();
        }

        //Flips
        if (horizontalInput > 0f && !facingRight && canMove && !isWallJumping)
        {

            FlipPlayerX();
        }

        if (horizontalInput < 0f && facingRight && canMove && !isWallJumping)
        {

            FlipPlayerX();

        }


        //Jumping
        if (InputManager.AButtonDown())
        {

            if (canMove && !isCrouching && IsGrounded())
            {
                Jump();
                JumpDust();
            }

            //Wall jumping
            if(!IsGrounded() && isWallSliding)
            {
                WallJump();

            }

        }

        if(IsGrounded() && falling)
        {
            isJumping = false;
        }

        //Double Jump
        if (!IsGrounded() && !isHanging && !isWallSliding)
        {

            playerAnimator.SetBool("Jumping", true);

            if (!doubleJump && canDoubleJump && InputManager.AButtonDown())
            {
                Jump();
                JumpDust();
                doubleJump = true;
                playerAnimator.Play("Jump2");
                enableLandDust = true;
            }
        }
        else if (IsGrounded())
        {
            playerAnimator.SetBool("Jumping", false);
            doubleJump = false;
            falling = false;
        }

            //Enables double jumping after every wall jump
        else if(isWallJumping)
        {
            doubleJump = false;
        }

        //Variable Jump Height
        if (InputManager.AButtonUp() && !IsGrounded() && playerRb.velocity.y > 5)
        {
            JumpStop();
        }

        //Ledge grab (Only works from underneath edge, if you increase jump or fall speed raycast will miss the edge detection due to the raycast being tied to the player position) 
        if (WallCheck() && !LedgeCheck() && canLedgeGrab)
        {
            canLedgeGrab = false;
            LedgeGrab();

        }

        //Allows you to jump out of ledge grab
        if (InputManager.AButtonDown() && isHanging)
        {
            canMove = true;

            isHanging = false;
            playerAnimator.Play("Hang to Jump");
            ResetGravity();
            Jump();

            if (canWallSlide)
            {
                StartCoroutine(WallSlideTempDisable());
            }

            StartCoroutine(LedgeGrabActivation());

        }

        //Wall Sliding
        if(canMove && canWallSlide && WallCheck() && !IsGrounded() && !isWallJumping)
        {

            playerAnimator.ResetTrigger("Stop Wall Sliding");

            if (horizontalInput != 0)
            {
                isWallSliding = true;
                playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -wallSlideSpeed, float.MaxValue));

                playerAnimator.Play("Wall Slide");
            }


            else
            {
                playerAnimator.Play("Jump");
                isWallSliding = false;
                StartCoroutine(WallSlideTempDisable());
                //Flips the player after ending wall slide so that animation direction and player direction matches up
                FlipPlayerX();

            }

            
        }
        else
        {

            isWallSliding = false;

            playerAnimator.SetTrigger("Stop Wall Sliding");
 
        }


        // Crouching
        if (verticalInput < -joystickDeadzone && canCrouch && !isRising)
        {

            if (IsGrounded() && canMove && !isCrouching)
            {
                isCrouching = true;

            }

        }

        if (isCrouching)
        {

            if (!isSliding && IsGrounded())
            {
                Crouch();
            }


        }


        // Keeps player crouched if colliding with ceiling
        if (CeilingCheck() && IsGrounded())
        {
            playerAnimator.SetBool("Crouching", true);
            isCrouching = true;
        }


        //Rise
        else if (verticalInput > -joystickDeadzone && isCrouching && !isSliding)
        {
            isRising = true;

            playerAnimator.SetBool("Crouching", false);

        }


        //Wait time before you can move after crouching
        if (isRising)
        {
            riseWait += Time.deltaTime;

            if (riseWait > 0.18f)
            {
                isRising = false;

                StopCrouch();

                riseWait = 0;
            }
        }


        //Dashing Input
        if (InputManager.BButtonDown())
        {
            if (canMove && canDash && !isCrouching)
            {
                isDashing = true;
            }
        }


        //Slide Input
        if (InputManager.BButtonDown())
        {
            if (canSlide && isCrouching && !isRising)
            {
                isSliding = true;
                slideDust = true;
                StartCoroutine(Slide());
            }
        }

        //Sliding
        if (isSliding)
        {

            //When Sliding off edge
            if (falling)
            {
                StopCrouch();
                isSliding = false;
                playerAnimator.Play("Jump");
                playerAnimator.SetBool("Crouching", false);

            }

            //Slide Dust
            if (slideDust)
            {
                slideDust = false;
                GameObject dust;
                dust = Instantiate(dustSlidePrefab, playerRb.transform.position + dustSlideOffset, Quaternion.identity);

                Animator dustAnim = dust.GetComponent<Animator>();

                dustAnim.Play("Dust_1");
                Destroy(dust, 1f);
            }

        }


        //Melee Attack Input

        if (attackInitiated && weapon == 1)
        {
            canMove = false;

            //bool value changed in animator
            if (!isAttacking)
            {

                canMove = true;
            }

        }


        //Manages attack input when charge attack is disabled and only normal attack is enabled
        if (InputManager.XButtonDown() && IsGrounded() && !isCrouching && weapon == 1 && canAttack && canMove)
        {
            attackInitiated = true;

            if (canChargeAttack)
            {
                return;
            }

            //Up Attack
            else if (verticalInput > joystickDeadzone)
            {
                UpAttack();
            }
            else
            {
                //Normal Attack
                Attack();
            }
        }


        //Charge Attack Input

        if (InputManager.XButtonHold() && IsGrounded() && !isJumping && !isCrouching && weapon == 1 && canChargeAttack)
        {

            if (canMove)
            {
                attackInitiated = true;
            }

            if (attackInitiated)
            {
                buttonHoldTime += Time.deltaTime;

                if (buttonHoldTime > 0.2f)
                {
                    ChargeAttack();
                    playerRb.velocity = new Vector2(0, playerRb.velocity.y);

                    //Charged Effect
                    if (buttonHoldTime > (chargeTime - 0.1f))
                    {
                        chargeEffectAnimator.SetBool("isCharging", true);
                        chargeEffectAnimator.Play("Charge Effect");
                    }
                }
            }

        }


        //Charge Attack Release
        if (InputManager.XButtonUp() && IsGrounded() && !isJumping && attackInitiated && canChargeAttack && weapon == 1)
        {

            chargeEffectAnimator.SetBool("isCharging", false);
            attackInitiated = false;

            if (buttonHoldTime <= 0.2f && canAttack)
            {
                canMove = false;
                buttonHoldTime = 0f;

                //Up Attack
                if (verticalInput > joystickDeadzone)
                {
                    UpAttack();
                }
                else
                {
                    //Normal Attack
                    Attack();
                }
            }

            else if (buttonHoldTime > chargeTime)
            {
                buttonHoldTime = 0f;
                playerAnimator.Play("Charge Attack Release");
                attackAnimator.Play("SlashAttack");
            }

            else if (buttonHoldTime > 0.2f && buttonHoldTime <= chargeTime)
            {
                buttonHoldTime = 0f;
                Return2Idle();
                MovementResume();

            }

            else
            {
                buttonHoldTime = 0f;
                MovementResume();
            }

        }


        //Blocking
        if (InputManager.YButtonHold() && IsGrounded() && canMove && canBlock && weapon == 1)
        {
            canMove = false;

            isBlocking = true;

            playerRb.velocity = Vector2.zero;
            playerAnimator.Play("Block");
        }
        if (InputManager.YButtonUp() && IsGrounded() && canBlock && weapon == 1)
        {
            isBlocking = false;
            playerAnimator.SetTrigger("Stop Blocking");
        }

        //Knockback condition
        if (playerHit && knockbackWhenHit)
        {

            StartCoroutine(Knockback());

        }

        //Weapon Swap Input
        if (InputManager.RBButtonDown() && canWeaponSwap)
        {
            if (IsGrounded() && canMove)
            {
                WeaponSwap();
                playerRb.velocity = Vector2.zero;
                canMove = false;
            }

        }


        //Gun Aim Input
        leftTriggerInput = Input.GetAxisRaw("LT");
        rightTriggerInput = Input.GetAxisRaw("RT");

        if (aimingJoy && canFreeAim && laserSight.line.enabled)
        {
            if (facingRight)
            {
                playerAnimator.SetFloat("Aim X", horizontalInput);
                playerAnimator.SetFloat("Aim Y", verticalInput);
            }

            else if (!facingRight)
            {
                playerAnimator.SetFloat("Aim X", -horizontalInput);
                playerAnimator.SetFloat("Aim Y", verticalInput);
            }

            //360 Aim gun fire input, shooting direction configured in LaserSight script
            if (rightTriggerInput > 0.2f)
            {
                shooting360 = true;
            }

            if (rightTriggerInput == 0 || isKnockedBack)
            {
                shooting360 = false;
            }

            //Stops shooting when aiming button has been released
            if (leftTriggerInput == 0)
            {
                shooting360 = false;
            }

        }

        //360 Aim for keyboard and mouse 
        if (aimingKB && canFreeAim && laserSight.line.enabled)
        {
            playerAnimator.SetFloat("Aim X", laserSight.mouseDirection.x * transform.localScale.x);
            playerAnimator.SetFloat("Aim Y", laserSight.mouseDirection.y);

            if (Input.GetMouseButton(0))
            {
                shooting360 = true;
            }

            if (Input.GetMouseButtonUp(0) || isKnockedBack)
            {
                shooting360 = false;
            }

            //Stops shooting when aiming button has been released
            if (InputManager.CtrlKeyUp())
            {
                shooting360 = false;
            }

        }

        if (IsGrounded() && !isCrouching && canFreeAim && weapon == 2)
        {

            if (InputManager.CtrlKeyHold() && !aimingJoy && canMove)
            {
                Aim();
                aimingKB = true;
            }

            if (InputManager.CtrlKeyUp() && !aimingJoy && !canMove)
            {
                aimingKB = false;
                StopAim();
            }

            if (leftTriggerInput > 0.2f && !aimingKB && canMove)
            {
                Aim();
                aimingJoy = true;
            }


            if (leftTriggerInput == 0 && !aimingKB && !canMove)
            {
                aimingJoy = false;
                StopAim();
            }
        }


        //Gun fire Input
        if (InputManager.XButtonDown() && IsGrounded() && !isCrouching && canFire && canMove && weapon == 2)
        {
            playerRb.velocity = Vector2.zero;
            playerAnimator.Play("Gun Fire Standing");
            StartCoroutine(Fire());
        }

        //Gun fire crouching
        if (InputManager.XButtonDown() && isCrouching && canFireCrouching && weapon == 2)
        {
            playerAnimator.Play("Gun Fire Crouching");
            StartCoroutine(FireCrouching());
        }

        //Gun Fire 360
        if (shooting360 && canFire && !isKnockedBack)
        {
            GameObject bullet = Instantiate(bullet360, laserSight.originPos.position, laserSight.aimPivot.gameObject.transform.rotation);

            if (aimingJoy)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(laserSight.laserDirection.x, laserSight.laserDirection.y).normalized * freeAimBulletSpeed;
            }
            if (aimingKB)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(laserSight.mouseDirection.x, laserSight.mouseDirection.y).normalized * freeAimBulletSpeed;
            }

            StartCoroutine(FireRate());

        }

    }


    void FixedUpdate()
    {

        if (enableLandDust)
        {
            if (isHanging)
            {
                enableLandDust = false;
            }
            else
            LandDust();
        }

        if (isDashing)
        {
            playerAnimator.Play("Dash");

            StartCoroutine(Dash());
        }


        //Higher falling speed after apex of jump
        if (playerRb.velocity.y < 0)
        {
 
            playerRb.gravityScale *= fallSpeedMultiplier;
            playerRb.gravityScale = Mathf.Clamp(playerRb.gravityScale, 0, maxGravityFallValue);
            falling = true;
        }
        else if (!isHanging)
        {
            ResetGravity();
        }

    }

    private void LedgeGrab()
    {

        isHanging = true;

        canMove = false;

        playerRb.gravityScale = 0f;
        playerRb.velocity = Vector2.zero;

        if (!facingRight)
        {
            transform.position = new Vector2(transform.position.x - hangOffset.x, transform.position.y + hangOffset.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + hangOffset.x, transform.position.y + hangOffset.y);
        }

        playerAnimator.Play("Ledge Hang");
    }

    IEnumerator LedgeGrabActivation()
    {
        yield return new WaitForSeconds(0.1f);

        canLedgeGrab = true;
    }

    IEnumerator WallSlideTempDisable()
    {
        canWallSlide = false;

        yield return new WaitForSeconds(0.2f);

        canWallSlide = true;

    }

    IEnumerator WallJumpDuration(float duration)
    {
        FlipPlayerX();

        yield return new WaitForSeconds(duration);
        isWallJumping = false;
    }

    void Jump()
    {

        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        isJumping = true;
        buttonHoldTime = 0;
        
    }

    void JumpStop()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForceOnRelease);
    }

    void WallJump()
    {
        isWallJumping = true;

        playerAnimator.Play("Jump");

        playerRb.velocity = new Vector2(-playerRb.transform.localScale.x * wallJumpPowerX, wallJumpPowerY);

        StartCoroutine(WallJumpDuration(wallJumpDuration));
    }

    void JumpDust()
    {
        jumpDustClone = Instantiate(jumpDustPrefab, playerRb.transform.position + jumpDustOffset, Quaternion.identity);
        Destroy(jumpDustClone, 1f);
    }

    //Land dust (Only activated after landing a double jump)
    void LandDust()
    {
        if (IsGrounded() && playerRb.gravityScale == playerGravity)
        {
            enableLandDust = false;

            GameObject dust2;
            dust2 = Instantiate(landDustPrefab, transform.position + landDustOffset, Quaternion.identity);

            Animator dustAnim2 = dust2.GetComponent<Animator>();

            dustAnim2.Play("Dust_2");

            Destroy(dust2, 1f);
        }
    }

    //Running
    void Run()
    {
        if (!isWallJumping && !isWallSliding)
        {
            playerRb.velocity = new Vector2(runSpeed * horizontalInput, playerRb.velocity.y);
        }
    }


    //Crouch
    private void Crouch()
    {
        canCrouch = false;
        canMove = false;

        playerRb.velocity = Vector2.zero;

        playerAnimator.SetBool("Crouching", true);
        playerAnimator.SetBool("Running", false);
    }

    private void StopCrouch()
    {
        canMove = true;
        canCrouch = true;
        isCrouching = false;
    }

    //Dashing
    private IEnumerator Dash()
    {
        canDash = false;

        playerRb.velocity = new Vector2(dashForce * transform.localScale.x * 10, 0);
        playerRb.gravityScale = 0f;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        if (!isHanging)
        {
            ResetGravity();
        }

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }


    //Sliding
    private IEnumerator Slide()
    {

        playerAnimator.Play("Slide");

        canSlide = false;
        canMove = false;
        playerRb.velocity = new Vector2(slideForce * transform.localScale.x * 10, 0);

        yield return new WaitForSeconds(slideDuration);

        isSliding = false;

        yield return new WaitForSeconds(slideCooldown);

        canSlide = true;

    }

    //Attack
    public void Attack()
    {
        playerAnimator.Play("Slash");
        attackAnimator.Play("SlashAttack");
        playerRb.velocity = new Vector2(0, playerRb.velocity.y);
    }

    public void UpAttack()
    {
        playerAnimator.Play("Slash Up");
        attackAnimator.Play("SlashAttack Up");
        playerRb.velocity = new Vector2(0, playerRb.velocity.y);
    }

    public void ChargeAttack()
    {
        playerAnimator.Play("Charge Attack Hold");
        playerRb.velocity = new Vector2(0, playerRb.velocity.y);
    }

    //Knockback
    public IEnumerator Knockback()
    {

        playerHit = false;
        playerAnimator.Play("Knockback");

        isKnockedBack = true;

        yield return new WaitForSeconds(knockbackDuration);

        playerRb.velocity = Vector2.zero;

        isKnockedBack = false;

        if (!playerHealth.dead)
        {
            MovementResume();
            Return2Idle();
        }

    }

    //Bullets hitting player
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet") && !playerHealth.dead)
        {
            MovementStop();

            if (!isBlocking)
            {
                playerHit = true;

                //calculates normalized direction in which the bullet hits player
                Vector2 direction = (playerRb.transform.position - collision.gameObject.transform.position).normalized;

                //Knocks player back in direction opposite of where the projectile hit
                playerRb.AddForce(direction * knockbackStrength * 10, ForceMode2D.Impulse);

                //The scalar product will determine direction hit from and will flip player so knockback animation will play accordingly in the correct direction
                if (!facingRight)
                {
                    if (Vector2.Dot(direction, transform.position) < 0)
                    {
                        FlipPlayerX();
                    }
                }
                else if (facingRight)
                {
                    if (Vector2.Dot(direction, transform.position) > 0)
                    {
                        FlipPlayerX();
                    }
                }
            }

        }

    }

    public void WeaponSwap()
    {
        //Changes animation layers when equiping gun

        if (weapon == 1)
        {
            playerAnimator.SetLayerWeight(1, 1);
            playerAnimator.Play("Weapon Swap");
            weapon = 2;
        }

        else if (weapon == 2)
        {
            playerAnimator.SetLayerWeight(1, 0);
            playerAnimator.Play("Weapon Swap Reverse");
            weapon = 1;
        }

    }

    //Gun Fire Shot and Cooldown
    public IEnumerator Fire()
    {

        canMove = false;
        canFire = false;

        GameObject bullet;

        bullet = Instantiate(projectile, transform.position + new Vector3(projectileOffset.x * transform.localScale.x, projectileOffset.y), Quaternion.identity);
        bullet.transform.localScale = new Vector3(transform.localScale.x, 1, 1);

        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(projectileSpeed * transform.localScale.x * 10, 0), ForceMode2D.Impulse);

        yield return new WaitForSeconds(fireCooldown);

        canFire = true;

    }
    public IEnumerator FireCrouching()
    {

        canFireCrouching = false;

        GameObject bullet;

        bullet = Instantiate(projectile, transform.position + new Vector3(projectileOffsetCrouching.x * transform.localScale.x, projectileOffsetCrouching.y), Quaternion.identity);
        bullet.transform.localScale = new Vector3(transform.localScale.x, 1, 1);

        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(projectileSpeed * transform.localScale.x * 10, 0), ForceMode2D.Impulse);

        yield return new WaitForSeconds(fireCooldown);

        canFireCrouching = true;

    }

    //Fire rate in bullets per second 
    private IEnumerator FireRate()
    {
        canFire = false;

        yield return new WaitForSeconds(1 / freeAimFireRatePerSecond);

        canFire = true;
    }


    //Gun Aiming animations, aiming directions set up as a blend tree in animator
    private void Aim()
    {
        canMove = false;
        playerRb.velocity = Vector2.zero;
        playerAnimator.SetBool("Running", false);
        playerAnimator.SetBool("Stop Aiming", false);
        playerAnimator.SetTrigger("Aiming");
    }

    private void StopAim()
    {
        playerAnimator.SetBool("Stop Aiming", true);
        playerAnimator.SetFloat("Aim X", 0);
        playerAnimator.SetFloat("Aim Y", 0);
    }


    //Is also called as an animation event
    public void Return2Idle()
    {
        playerAnimator.Play("Idle");
        attackAnimator.Play("Default");

    }

    void ResetGravity()
    {
        playerRb.gravityScale = playerGravity;
    }

}
