using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


public enum PlayerRole {
	Villager = 1, // 村民
	Wolf = 2,     // 狼人
	Prophet = 3,   // 预言家
    Witch = 4       // 女巫
}

public enum PlayerState {
	Idle = 1,
	Dead = 2,
	Busy = 3,
	Dying = 4
}

public enum PlayerGender {
	Male = 1,
	Female = 2
}

[Serializable]
public class PlayerProfile
{
	[SerializeField]
	private int id;
	[SerializeField]
	private string name;
	[SerializeField]
	private PlayerRole role;
	[SerializeField]
	private PlayerGender gender;
	[SerializeField]
	private PlayerState state;
    [SerializeField]
    private string character;

    public int Id { get => id; set => id = value; }
	public string Name { get => name; set => name = value; }
	public PlayerRole Role { get => role; set => role = value; }
	public PlayerGender Gender { get => gender; set => gender = value; }
	public PlayerState State {
		get => state; set {
			state = value;
		}
	}

    public string Character { get => character; set => character = value; }

    public PlayerProfile ()
	{
	}

    public override string ToString () {
        return JsonConvert.SerializeObject (this, Formatting.Indented, new PlayerProfileConverter());
    }
}

public class PlayerProfileConverter : JsonConverter {
    public override bool CanConvert (Type objectType) {
        return objectType == typeof (PlayerProfile);
    }

    public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject jo = JObject.Load (reader);
        PlayerProfile profile = new PlayerProfile ();

        // 检查 id 是否为空
        if (jo ["id"] != null)
            profile.Id = jo ["id"].Value<int> ();

        // 检查 name 是否为空
        if (jo ["name"] != null)
            profile.Name = jo ["name"].Value<string> ();

        // 检查 role 是否为空
        if (jo ["role"] != null)
            profile.Role = ParseRole (jo ["role"].Value<string> ());

        // 检查 character 是否为空
        if (jo ["character"] != null)
            profile.Character = jo ["character"].Value<string> ();

        // 检查 status 是否为空
        if (jo ["status"] != null)
            profile.State = ParseState (jo ["status"].Value<int> ());

        // 检查 gender 是否为空
        if (jo ["gender"] != null)
            profile.Gender = ParseGender (jo ["gender"].Value<string> ());

        return profile;
    }

    public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
        throw new NotImplementedException ();
    }

    private PlayerGender ParseGender (string genderString) {
        return genderString switch
        {
            "男" => PlayerGender.Male,
            "女" => PlayerGender.Female,
            null => PlayerGender.Female,
            _ => throw new JsonSerializationException($"Invalid gender value: {genderString}"),
        };
    }

    private PlayerRole ParseRole (string roleString) {
        return roleString switch
        {
            "预言家" => PlayerRole.Prophet,
            "村民" => PlayerRole.Villager,
            "狼人" => PlayerRole.Wolf,
            "女巫" => PlayerRole.Witch,
            _ => throw new JsonSerializationException($"Invalid role value: {roleString}"),
        };
    }

    private PlayerState ParseState (int stateValue) {
        return (PlayerState)stateValue;
    }

    //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //{
    //    throw new NotImplementedException ();
    //}

    //public override bool CanConvert (Type objectType) {
    //    throw new NotImplementedException ();
    //}
}

public static class PlayerRoleExtensions {
    public static string ToString (this PlayerRole role) {
        return role switch {
            PlayerRole.Villager => "村民",
            PlayerRole.Wolf => "狼人",
            PlayerRole.Prophet => "预言家",
            PlayerRole.Witch => "女巫",
            _ => throw new ArgumentOutOfRangeException (nameof (role), role, null)
        };
    }
}