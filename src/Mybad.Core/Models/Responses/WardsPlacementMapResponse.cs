using Mybad.Core.Models.Entries;

namespace Mybad.Core.Models.Responses;

public class WardsPlacementMapResponse : BaseResponse
{
	public List<Ward> ObserverWards { get; set; } = [];

	public List<Ward> SentryWards { get; set; } = [];
}
