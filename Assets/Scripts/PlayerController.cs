using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //allows you to push an object around. make sure you have a rigidbody 2d on the eplayer
    public Rigidbody2D theRB;
    //this will allow us to take the function for speed and multiply by moveSpeed to scale speed if need be
    public float moveSpeed;
    //we need a player instance to keep a track of the player object between scenes
    public static PlayerController instance;
    //we need the areaTransition name because this will help keep track of which area the player is moving to/from when switching scenes
    public string areaTransitionName;
    //allows the use of animator within the sciprt to call activate the animation events
    public Animator anim;
    public GameObject swordObj;
    Animator swordAnim;

    public PlayerStats playerStats;
    public EnemyController target;

    Camera cam;

    public bool isTalking = false;
    PlayerStats stats;

    public float runCost = 15f;
    public float attackCost = 500f;

    public AudioSource src;
    public AudioSource otherSrc;
    public AudioClip[] swordClip;

    bool isAttacking = false;
    enum Direction { Fail, North, East, South, West };
    private Direction mouseDir;
    private int delay = 0;

    void Awake()
    {
        //we make an ninstance of the player object and do not destroy the player object unless there is another instance in the scene to avoid duplicates. 
        //pretty sure this kills the older instance. i forget how the tutorial explained it exactly
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        swordAnim = swordObj.GetComponent<Animator>();
        src = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        cam = FindObjectOfType<Camera>();
        stats = GetComponent<PlayerStats>();
        theRB.freezeRotation = true;
    }

    public void PlayClip(AudioClip cl, float vol = 0.5f)
    {
        otherSrc.volume = vol;
        otherSrc.PlayOneShot(cl);
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null) cam = FindObjectOfType<Camera>();

        if (isTalking || !stats.alive)
        {
            theRB.velocity = Vector2.zero;
        }

        if (!isTalking && stats.alive)
        {
            //have the player always face the mouse for aiming during combat
            faceMouse();

            //have the main camera follow the player without parenting (parenting causes issues when used with faceMouse())
            cameraFollow();

            //this is the movement. it takes the input for both vertical and horizontal axis simultaneously (allowing for diagonal movement) and multiplies by
            //movespeed so that we can modify the speed as needed. It also includes a sprinting function.
            if (Input.GetKey(KeyCode.LeftShift) && stats.curStam > 0)
            {
                stats.ConsumeStamina(runCost);
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed * 2;
            }
            else
            {
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
            }

            //This is for the sword attack. It activates the trigger for the animation to start
            if (Input.GetMouseButtonDown(0) && stats.curStam > 0)
            {
                stats.ConsumeStamina(attackCost);
                src.PlayOneShot(swordClip[Random.Range(0, swordClip.Length - 1)]);
                anim.SetBool("IsAttacking", true);
                isAttacking = true;
                swordAnim.SetTrigger("IsAttacking");
                if (target.tag != "holder")
                    target.EnemyTakeDamage(playerStats.dmgDealt);
                else
                    target.enemyHealth = 1000000;
            }

            SetAnimParams(mouseDir, theRB.velocity);

            if (isAttacking)
            {
                delay++;
                if (delay == 16)
                {
                    anim.SetBool("IsAttacking", false);
                    isAttacking = false;
                    delay = 0;
                }
            }

        }
    }

    void faceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        if (cam != null)
        {
            mousePos = cam.ScreenToWorldPoint(mousePos);
            Vector2 faceDir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

            mouseDir = FindMouseDirection(faceDir);

        }
        //transform.up = faceDir;
    }

    void cameraFollow()
    {
        if (cam != null)
        {
            Vector3 player = GameObject.FindGameObjectWithTag("Player").transform.position;
            cam.transform.position = new Vector3(player.x, player.y, -500);
        }
    }

    Direction FindMouseDirection(Vector2 Mouse)
    {
        Direction tempDir = Direction.Fail;

        if (Mathf.Abs(Mouse.x) >= Mathf.Abs(Mouse.y))
        {
            if (Mouse.x >= 0)
            {
                tempDir = Direction.East;
            }
            else
            {
                tempDir = Direction.West;
            }
        }
        else
        {
            if (Mouse.y >= 0)
            {
                tempDir = Direction.North;
            }
            else
            {
                tempDir = Direction.South;
            }
        }
        return tempDir;
    }

    void SetAnimParams(Direction direction, Vector2 vel)
    {
        float magnitude = 0.0f;

        switch (direction)
        {
            case Direction.North:
                swordObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                swordObj.transform.localPosition = new Vector2(0f, 0.575f);
                anim.SetInteger("Direction", 1);
                break;
            case Direction.East:
                swordObj.transform.rotation = Quaternion.Euler(0, 0, -90);
                swordObj.transform.localPosition = new Vector2(0.575f, 0f);
                anim.SetInteger("Direction", 2);
                break;
            case Direction.South:
                swordObj.transform.rotation = Quaternion.Euler(0, 0, 180);
                swordObj.transform.localPosition = new Vector2(0, -0.575f);
                anim.SetInteger("Direction", 3);
                break;
            case Direction.West:
                swordObj.transform.rotation = Quaternion.Euler(0, 0, 90);
                swordObj.transform.localPosition = new Vector2(-0.575f, 0f);
                anim.SetInteger("Direction", 4);
                break;
            case Direction.Fail:
                swordObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                swordObj.transform.localPosition = new Vector2(0, 0.575f);
                anim.SetInteger("Direction", 3);
                Debug.Log("FindMouseDirection function failed");
                break;
            default:
                swordObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                swordObj.transform.localPosition = new Vector2(0, 0.575f);
                anim.SetInteger("Direction", 3);
                break;
        }

        magnitude = vel.x + vel.y;

        if (magnitude != 0)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }
}
