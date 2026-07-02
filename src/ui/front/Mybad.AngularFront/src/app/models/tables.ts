export interface TableMetaData {
    name: string;
    columns: TableColumnMeta[];
}

export interface TableColumnMeta {
    name: string;
    type: string;
    isPK: boolean;
}

export interface TableDataApiResponse {
    data: any[],
    meta: Pagination
}

export interface Pagination {
    page: number,
    pageSize: number,
    totalCount: number,
    totalPages: number
}