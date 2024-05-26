using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConclusionCtrl : MonoBehaviour
{
    TextContoller textContoller;

    private void Awake () {
        textContoller = this.GetComponent<TextContoller> ();
    }

    public void onClickBacktoMenu () {
        SceneManager.LoadScene (0);
    }

    public void SetConclusion(string str)
    {
        StartCoroutine(textContoller.TypeText (str, 0.01f));
    }
}
