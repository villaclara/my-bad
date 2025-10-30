using Mybad.Core;
using Mybad.Core.Requests;
using Mybad.Core.Responses;

namespace Mybad.Services.OpenDota.Providers;

public class ODotaWardsSingleMatchProvider : IInfoProvider<WardLogRequest, WardsLogMatchResponse>
{
	public Task<WardsLogMatchResponse> GetInfo(WardLogRequest request)
	{
		throw new NotImplementedException();
	}
}
