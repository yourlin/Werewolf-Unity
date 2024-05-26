using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;


[System.Serializable]
public class PlayerVotes
{
    public VoteData votes;
}

[Serializable]
public class VoteData
{
    public List<VoteItem> player_votes = new();
    public List<VoteItem> wolf_votes = new();
    public List<VoteItem> prophet_votes = new();
    public List<VoteItem> witch_antidotes = new();
    public List<VoteItem> witch_poisions = new();

}

[System.Serializable]
public class VoteItem
{
    public int id;
    public string name;
    public int count;
}