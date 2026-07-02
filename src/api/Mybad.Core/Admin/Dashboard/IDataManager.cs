namespace Mybad.Core.Admin.Dashboard;

public interface IDataManager
{
    /// <summary>
    /// Gets the metadata for all tables in the database, including their names and columns.
    /// </summary>
    /// <returns>Meta instance for all tables.</returns>
    List<TableMetadata> GetTablesMetadata();

    /// <summary>
    /// Get the data for a specific table, with pagination support.
    /// </summary>
    /// <returns>Instance of <see cref="TableApiResponse"/> containing list of data and some meta info.</returns>
    Task<TableApiResponse> GetTableData(string tableName, int page = 1, int pageSize = 100);
}
