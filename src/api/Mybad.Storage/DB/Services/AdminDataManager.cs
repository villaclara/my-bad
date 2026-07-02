using Microsoft.EntityFrameworkCore;
using Mybad.Core.Admin.Dashboard;

namespace Mybad.Storage.DB.Services;

internal class AdminDataManager : IDataManager
{
    private readonly ApplicationDbContext _context;
    private Dictionary<string, TableMetadata> _tablesData;

    public AdminDataManager(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        _tablesData = InitializeTablesTypes();
    }

    /// <inheritdoc />
    public async Task<TableApiResponse> GetTableData(string tableName, int page = 1, int pageSize = 100)
    {
        tableName = tableName.ToLowerInvariant();

        var set = _context.SelectDbSet(tableName).Cast<object>();

        var data = await set
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync();
        var count = await set.CountAsync();

        return new TableApiResponse(
            Data: data,
            Meta: new Pagination(
                Page: page,
                PageSize: pageSize,
                TotalCount: count,
                TotalPages: (count + pageSize - 1) / pageSize
            )
        );
    }

    /// <inheritdoc />
    public List<TableMetadata> GetTablesMetadata() =>
        [.. _tablesData.Values];

    /// <summary>
    /// Gets the actual metadata for all tables with names and columns info.
    /// </summary>
    private Dictionary<string, TableMetadata> InitializeTablesTypes() =>
        _tablesData = _context.Model.GetEntityTypes()
            .Where(x => x.GetTableName() != null)
            .Select(x =>
            {
                var name = x.GetTableName() ?? "unknown";
                var columns = x.GetProperties()
                    .Select(x =>
                        new TableColumn(x.Name, x.ClrType.Name, x.IsPrimaryKey())).ToList();

                return (new TableMetadata(name, columns));
            })
            .ToDictionary(x => x.Name)
            ?? [];
}
