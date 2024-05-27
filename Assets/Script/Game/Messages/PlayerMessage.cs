using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public class GameResult {
    [SerializeField]
    public List<string> History { get; set; }
    [SerializeField]
    public List<int> Victory;
}

[Serializable]
public class MessageContent {
    [SerializeField]
    public string role;
    [SerializeField]
    public string content;
}

public enum GameStage {
    Waiting = 0,
    NightWolf = 1,
    NightProphet = 2,
    NightWitch = 3,
    NightAction = 4,
    DeathWords = 5,
    DayDebate = 6,
    DayVote = 7,
    Assistant = 8
}

public enum PlayerMessageType {
    PlayerMessage = 1,
    ProphetMessage = 2,
    WolfMessage = 3,
    Justice = 4,
    GameConclusion = 5
}

[Serializable]
public class PlayerMessage {
    [SerializeField]
    public int PlayerId { get; set; }
    [SerializeField]
    public int Round { get; set; }
    [SerializeField]
    public string PlayerName { get; set; }
    [SerializeField]
    public MessageContent Message { get; set; }
    [SerializeField]
    public int TargetId { get; set; }
    [SerializeField]
    public GameResult Result { get; set; }
    [SerializeField]
    public PlayerMessageType Type { get; set; }
    [SerializeField]
    public GameStage Stage { get; set; }
    [SerializeField]
    public bool IsDay { get; set; }
    [SerializeField]
    public string CurrentTime { get; set; }

    public override string ToString () {
        return JsonConvert.SerializeObject (this, Formatting.None, new PlayerMessageConverter ());
    }
}

public class PlayerMessageConverter : JsonConverter {
    public override bool CanConvert (Type objectType) {
        return objectType == typeof (PlayerMessage);
    }

    public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject jsonObject = JObject.Load (reader);

        PlayerMessage playerMessage = new PlayerMessage {
            PlayerId = jsonObject ["player_id"] != null ? (int)jsonObject ["player_id"] : 0,
            PlayerName = jsonObject ["player_name"]?.ToString (),
            Message = new MessageContent {
                role = jsonObject ["message"] != null ? (string)jsonObject ["message"] ["role"] : null,
                content = jsonObject ["message"] != null ? (string)jsonObject ["message"] ["content"] : null
            },
            TargetId = jsonObject ["target_id"] != null ? (int)jsonObject ["target_id"] : 0,
            Round = jsonObject ["round"] != null ? (int)jsonObject ["round"] : 0,
            IsDay = jsonObject ["is_day"] != null ? (bool)jsonObject ["is_day"] : false,
            CurrentTime = jsonObject ["current_time"]?.ToString (),
            Type = jsonObject ["type"] != null ? (PlayerMessageType)Enum.Parse (typeof (PlayerMessageType), (string)jsonObject ["type"], true) : PlayerMessageType.PlayerMessage,
            Stage = jsonObject ["stage"] != null ? (GameStage)Enum.Parse (typeof (GameStage), (string)jsonObject ["stage"], true) : GameStage.Waiting
        };

        if (jsonObject ["result"] != null) {
            JObject resultObject = (JObject)jsonObject ["result"];
            playerMessage.Result = new GameResult {
                History = resultObject ["history"]?.ToObject<List<string>> (),
                Victory = resultObject ["victory"]?.ToObject<List<int>> ()
            };
        }

        return playerMessage;
    }

    public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
        PlayerMessage playerMessage = (PlayerMessage)value;

        writer.WriteStartObject ();

        writer.WritePropertyName ("player_id");
        writer.WriteValue (playerMessage.PlayerId);

        writer.WritePropertyName ("player_name");
        writer.WriteValue (playerMessage.PlayerName ?? string.Empty);

        writer.WritePropertyName ("message");
        writer.WriteStartObject ();
        writer.WritePropertyName ("role");
        writer.WriteValue (playerMessage.Message?.role ?? string.Empty);
        writer.WritePropertyName ("content");
        writer.WriteValue (playerMessage.Message?.content ?? string.Empty);
        writer.WriteEndObject ();

        writer.WritePropertyName ("target_id");
        writer.WriteValue (playerMessage.TargetId);

        writer.WritePropertyName ("round");
        writer.WriteValue (playerMessage.Round);

        writer.WritePropertyName ("is_day");
        writer.WriteValue (playerMessage.IsDay);

        writer.WritePropertyName ("current_time");
        writer.WriteValue (playerMessage.CurrentTime ?? string.Empty);

        writer.WritePropertyName ("type");
        writer.WriteValue (playerMessage.Type.ToString ());

        writer.WritePropertyName ("stage");
        writer.WriteValue (playerMessage.Stage.ToString ());

        writer.WritePropertyName ("result");
        writer.WriteStartObject ();
        writer.WritePropertyName ("history");
        serializer.Serialize (writer, playerMessage.Result?.History ?? new List<string> ());
        writer.WritePropertyName ("victory");
        serializer.Serialize (writer, playerMessage.Result?.Victory ?? new List<int> ());
        writer.WriteEndObject ();

        writer.WriteEndObject ();
    }

    //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //{
    //    throw new NotImplementedException ();
    //}

    //public override bool CanConvert (Type objectType) {
    //    throw new NotImplementedException ();
    //}
}