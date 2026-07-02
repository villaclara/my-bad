namespace Mybad.Core.Admin.Dashboard;

internal class Dtos { }

/// <summary>
/// Represents a column in a database table with its name, type, and primary key status.
/// </summary>
public record TableColumn(string Name, string Type, bool IsPK);

/// <summary>
/// Represents metadata for a database table, including its name and a list of columns.
/// </summary>
public record TableMetadata(string Name, List<TableColumn> Columns);

/// <summary>
/// Represents a response from a table API, containing the data and pagination metadata.
/// </summary>
public record TableApiResponse(List<object> Data, Pagination Meta);

/// <summary>
/// Represents pagination information for API responses, including the current page, page size, total count of items, and total number of pages.
/// </summary>
public record Pagination(int Page, int PageSize, int TotalCount, int TotalPages);