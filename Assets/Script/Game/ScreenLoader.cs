 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    public IEnumerator LoadScene(int index)
    {
        animator.SetBool("FadeIn", true);
        animator.SetBool("FadeOut", false);

        yield return new WaitForSeconds(1);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.completed += onLoadScene;
    }

    private void onLoadScene(AsyncOperation operation)
    {
        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", true);

        new WaitForSeconds(1);
    }
}
