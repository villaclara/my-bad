using Mybad.Core.Models.Entries;

namespace Mybad.Core.Models.Responses;

public class WardsLogMatchResponse
{
	public List<WardLog> ObserverWardsLog { get; set; } = [];
	public List<WardLog> SentryWardsLog { get; set; } = [];
}
