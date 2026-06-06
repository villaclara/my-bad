using Microsoft.EntityFrameworkCore;
using Mybad.Core.Admin.Dashboard;

namespace Mybad.Storage.DB.Services;

internal class AdminDataManager : IDataManager
{
    private readonly ApplicationDbContext _context;

    public AdminDataManager(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public List<T> GetTableData<T>()
    {
        throw new NotImplementedException();
    }

    public List<TableMetadata> GetTablesMetadata() =>
        [.. _context.Model.GetEntityTypes()
            .Distinct()
            .Select(x =>
                new TableMetadata(
                    x.GetTableName() ?? "unknown",
                    [.. x.GetProperties().Select(x => x.Name)]
                ))
            ];
}
