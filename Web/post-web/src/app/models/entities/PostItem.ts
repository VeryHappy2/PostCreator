import { IPostCategory } from "./PostCategory";
import { IPostComment } from "./PostComment";
import { IPostLike } from "./PostLike";

export interface IPostItem {
    id: number;
    userName: string;
    title: string;
    userId: string;
    content: string;
    date: string;
    comments: Array<IPostComment> | null;
    category: IPostCategory;
    views: number
    likes: number
}
