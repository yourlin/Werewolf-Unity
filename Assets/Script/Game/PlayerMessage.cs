using System.Collections.Generic;

public class GameResult {
	public List<string> History { get; set; }
	public List<int> Victory;
}


public enum PlayerMessageType {
	PlayerMessage = 1,
	ProphetMessage = 2,
	WolfMessage = 3,
	Justice = 4,
	GameConclusion = 5
}

public class PlayerMessage
{
	public int PlayerId { get; set; }
	public int Round { get; set; }
	public string PlayerName { get; set; }
	public string Message { get; set; }
	public int TargetId { get; set; }
	public GameResult Result { get; set; }
	public PlayerMessageType Type { get; set; }
}
