using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Animator anim;

    // Start is called before the first frame update
    void Start()
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

        //This next part is for the sword attack to call for it to b active
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //this is the movement. it takes the input for both vertical and horizontal axis simultaneously (allowing for diagonal movement) and multiplies by
        //movespeed so that we can modify the speed as needed. It also includes a sprinting function.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed * 2;
        }
        else
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        }
        //This is for the sword attack. It activates the trigger for the animation to start
        if(Input.GetMouseButtonDown (0))
        {
            anim.SetTrigger("Attack");
        }
    }
}
