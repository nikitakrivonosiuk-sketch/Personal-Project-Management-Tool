import { Priority } from "./card.models";

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
    priority: Priority;
}

export interface CreateCardRequestDto{
    title: string;
    description?: string;
    dueDate: string;
    priority: Priority;
    boardListId: string;
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
