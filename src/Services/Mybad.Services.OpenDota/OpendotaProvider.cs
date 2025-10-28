using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Mybad.Core;
using Mybad.Core.Requests;
using Mybad.Services.OpenDota.ApiResponseModels;
using Mybad.Services.OpenDota.ApiResponseReaders;

[assembly: InternalsVisibleTo("OpenDotaService.Tests")]

namespace Mybad.Services.OpenDota;

public class OpendotaProvider : IInfoProvider
{
	private static string _urlPath = "https://api.opendota.com/api/";

	public async Task<BaseResponse> GetData(BaseRequest request)
	{
		var task = request switch
		{
			WardLogMatchRequest req => GetWardsInfoForMatch(req),
			WardMapRequest req => GetWardsPlacementMap(req),
			_ => throw new Exception()
		};

		return await task;
	}

	private async Task<BaseResponse> GetWardsPlacementMap(WardMapRequest request)
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

			var reader = new WardsPlacementMapReader();

			return reader.ConvertWardsPlacementMap(apiResponse);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private async Task<BaseResponse> GetWardsInfoForMatch(WardLogMatchRequest request)
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
