using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float maxHighlightRange = 32.0f;
    public float maxInteractableRange = 3.0f;
    public bool isInteractable = true;
    public bool isInteracting = false;
    public bool isHighlighted = false;
    public bool mouseIsOver = false;
    public GameObject display;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                Vector3 diffPos = transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                float distanceToPlayerSquared = (diffPos.x * diffPos.x) + (diffPos.y * diffPos.y);
                if (distanceToPlayerSquared <= maxHighlightRange || !isInteracting)
                {
                    //highlight NPC
                    if (!isHighlighted && !mouseIsOver)
                    {
                        isHighlighted = true;
                        Debug.Log("highlighting NPC");
                    }
                }
                else
                {
                    if (!mouseIsOver)
                    {
                        isHighlighted = false;
                    }
                }
                if (distanceToPlayerSquared <= maxInteractableRange || !isInteracting)
                {
                    display.SetActive(true);
                    if (Input.GetKey("e") || Input.GetKey("E"))
                    {
                        isInteracting = true;
                        Debug.Log("Player is now interacting with NPC");
                        //deploy code to display NPC dialof and limit player controls
                        //NPC isInteracting=true indicates this state.
                    }
                }
                else
                {
                    display.SetActive(false);
                }
                if (isInteracting)
                {
                    //deploy code to display NPC dialog and limit player controls
                    isHighlighted = false;
                    if (Input.GetKey("x") || Input.GetKey("X"))//placeholder key press to exit isInteracting state
                    {
                        isInteracting = false;
                        Debug.Log("Player is no longer interacting with NPC");
                    }
                }
            }
        }

    }

    private void OnMouseEnter()
    {
        mouseIsOver = true;
        if (!isHighlighted || !isInteracting)
        {
            //highlight NPC
            isHighlighted = true;
            Debug.Log("highlighting NPC");
        }
    }

    private void OnMouseExit()
    {
        mouseIsOver = false;
    }
}
