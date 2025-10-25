using System.Text.Json.Serialization;
using Mybad.Core.Models.Responses;

namespace Mybad.Services.OpenDota.ApiResponseModels;

public class WardsInfo : BaseResponse
{
	[JsonPropertyName("players")]
	public List<Player> Players { get; set; } = [];
}

public class Player
{
	[JsonPropertyName("account_id")]
	public long AccountId { get; set; }

	[JsonPropertyName("player_slot")]
	public int Slot { get; set; }

	[JsonPropertyName("lane_role")]
	public int LanePos { get; set; }

	[JsonPropertyName("personaname")]
	public string Name { get; set; }

	[JsonPropertyName("obs_placed")]
	public int ObserversPlaced { get; set; }

	[JsonPropertyName("sen_placed")]
	public int SentriesPlaced { get; set; }

	[JsonPropertyName("obs_log")]
	public List<WardLogEntry> ObsLog { get; set; } = [];

	[JsonPropertyName("sen_log")]
	public List<WardLogEntry> SenLog { get; set; } = [];

	[JsonPropertyName("obs_left_log")]
	public List<WardLeftLogEntry> ObsLeftLog { get; set; } = [];

	[JsonPropertyName("sen_left_log")]
	public List<WardLeftLogEntry> SenLeftLog { get; set; } = [];

}

/// <summary>
/// Represents the obs_left_log/sen_log entry in the api response.
/// Are used for both Observer and Sentry wards.
/// </summary>
/// <remarks>
/// The objects looks like this in the json response:
/// {
///       "time": 1095,
///       "type": "obs_log",
///       "slot": 1,
///       "x": 162.5,
///       "y": 156.1,
///       "z": 130,
///       "entityleft": false,
///       "ehandle": 2951573,
///       "key": "[163,156]",
///       "player_slot": 1
///     }
/// </remarks>

public class WardLeftLogEntry
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

/// <summary>
/// Represents the obs_log/sen_log entry in the api response.
/// Are used for both Observer and Sentry wards.
/// </summary>
/// <remarks>
/// The objects looks like this in the json response:
/// {
///       "time": 1095,
///       "type": "obs_log",
///       "slot": 1,
///       "attackername": "npc_dota_hero_oracle",
///       "x": 162.5,
///       "y": 156.1,
///       "z": 130,
///       "entityleft": false,
///       "ehandle": 2951573,
///       "key": "[163,156]",
///       "player_slot": 1
///     }
/// </remarks>
public class WardLogEntry
{
	[JsonPropertyName("time")]
	public int Time { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("slot")]
	public int Slot { get; set; }

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



