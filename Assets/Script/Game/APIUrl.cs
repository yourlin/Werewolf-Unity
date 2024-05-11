using System;
public class APIUrl
{
	public readonly static string getPlayer = GameSetting.APIUrl + "getPlayer";
	public readonly static string startGame = GameSetting.APIUrl + "startGame";
	public readonly static string stopGame = GameSetting.APIUrl + "stopGame";
	public readonly static string resetGame = GameSetting.APIUrl + "resetGame";
	public readonly static string getMsg = GameSetting.APIUrl + "getMsg";
    public readonly static string getEndingMsg = GameSetting.APIUrl + "fakeEnding";
}