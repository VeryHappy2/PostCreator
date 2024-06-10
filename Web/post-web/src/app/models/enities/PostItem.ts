import { PostCategory } from "./PostCategory";
import { PostComment } from "./PostComment";


export interface PostItem {
    id: number;
    title: string;
    userId: string;
    content: string;
    date: string;
    comments: Array<PostComment>;
    category: PostCategory;
}