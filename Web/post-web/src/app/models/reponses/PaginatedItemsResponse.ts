export interface PaginatedItemsResponse<T> {
    pageIndex: number;
    pageSize: number;
    search: string | null;
    count: number;
    data: T[];
}