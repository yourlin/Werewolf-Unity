using System;
public class APIUrl
{
	private readonly static string baseUrl = "http://werewolflb-388054704.us-east-1.elb.amazonaws.com/";
	public readonly static string getPlayer = baseUrl + "getPlayer";
	public readonly static string startGame = baseUrl + "startGame";
	public readonly static string stopGame = baseUrl + "stopGame";
	public readonly static string resetGame = baseUrl + "resetGame";
	public readonly static string getMsg = baseUrl + "getMsg";
}

