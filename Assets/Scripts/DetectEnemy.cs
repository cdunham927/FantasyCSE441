using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : MonoBehaviour
{
    public PlayerController playerController;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "enemy_melee" || other.tag == "enemy_ranged")
        {
            playerController.target = other.gameObject.GetComponent<EnemyController>();
        }
        else
        {
            playerController.target = GameObject.FindGameObjectWithTag("holder").GetComponent<EnemyController>();
        }
    }
}
