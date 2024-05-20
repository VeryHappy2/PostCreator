export interface GeneralResponse<T> {
    flag?: boolean
    message?: string
    data?: T
}