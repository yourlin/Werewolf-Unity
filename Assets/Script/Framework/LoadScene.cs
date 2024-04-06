using UnityEngine;

public class LoadScene : MonoBehaviour
{
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">加载场景名字</param>

    public void SwitchScene(string sceneName)
    {
        SceneSwitchEventHandler.CallSwitchScenes(sceneName);
    }
}
