using UnityEngine;

public class LoadScene : MonoBehaviour
{
    /// <summary>
    /// ���س���
    /// </summary>
    /// <param name="sceneName">���س�������</param>

    public void SwitchScene(string sceneName)
    {
        SceneSwitchEventHandler.CallSwitchScenes(sceneName);
    }
}
