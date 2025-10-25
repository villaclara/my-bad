using System.Text.Json.Serialization;
using Mybad.Core.Models.Responses;

namespace Mybad.Services.OpenDota.Models.Wards;

public class WardsInfo : BaseResponse
{
	[JsonPropertyName("players")]
	public List<Player> Players { get; set; } = [];
}


public class ObsLeftLogEntry
{
	[JsonPropertyName("time")]
	public int Time { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("slot")]
	public int Slot { get; set; }

	[JsonPropertyName("attackername")]
	public string AttackerName { get; set; }

	[JsonPropertyName("x")]
	public double X { get; set; }

	[JsonPropertyName("y")]
	public double Y { get; set; }

	[JsonPropertyName("z")]
	public double Z { get; set; }

	[JsonPropertyName("entityleft")]
	public bool EntityLeft { get; set; }

	[JsonPropertyName("ehandle")]
	public long EHandle { get; set; }

	[JsonPropertyName("key")]
	public string Key { get; set; }

	[JsonPropertyName("player_slot")]
	public int PlayerSlot { get; set; }
}



public class Player
{
	[JsonPropertyName("player_slot")]
	public int Slot { get; set; }

	[JsonPropertyName("obs_left_log")]
	public List<ObsLeftLogEntry> ObsLeftLogs { get; set; } = [];
}



