using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CircularMessage : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField]
    private float interval = 3f;

    private float timer = 0f;

    [SerializeField]
    List<string> strings;

    int currentIndex = 0;


    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 累加时间

        if (timer >= interval) {
            currentIndex++;
            currentIndex %= strings.Count;
            timer = 0f; // 重置计时器
        }
        text.text = strings [currentIndex];
    }
}
