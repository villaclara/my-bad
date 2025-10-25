using System.Text.Json;
using Mybad.Services.OpenDota.ApiReponseConverters;
using Mybad.Services.OpenDota.ApiResponseModels;

namespace OpenDotaService.Tests;

public class ApiResponseConverterTests
{
	[Fact]
	public void GetWardsPlacementMap_ReturnsWardsPlacementResponse()
	{
		// Arrange 
		var apiResponseFilePath = "Data/wardmap.txt";
		var json = File.ReadAllText(apiResponseFilePath);
		var obsCount = 22;
		var distinctObsCount = 19;
		var senCount = 50;
		var distinctSenCount = 40;

		// Act 
		var data = JsonSerializer.Deserialize<WardMapRequest>(json);
		var reader = new WardsPlacementMapReader();
		var response = reader.ConvertWardsPlacementMap(data!);
		int oCount = 0, sCount = 0;
		foreach (var kvp in response.ObserverWards)
		{
			oCount += kvp.Amount;
		}

		foreach (var kvp in response.SentryWards)
		{
			sCount += kvp.Amount;
		}

		// Assert
		Assert.NotNull(response);
		Assert.Equal(distinctObsCount, response.ObserverWards.Count);
		Assert.Equal(distinctSenCount, response.SentryWards.Count);
		Assert.Equal(obsCount, oCount);
		Assert.Equal(senCount, sCount);
	}
}
