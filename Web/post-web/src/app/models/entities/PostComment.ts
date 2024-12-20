export interface IPostComment {
    id: number | null;
    userName: string | null
    postId: number;
    content: string;
}