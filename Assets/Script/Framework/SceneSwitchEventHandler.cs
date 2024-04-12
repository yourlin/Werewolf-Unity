using System;

public static class SceneSwitchEventHandler
{
    /// <summary>
    /// 
    /// </summary>
    public static Action<string> SwitchScenes;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetScene">Ŀ�곡��</param>

    public static void CallSwitchScenes(string targetScene)
    {
        SwitchScenes?.Invoke(targetScene);
    }
}