using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextScene : MonoBehaviour
{
    public float loadTime = 0.1f;

    private void Awake()
    {
        Invoke("ToNext", loadTime);
    }

    void ToNext()
    {
        SceneManager.LoadScene("SmallTown");
    }
}
