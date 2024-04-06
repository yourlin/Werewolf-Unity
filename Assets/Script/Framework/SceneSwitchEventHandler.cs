using System;

public static class SceneSwitchEventHandler
{
    /// <summary>
    /// �¼�(�л�����ʱ����)
    /// </summary>
    public static Action<string> SwitchScenes;
    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="targetScene">Ŀ�곡��</param>

    public static void CallSwitchScenes(string targetScene)
    {
        SwitchScenes?.Invoke(targetScene);
    }
}