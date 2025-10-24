using System.Net.Http.Json;
using Mybad.Core.Models;
using Mybad.Core.Services;

namespace Mybad.Services.OpenDota;

public class WardsService : IRequessService
{
	public async Task<BaseResponse> GetData(Type type1, string path)
	{
		using var http = new HttpClient();
		var response = await http.GetFromJsonAsync(path);

		foreach (var p in response.Players)
		{
			Console.WriteLine($"Info for player slot - {p.Slot}");
			foreach (var o in p.ObsLeftLogs)
			{
				Console.WriteLine(o.Time);
			}
		}

		var response = new WardsResponse();
		return Task.FromResult((BaseResponse)response);
	}
}
