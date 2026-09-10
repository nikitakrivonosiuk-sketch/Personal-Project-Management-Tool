import { TaskPriority } from "./list/card/card-item";
import { ListItem } from "./list/list-item";

export interface BoardListDto {
    id: string;
    boardId: string;
    title: string;
    position: number;
    cards: CardDto[];
}

export interface CreateListRequestDto {
    title: string;
    boardId: string;
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

export interface CreatingBoardRequestDto {
    title: string;
}

export interface BoardDto {
    id: string;
    title: string;
    createdAt: string;
    boardLists: BoardListDto[];
}
