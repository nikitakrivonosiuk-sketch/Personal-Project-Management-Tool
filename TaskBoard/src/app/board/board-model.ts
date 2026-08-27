import { TaskPriority } from "./list/card/card-item";

export interface BoardListDto {
    id: string;
    title: string;
    position: number;
}

export interface CardDto {
    id: string;
    title: string;
    description?: string;
    dueDate?: string;
    boardListId: string;
    boardTitle: string;
    priority: TaskPriority;
}
