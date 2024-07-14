export interface IPageItemsRequest
{
    pageIndex: number
    pageSize: number
    categoryFilter?: number | null
    searchByTitle?: string | null
    searchByUserName?: string | null
}

