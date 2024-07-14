export interface IPaginatedItemsResponse<T> {
    pageIndex: number;
    pageSize: number;
    search: string | null;
    count: number;
    data: T[];
}