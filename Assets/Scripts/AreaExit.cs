using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    //in the inspector, this must be labeled with the name of the next scene
    public string areaToLoad;
    //in the inspector, this must be labeled with the name of the specific exit name in the other scene
    //example: in going from "scene1- east" exit to next scene, you will label this something like "scene2- west"
    public string areaTransitionName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //this keeps a track of the player location (player object tag must be swithed to "Player" in the inspector) 
        //and if the scene is loaded, it takes the last areaTransition name of the player (something you may have noted in the PlayerController script) and makes you load into the corresponding postion in the new scene.
        //example: if you wanna go from "Scene1- east" exit to scene 2, this will allow you to load scene 2 and start at the correct spot "Scene2- west"
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(areaToLoad);

            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
