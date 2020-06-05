using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public void PlayerRespawn()
    {
        PlayerController.instance.GetComponent<PlayerStats>().Respawn();
    }
}
