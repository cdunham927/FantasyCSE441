using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int id = 0;
    public string itemName;
    public string itemDescription;
    bool inRange = false;
    bool canPickup = false;
    Animator anim;
    public float distance;
    float distanceToPlayerSquared;
    PlayerController player;
    public GameObject display;
    public AudioClip clip;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("ItemAnimate", canPickup);

        Vector3 diffPos = transform.position - player.transform.position;
        distanceToPlayerSquared = (diffPos.x * diffPos.x) + (diffPos.y * diffPos.y);

        if (distanceToPlayerSquared > distance)
        {
            inRange = false;
            canPickup = false;
        }
        else
        {
            inRange = true;
        }

        if (canPickup)
        {
            display.SetActive(true);
            if (Input.GetMouseButtonDown(0)) PickupItem();
        }
        else display.SetActive(false);
    }

    void PickupItem()
    {
        //Debug.Log("Updating quest via itemcontroller");
        FindObjectOfType<QuestHolder>().UpdateCollectQuests(id);
        PlayerController.instance.PlayClip(clip);
        gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        if (inRange)
        {
            canPickup = true;
            //Debug.Log("Mouse is over item and player is in range");
        }
    }

    private void OnMouseExit()
    {
        //Debug.Log("Mouse is exiting item");
        canPickup = false;
    }
}
