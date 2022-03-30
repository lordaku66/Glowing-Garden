using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Singleton Player. Can be accessed publicly with Player.Instance
/// Controls the movement of the player above ground and below ground.
/// 
/// OnSprout, OnJump and OnLongIdle are used to trigger game audio or animations
/// related to their described player actions.
///  
/// | Authors: Jacques Visser & Krishna Thiruvengadam
/// </summary>

[SelectionBase]
public class Player : MonoBehaviour
{
    public static event Action OnSprout;
    public static event Action OnJump;
    public static event Action OnLongIdle;

    private static Player _instance;
    public static Player Instance { get { return _instance; } }
    public GameObject playerSprite;
    private CapsuleCollider2D coll;
    public GameObject cursor;
    public GameObject myceliumEmitter;

    [Header("MOVE SPEED | JUMP HEIGHT | COLLISION SIZE")]
    public float moveSpeed = 1f;
    public float jumpForce = 3.5f;
    public float collisionRadius = 1f;

    [Header("GROUND & SOIL Y-POSITIONS")]
    public float groundLevel = 0;
    [SerializeField] private float soilLevel = 0;
    [SerializeField] private float transitionTime = 0.5f;

    [Header("COLLISION LAYERS")]
    [SerializeField] LayerMask whatIsOver;
    [SerializeField] LayerMask whatIsUnder;
    [SerializeField] LayerMask whatIsMush;
    [SerializeField] LayerMask whatIsObstacle;
    public bool grounded;
    public bool digging;
    public bool onMushroom;
    public bool stuck;

    public Transform drillTip;

    [Header("CAMERAS")]
    public GameObject undergroundCam;
    public GameObject overgroundCam;

    [Header("TRANSISTION CHECKS")]
    private bool ascend;
    private bool canDig;

    Vector3 moveDirection;
    float playerYPos;

    private Animator anim;
    SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    [SerializeField] private float idleWaitTime;
    [SerializeField] private bool longIdle;
    public float idleTime = 0;
    public float airTime = 0;

    private Mycos collectedMyco;

    private void OnEnable()
    {
        _instance = this;

        GameManager.OnLeftHold += OvergroundMovement;
        GameManager.OnLeftHold += UndergroundMovement;

        GameManager.OnLeftUp += OnStop;

        GameManager.OnRightMouse += Jump;

        GameManager.OnPause += OnPause;
        GameManager.OnInteract += OnPause;
        MouseRangeCheck.OnMouseInvalid += OnPause;

        GameManager.OnDialogueClose += OnPlay;
        GameManager.OnPlay += OnPlay;
        MouseRangeCheck.OnMouseValid += OnPlay;

        Stamina.OnMeterEmpty += TurnTrailsOff;
        Stamina.OnMeterFilled += OnRefill;

        Transitions.UndergroundEnter += OnPause;
        Transitions.UndergroundExit += OnPause;
        Transitions.OnReady += OnPlay;

        Transitions.UndergroundEnter += OnGroundEnter;
        Transitions.UndergroundExit += OnGroundExit;
    }

    private void Start()
    {
        coll = GetComponent<CapsuleCollider2D>();
        anim = playerSprite.GetComponent<Animator>();
        spriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Disable the player script when GameManager.Pause is invoked.
    private void OnPause()
    {
        this.enabled = false;
        anim.SetBool("IsWalking", false);
    }

    // Enable the player script whrn GameManager.Play is invoked.
    private void OnPlay()
    {
        this.enabled = true;
    }

    private void OnStop()
    {
        anim.SetBool("IsColliding", false);
        anim.SetBool("IsWalking", false);
    }

    // Change to underground camera fov, emit mycelium, disable player gravity.
    public void OnGroundEnter()
    {
        overgroundCam.SetActive(false);
        undergroundCam.SetActive(true);

        StartCoroutine(StartTrails());

        rb2d.velocity = Vector2.zero;
    }

    // Sprout mushrooms and reset camera and gravity.
    public void OnGroundExit()
    {
        overgroundCam.SetActive(true);
        undergroundCam.SetActive(false);

        myceliumEmitter.GetComponent<TrailRenderer>().emitting = false;

        // Sprout a mushroom so long as the player has mycos in their inventory
        if (GameManager.hasMycos)
        {
            OnSprout?.Invoke();
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        moveDirection = cursor.transform.position - transform.position;
        playerYPos = transform.position.y;

        // Collisions are checked via physics shapes and read as bools and filtred via layermasks.
        digging = Physics2D.OverlapCircle(transform.position, collisionRadius, whatIsUnder);
        grounded = Physics2D.OverlapCircle(transform.position, collisionRadius, whatIsOver);
        onMushroom = Physics2D.OverlapCircle(transform.position, collisionRadius, whatIsMush);
        stuck = Physics2D.OverlapCircle(drillTip.position, 0.03f, whatIsObstacle);

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            idleTime = 0f;
            longIdle = false;
        }
        else
        {
            if (idleTime <= idleWaitTime)
            {
                idleTime += Time.deltaTime;
            }

            if (!digging)
            {
                if (idleTime > idleWaitTime && !longIdle)
                {
                    OnLongIdle?.Invoke();
                    longIdle = true;
                }
            }
        }

        // Update player animator 
        anim.SetFloat("idleWait", idleTime);
        anim.SetBool("IsGrounded", grounded);
        anim.SetBool("IsDigging", digging);
    }

    private void FixedUpdate()
    {
        if (grounded)
        {
            rb2d.angularVelocity = 0;
            moveSpeed = 1;
            airTime = 0;
        }
        else if (digging)
        {
            grounded = false;
            moveSpeed = 1.8f;
            rb2d.freezeRotation = true;
        }
        else
        {
            airTime += Time.deltaTime;
        }
    }


    // Set the collected Myco nutrient based on a trigger check.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Mycos")
        {
            collectedMyco = other.gameObject.GetComponent<Mycos>();
        }
    }

