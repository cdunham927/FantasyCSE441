using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public string areaToLoad;
    public string areaTransitionName;

    public float transitionTime = 0.5f;

    // Update is called once per frame
    void Start()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LoadLevel(areaToLoad));
        }
    }

    IEnumerator LoadLevel(string levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
        PlayerController.instance.areaTransitionName = areaTransitionName;
    }
}
