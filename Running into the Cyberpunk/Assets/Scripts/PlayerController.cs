using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public KeyCode left, right, down, jump, dash, walk;
    public Animator anim, dashAnim;
    public GameObject grounded, body, zero, one, two;
    public LayerMask layerGround;
    public int jumpCount = 2, dashCount = 2, objectType;
    public float speed, dashCoolDown, dashReload, multiplier, shootingDuration, avoidTimer, jumpForce;
    public bool dashed, isShooting;

    private GameManager gm;
    private GameObject player;
    private Rigidbody2D r2d;
    private BoxCollider2D b2d;
    private int xL = 0, xR = 0, x = 0;
    private float dashDuration, dashRemainder = int.MaxValue, downTimer, multiplierStand, dashCountChecker;
    private RaycastHit2D hit;
    private bool jumped, canJumpDown = true;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        player = this.gameObject;
        r2d = GetComponent<Rigidbody2D>();
        b2d = GetComponent<BoxCollider2D>();
        dashCountChecker = dashCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.startGame)
        {
            multiplier = gm.multiplier[objectType];

            KillPlayer();
            CharacterInput();
            Dash();
            if (dashed == false)
            {
                if (isGrounded())
                {
                    OnGround();
                }
                else
                {
                    InAir();
                }
            }
            CharacterRotation();
        }
        else
        {
            if (isGrounded())
            {
                CharacterInput();
            }
        }
    }

    void KillPlayer()
    {
        if (player.transform.position.y <= -8f)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator ShootingTimer()
    {
        yield return new WaitForSecondsRealtime(1.25f);
        isShooting = false;
        xL = 0;
        xR = 0;
    }

    void CharacterInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isShooting = true;
            StartCoroutine(ShootingTimer());
        }

        if (Input.GetKey(left))
        {
            if (gm.startGame == false)
            {
                gm.startGame = true;
            }

            xL = -1;
        }
        else if (Input.GetKeyUp(left))
        {
            if (gm.startGame == false)
            {
                gm.startGame = true;
            }

            xL = 0;
        }

        if (Input.GetKey(right))
        {
            if (gm.startGame == false)
            {
                gm.startGame = true;
            }

            xR = 1;
        }
        else if (Input.GetKeyUp(right))
        {
            if (gm.startGame == false)
            {
                gm.startGame = true;
            }

            xR = 0;
        }

        if (Input.GetKeyDown(down) && canJumpDown)
        {
            if (gm.startGame == false)
            {
                gm.startGame = true;
            }

            b2d.enabled = false;
            r2d.velocity = new Vector2(r2d.velocity.x, -(Mathf.Abs(r2d.velocity.y) + 3.5f));
            downTimer = Time.time + .018f;
            canJumpDown = false;
        }

        if (downTimer <= Time.time)
        {
            b2d.enabled = true;
        }

        if (gm.startGame == false)
        {
            if (Input.GetKeyDown(jump) && jumpCount > 0)
            {
                jumpCount--;
                r2d.AddForce(new Vector2(0, (jumpForce + (-r2d.velocity.y))), ForceMode2D.Impulse);
                gm.startGame = true;
                canJumpDown = true;
            }

            Dash();
        }

        x = xL + xR;
    }

    void OnGround()
    {

        if (jumped)
        {
            jumpCount = 2;
            jumped = false;
            canJumpDown = true;
        }

        if (Input.GetKeyDown(jump) && jumpCount > 0)
        {
            jumpCount--;
            r2d.AddForce(new Vector2(0, (jumpForce + (-r2d.velocity.y))), ForceMode2D.Impulse);
            canJumpDown = true;
        }

        if (x == 0)
        {
            anim.SetInteger("state", 0);
            if (hit.transform.GetComponent<MovingObjects>())
            {
                multiplierStand = hit.transform.GetComponent<MovingObjects>().multiplier;
            }
            r2d.velocity = new Vector2((x * speed) - (.01f * multiplierStand), r2d.velocity.y);
        }
        else
        {
            if (Mathf.Abs(x) == 1)
            {
                anim.SetInteger("state", 1);
                if (x == 1)
                {
                    r2d.velocity = new Vector2(x * speed, r2d.velocity.y);
                }
                else
                {
                    r2d.velocity = new Vector2((x * speed) - (.01f * multiplier), r2d.velocity.y);
                }
            }
        }
    }

    void InAir()
    {
        jumped = true;
        anim.SetInteger("state", 4);
        if (Input.GetKeyDown(jump) && jumpCount > 0)
        {
            if (anim.GetInteger("state") == 4)
            {
                anim.SetInteger("state", 5);
            }
            else
            {
                anim.SetInteger("state", 4);
            }
            jumpCount--;
            r2d.AddForce(new Vector2(0, (jumpForce + (-r2d.velocity.y))), ForceMode2D.Impulse);
            canJumpDown = true;
        }

        if (x == -1)
        {
            r2d.velocity = new Vector2((x * speed) - (.01f * multiplier), r2d.velocity.y);
        }
        else
        {
            r2d.velocity = new Vector2(x * speed, r2d.velocity.y);
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(dash) && dashed == false && dashCount > 0)
        {
            dashed = true;
            dashCount--;

            if (x == 0)
            {
                if (Input.GetKeyDown(dash))
                {
                    anim.SetInteger("state", 7);
                    r2d.AddForce(new Vector2(500f * player.transform.localScale.x, 0), ForceMode2D.Force);
                }
            }
            else
            {
                if (Mathf.Abs(x) == 1)
                {
                    if (Input.GetKeyDown(dash))
                    {
                        anim.SetInteger("state", 7);
                        r2d.AddForce(new Vector2(1000f * x, 0), ForceMode2D.Force);
                    }
                }
            }

            if (dashed)
            {
                dashDuration = Time.time + dashCoolDown;
                if (dashRemainder <= Time.time || dashRemainder == int.MaxValue)
                {
                    dashRemainder = Time.time + dashReload;
                }
            }

            if (gm.startGame == false)
            {
                gm.startGame = true;
            }
        }

        if (dashDuration <= Time.time)
        {
            dashed = false;
        }

        if (dashed)
        {
            r2d.gravityScale = 0;
            r2d.velocity = new Vector2(r2d.velocity.x, 0f);
        }
        else
        {
            if (r2d.gravityScale <= .9f)
            {
                r2d.gravityScale += .1f;
            }
        }

        if (dashCount == 0)
        {
            zero.SetActive(true);
            one.SetActive(false);
        }
        else if (dashCount == 1)
        {
            zero.SetActive(false);
            one.SetActive(true);
            two.SetActive(false);
        }
        else
        {
            one.SetActive(false);
            two.SetActive(true);
        }


        //frame loading effect dashAnim[1=loading, 0=full]
        if (dashCount != dashCountChecker)
        {
            dashAnim.SetInteger("state", 1);

            if (dashRemainder <= Time.time)
            {
                dashRemainder += dashReload;
                dashCount++;
            }
        }
        else
        {
            dashAnim.SetInteger("state", 0);
            dashRemainder = int.MaxValue;
        }
    }

    void CharacterRotation()
    {
        if (x < 0)
        {
            player.transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (x > 0)
        {
            player.transform.localScale = new Vector3(2, 2, 2);
        }
    }

    bool isGrounded()
    {
        if (hit = Physics2D.Raycast(grounded.transform.position, -Vector2.up, 0.075f, layerGround))
        {
            hit.transform.GetComponent<MovingObjects>().willFall = true;
            return true;
        }

        return false;
    }
}
