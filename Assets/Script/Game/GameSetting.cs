using UnityEngine;

public class GameSetting : Singleton<GameSetting>
{
    [SerializeField]
    static int playerNum = 10;
    [SerializeField]
    static int playSpeed = 1;
    [SerializeField]
    static float gameLoopInterval = 1000f;
	[SerializeField]
    static int requestTimeout = 1;

    public static int PlaySpeed { get => playSpeed; set => playSpeed = value; }
    public static int PlayerNum { get => playerNum; set => playerNum = value; }
	public static float GameLoopInterval { get => gameLoopInterval; set => gameLoopInterval = value; }
	public static int RequestTimeout { get => requestTimeout; set => requestTimeout = value; }
    public static float MessageEndWaitforSeconds { get; internal set; } = 1f;

    // todo 加载配置文件

}
