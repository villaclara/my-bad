namespace Mybad.Core.Models;

public class BaseRequest
{
	public string URL { get; set; }

	public RequestType RequestType { get; set; }
}

public class WardsRequest : BaseRequest
{
	public int MatchesCount { get; set; }
}

public enum RequestType
{
	Wards = 0,
	Picks = 1,
	Unknown = 999
}
