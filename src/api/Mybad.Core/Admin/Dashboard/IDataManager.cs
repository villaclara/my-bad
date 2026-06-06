namespace Mybad.Core.Admin.Dashboard;

public interface IDataManager
{
    List<TableMetadata> GetTablesMetadata();   // here pbly return table name + columns. 
    // But Im not sure because as I see in front i want to have tables list on the left
    // then when i select one table i got it columns while loading data. so maybe this is good anyway

    List<T> GetTableData<T>();  // here probably goes the Skip + Pagination 
    // implementation of pagination should be cursor based with continious scrolling on front end and also page based
}
