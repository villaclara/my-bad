using System.Net.Http.Json;
using Mybad.Core.Models;
using Mybad.Core.Models.Responses;
using Mybad.Core.Services;
using Mybad.Services.OpenDota.ApiResponseModels;

namespace Mybad.Services.OpenDota;

public class OpenDotaRequestService : IRequestService
{
	private static string _urlPath = "https://api.opendota.com/api/";

	public async Task<BaseResponse> GetData(BaseRequest request)
	{
		var task = request.RequestType switch
		{
			RequestType.Wards => GetWardsInfoForMatch((WardsRequest)request),
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

		try
		{
			var apiResponse = await http.GetFromJsonAsync<WardPlacementMap>(_urlPath + $"players/136996088/wardmap?limit={limit}");

			if (apiResponse == null)
			{
				throw new InvalidOperationException();
			}

			var reader = new ApiReponseConverters.WardsPlacementMapReader();

			return reader.ConvertWardsPlacementMap(apiResponse);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private async Task<BaseResponse> GetWardsInfoForMatch(WardsRequest request)
	{
		using var http = new HttpClient();
		var response = await http.GetFromJsonAsync<MatchWardLogInfo>(_urlPath + $"matches/8519566987");

		throw new NotImplementedException();
	}

	private async Task<BaseResponse> GetHeroesInfo(string url)
	{
		throw new NotImplementedException();
	}
}
