import { IPostCategory } from "./PostCategory";
import { IPostComment } from "./PostComment";


export interface IPostItem {
    id: number;
    userName: string;
    title: string;
    userId: string;
    content: string;
    date: string;
    comments: Array<IPostComment> | null;
    category: IPostCategory;
}