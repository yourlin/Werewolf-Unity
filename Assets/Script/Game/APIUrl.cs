using System;
public class APIUrl
{
	public readonly static string getPlayer = GameSetting.APIUrl + "getPlayer";
    public readonly static string getVotes = GameSetting.APIUrl + "getVotes";
    public readonly static string startGameCN = GameSetting.APIUrl + "startGame?lang=cn";
    public readonly static string startGameEN = GameSetting.APIUrl + "startGame?lang=en";
    public readonly static string stopGame = GameSetting.APIUrl + "stopGame";
	public readonly static string resetGame = GameSetting.APIUrl + "resetGame";
	public readonly static string getMsg = GameSetting.APIUrl + "getMsg";
    public readonly static string getEndingMsg = GameSetting.APIUrl + "fakeEnding";
}