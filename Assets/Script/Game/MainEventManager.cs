using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEventManager : MonoBehaviour
{
    ScreenLoader screenLoader;
    // Start is called before the first frame update
    void Awake()
    {
        screenLoader = GetComponent<ScreenLoader> ();
    }

    public void onQuit () {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif
    }

    public void onBackToMenu () {
        StartCoroutine (screenLoader.LoadScene (0));
    }
}
