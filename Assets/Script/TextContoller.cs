using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextContoller : MonoBehaviour
{
    public TMP_Text text;

    IEnumerator TypeText(TMP_Text tMP_text, string str, float interval) {
        int i = 0;
        while(i <= str.Length) {
            tMP_text.text = str.Substring (0, i++);
            yield return new WaitForSeconds (interval);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (TypeText (text, "这是一段用于珠子打印的代码，可以做用于对话框文字显示", 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
