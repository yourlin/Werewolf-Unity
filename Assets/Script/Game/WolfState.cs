using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfState : MonoBehaviour
{
    [Range (0f, 10f)]
    [SerializeField]
    private float interval = 3f;
    Animator animator;
    private float timer = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 累加时间

        if (timer >= interval) {

            animator.SetBool ("IsMoving", GetRandomBool ());
            animator.SetBool ("IsRunning", GetRandomBool ());
            timer = 0f; // 重置计时器
        }
    }

    private bool GetRandomBool () {
        return Random.Range (0, 2) == 0;
    }
}