    // Defines the sectors of movement when the player is overground.
    public void OvergroundMovement()
    {
        anim.SetBool("IsWalking", true);

        if (playerYPos <= soilLevel)
        {
            return;
        }

        if (GameCursor.GetRotation() <= 10f || GameCursor.GetRotation() > 350f)
        {
            // Top: 20 deg 
            return;
        }
        else if (GameCursor.GetRotation() > 10f && GameCursor.GetRotation() <= 170f)
        {
            // Left: 160 deg

            // Move left
            transform.Translate(moveSpeed * -Vector3.right * Time.deltaTime);
            spriteRenderer.flipX = true;
        }
        else if (GameCursor.GetRotation() > 190f && GameCursor.GetRotation() <= 350f)
        {
            // Right: 160 deg

            // Move right
            transform.Translate(moveSpeed * Vector3.right * Time.deltaTime);
            spriteRenderer.flipX = false;
        }
        else
        {
            // Bottom: 10 deg
            if (grounded && !onMushroom)
            {
                canDig = true;
            }
        }

        // If the mouse faces down, burrow.
        if (canDig)
        {
            Transitions.Burrow(gameObject);
            canDig = false;
        }
    }

    // Defines the sectors of movement when the player is underground.
    public void UndergroundMovement()
    {
        if (!digging)
        {
            return;
        }
        anim.SetBool("IsColliding", stuck);

        playerSprite.transform.localEulerAngles = new Vector3(0f, 0f, GameCursor.GetRotation());
        transform.Translate(moveSpeed * moveDirection * 2 * Time.deltaTime);

        if (playerYPos < groundLevel && playerYPos >= soilLevel)
        {
            if (GameCursor.GetRotation() <= 60f || GameCursor.GetRotation() > 300f)
            {
                ascend = true;
            }
            else
            {
                ascend = false;
            }
        }

        // If the mouse faces up, resurface.
        if (ascend)
        {
            Transitions.Resurface(gameObject);
            ascend = false;
        }
    }

    public void UpwardTransition()
    {
        coll.enabled = false;
        LeanTween.moveLocalY(gameObject, groundLevel, transitionTime).setOnComplete(() => coll.enabled = true);

        ascend = false;
        rb2d.gravityScale = 1;

        AudioManager.Instance.Play("Exiting Underground");
    }

    // Wait 0.7 secs before turning on mycelium emitters.
    private IEnumerator StartTrails()
    {
        yield return new WaitForSeconds(0.7f);
        myceliumEmitter.GetComponent<TrailRenderer>().emitting = true;
    }

    // Sends the player upwards while grounded using a rigidbody impulse.
    public void Jump()
    {
        if (!digging)
        {
            if (grounded || onMushroom)
            {
                rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Twimst();
                idleTime = 0f;
                longIdle = false;

                OnJump?.Invoke();
            }
        }
    }

    // When the player runs our of stamina - turn of mycelium trails and myco collection.
    public void TurnTrailsOff()
    {
        gameObject.tag = "Untagged";
        myceliumEmitter.GetComponent<TrailRenderer>().emitting = false;
        moveSpeed = 1.2f;
    }

    public void OnRefill()
    {
        gameObject.tag = "Player";
    }

    // This is the code that makes the player do a back flip when they jump.
    public void Twimst()
    {
        if (!playerSprite.GetComponent<SpriteRenderer>().flipX)
        {
            playerSprite.LeanRotateZ(-720, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            playerSprite.LeanRotateZ(720, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        }
    }

    // Gets the collected myco set in OnTriggerEnter2D
    public Mycos GetCollectedMyco()
    {
        return collectedMyco;
    }

    // Gets the x position of the player when a mushroom is sprouted. 
    public static float GetX()
    {
        return Instance.transform.position.x;
    }

    // Gets the rigidbod2d attached to the player.
    public Rigidbody2D GetBody()
    {
        return rb2d;
    }

    // Show physics shape circle colliders in the inspector.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(drillTip.position, 0.05f);
    }

    private void OnDisable()
    {
        GameManager.OnLeftHold -= OvergroundMovement;
        GameManager.OnLeftHold -= UndergroundMovement;

        GameManager.OnLeftUp -= OnStop;

        GameManager.OnRightMouse -= Jump;

        Stamina.OnMeterEmpty -= TurnTrailsOff;
        Stamina.OnMeterFilled -= OnRefill;

        Transitions.UndergroundEnter -= OnGroundEnter;
        Transitions.UndergroundExit -= OnGroundExit;
    }
}