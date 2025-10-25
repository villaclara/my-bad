using Mybad.Core.Models;
using Mybad.Core.Models.Responses;
using Mybad.Core.Services;
using Mybad.Services.OpenDota;


IRequestService service = new OpenDotaRequestService();

//BaseRequest request = new WardsRequest
//{
//	URL = "/matches/8519566987",
//	RequestType = RequestType.Wards,
//	MatchesCount = 10,
//};
//var aa = await service.GetData(request);
//var bb = (WardsInfo)aa;
//Console.WriteLine(bb.Players.Count);

BaseRequest request1 = new WardsRequest
{
	URL = "/matches/8519566987",
	RequestType = RequestType.Unknown,
	MatchesCount = 5,
};
var aa1 = await service.GetData(request1);
var bb1 = (WardsPlacementMapResponse)aa1;
Console.WriteLine(bb1.ObserverWards.Count);
foreach (var kvp in bb1.ObserverWards)
{
	Console.WriteLine(kvp.X + " " + kvp.Y + " " + kvp.Amount);
}
