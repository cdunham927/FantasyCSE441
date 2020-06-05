using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    //this name is needed to keep a track of where the player will start
    public string transitionName;
    // Start is called before the first frame update
    void Start()
    {
        //this takes the position of the player and makes it match the object holding this script
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
        }
    }
}
