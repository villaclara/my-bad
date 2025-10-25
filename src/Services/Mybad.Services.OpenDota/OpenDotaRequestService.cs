using System.Net.Http.Json;
using Mybad.Core.Models;
using Mybad.Core.Models.Responses;
using Mybad.Core.Services;
using Mybad.Services.OpenDota.ApiResponseModels;
using Mybad.Services.OpenDota.Models.Wards;

namespace Mybad.Services.OpenDota;

public class OpenDotaRequestService : IRequestService
{
	private static string _urlPath = "https://api.opendota.com/api/";

	public async Task<BaseResponse> GetData(BaseRequest request)
	{
		var task = request.RequestType switch
		{
			RequestType.Wards => GetWardsPlacementMap((WardsRequest)request),
			RequestType.Picks => GetHeroesInfo(request.URL),
			_ => GetWardsPlacementMap((WardsRequest)request)
		};

		return await task;
	}

	private async Task<BaseResponse> GetWardsPlacementMap(WardsRequest request)
	{
		var limit = 5;
		using var http = new HttpClient();
		//var response = await http.GetFromJsonAsync<WardsInfo>(_urlPath + $"players/136996088/matches?limit={request.MatchesCount}");
		var apiResponse = await http.GetFromJsonAsync<WardMapRequest>(_urlPath + $"players/136996088/wardmap?limit={limit}");

		var reader = new ApiReponseConverters.WardsPlacementMapReader();

		return reader.ConvertWardsPlacementMap(apiResponse);
	}

	private async Task<BaseResponse> GetWardsInfoForMatch(WardsRequest request)
	{
		using var http = new HttpClient();
		var response = await http.GetFromJsonAsync<WardsInfo>(_urlPath + $"players/136996088/8519566987");

		throw new NotImplementedException();
	}

	private async Task<BaseResponse> GetHeroesInfo(string url)
	{
		throw new NotImplementedException();
	}
}
