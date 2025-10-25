using Mybad.Core.Models.Responses;
using Mybad.Services.OpenDota.ApiResponseModels;

namespace Mybad.Services.OpenDota.ApiReponseConverters;

public class WardsPlacementMapReader
{
	public WardsPlacementMapResponse ConvertWardsPlacementMap(WardMapRequest apiReponse)
	{
		var response = new WardsPlacementMapResponse();
		foreach (var (x, innerDic) in apiReponse.Obs)
		{
			foreach (var (y, amount) in innerDic)
			{
				response.ObserverWards.Add(new Core.Models.Entries.Ward
				{
					X = int.Parse(x),
					Y = int.Parse(y),
					Amount = amount
				});
			}
		}

		foreach (var (x, innerDic) in apiReponse.Sen)
		{
			foreach (var (y, amount) in innerDic)
			{
				response.SentryWards.Add(new Core.Models.Entries.Ward
				{
					X = int.Parse(x),
					Y = int.Parse(y),
					Amount = amount,
				});
			}
		}

		return response;
	}
}
