using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextContoller : MonoBehaviour
{
    public TMP_Text text;

    public void Awake()
    {
        text = text.GetComponent<TMP_Text> ();
    }

    public IEnumerator TypeText(string str, float interval) {
        int i = 0;
        while(i <= str.Length) {
            text.text = str.Substring (0, i++);
            yield return new WaitForSeconds (interval);
        }
    }
}
