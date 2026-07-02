using Microsoft.AspNetCore.Mvc;
using Mybad.Core.Admin.Dashboard;

namespace Mybad.API.Endpoints;

public static class AdminEndpoints
{
    public static RouteGroupBuilder MapAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/data")
            .RequireCors("AllowAngularApp")
            .AddEndpointFilter<ApiKeyEndpointFilter>();

        group.MapGet("meta", GetTablesMetadata)
            .Produces(200);

        group.MapGet("table/{tableName}", GetTablesData)
            .Produces(200);

        return group;
    }

    private static IResult GetTablesMetadata(IDataManager dataManager)
    {
        return TypedResults.Ok(dataManager.GetTablesMetadata());
    }

    private static async Task<IResult> GetTablesData(
        [FromRoute] string tableName,
        IDataManager dataManager,
        [FromQuery] int? page = 1, [FromQuery] int? size = 100)
    {
        var result = await dataManager.GetTableData(tableName, page ?? 1, size ?? 100);
        return TypedResults.Ok(result);
    }
}
