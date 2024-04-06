using System;

public static class SceneSwitchEventHandler
{
    /// <summary>
    /// 事件(切换场景时调用)
    /// </summary>
    public static Action<string> SwitchScenes;
    /// <summary>
    /// 调用事件
    /// </summary>
    /// <param name="targetScene">目标场景</param>

    public static void CallSwitchScenes(string targetScene)
    {
        SwitchScenes?.Invoke(targetScene);
    }
}