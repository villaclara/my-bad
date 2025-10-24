using Mybad.Core.Models;
using Mybad.Core.Services;
using Mybad.Services.OpenDota;

//var open = new OpenDota();
//await open.DoMatch();

IRequessService service = new WardsService();

var aa = service.GetData().Result;
var bb = (WardsResponse)aa;
Console.WriteLine(bb.Wards);
