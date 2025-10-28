namespace Mybad.Core.Requests;

public class WardLogRequest : BaseRequest
{
	public int MatchesCount { get; set; } = 10;
}
